using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.EntityFrameworkCore;
using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using Muzziq.Services;
using NSubstitute;
using Xunit;

namespace UnitTest.Services.RoomServiceTests
{
    public class when_leaving_room
    {
        private readonly IUtilsService _utilsService;
        private readonly IMatchService _matchService;
        private readonly Fixture _fixture;

        public when_leaving_room()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _utilsService = _fixture.Freeze<IUtilsService>();
            _matchService = _fixture.Freeze<IMatchService>();
        }

        [Fact]
        public void player_should_be_removed_from_room()
        {
            var player1 = _fixture.Create<Player>();
            var player2 = _fixture.Create<Player>();
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("MuzziqDB_LeaveRoom").Options;
            using (var context = new ApplicationDbContext(dbOptions))
            {
                context.Players.Add(player1);
                context.Players.Add(player2);
                context.SaveChanges();
                var room = _fixture.Create<Room>();
                room.Players.Add(context.Players.First(x => x.Id == player1.Id));
                room.Players.Add(context.Players.First(x => x.Id == player2.Id));
                context.Room.Add(room);
                context.SaveChanges();
                var sut = new RoomService(context, _matchService, _utilsService);

                sut.LeaveRoom(room, player1);
            }

            using (var context = new ApplicationDbContext(dbOptions))
            {
                Assert.Equal(1, context.Room.Count());
                var savedPlayer1 = context.Players.ToList().Single(item => item.Id == player1.Id);
                var savedRoom = context.Room.First();  
                Assert.Null(savedRoom.Players.FirstOrDefault(x => x.Id == savedPlayer1.Id && x.Nickname == savedPlayer1.Nickname));
            }
        }
    }
}
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
    public class when_joining_room
    {
        private readonly IUtilsService _utilsService;
        private readonly IMatchService _matchService;
        private readonly Fixture _fixture;

        public when_joining_room()
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
        public void player_should_be_added_to_room()
        {
            var playerId = _fixture.Create<int>();
            var player = _fixture.Build<Player>().With(pl => pl.Id, playerId).Create();
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("MuzziqDB_JoinRoom").Options;
            using (var context = new ApplicationDbContext(dbOptions))
            {
                _utilsService.GetPlayerById(playerId).Returns(player);
                var roomId = _fixture.Create<int>();
                var room = _fixture.Create<Room>();
                _utilsService.GetRoomById(roomId).Returns(room);
                context.Room.Add(room);
                context.Players.Add(player);
                context.SaveChanges();
                var sut = new RoomService(context, _matchService, _utilsService);

                sut.JoinRoom(roomId, playerId);
            }

            using (var context = new ApplicationDbContext(dbOptions))
            {
                Assert.Equal(1, context.Room.Count());
                var savedRoom = context.Room.First();
                var savedPlayer = context.Players.First(item => item.Id == player.Id);
                Assert.NotNull(savedRoom.Players.SingleOrDefault(x => x.Id == savedPlayer.Id && x.Nickname == savedPlayer.Nickname));
            }
        }
    }
}
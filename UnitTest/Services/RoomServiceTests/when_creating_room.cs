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
    public class when_creating_room
    {
        private readonly IUtilsService _utilsService;
        private readonly IMatchService _matchService;
        private readonly Fixture _fixture;

        public when_creating_room()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization());          
            _utilsService = _fixture.Freeze<IUtilsService>();
            _matchService = _fixture.Freeze<IMatchService>();
            
        }

        [Fact]
        public void should_return_room_with_correct_name_and_ownerId()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("MuzziqDB_CreateRoom1").Options;
            var name = _fixture.Create<string>();
            var ownerId = _fixture.Create<int>();
            using (var context = new ApplicationDbContext(dbOptions))
            {
                var player = _fixture.Build<Player>().With(pl => pl.Id, ownerId).Create();
                _utilsService.GetPlayerById(ownerId).Returns(player);
                var sut = new RoomService(context, _matchService, _utilsService);

                sut.CreateRoom(ownerId, name, _fixture.CreateMany<int>().ToArray());
            }

            using (var context = new ApplicationDbContext(dbOptions))
            {
                Assert.Equal(1, context.Room.Count());
                var result = context.Room.First();
                Assert.Equal(name, result.Name);
                Assert.Equal(ownerId, result.OwnerId);
            }
        }

        [Fact]
        public void owner_should_be_added_as_player()
        {
            var ownerId = _fixture.Create<int>();
            var owner = _fixture.Build<Player>().With(player => player.Id, ownerId).Create();
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("MuzziqDB_CreateRoom2").Options;
            using (var context = new ApplicationDbContext(dbOptions))
            {
                _utilsService.GetPlayerById(ownerId).Returns(owner);
                context.Players.Add(owner);
                var sut = new RoomService(context, _matchService, _utilsService);
                
                sut.CreateRoom(ownerId, _fixture.Create<string>(), _fixture.CreateMany<int>().ToArray());
            }

            using (var context = new ApplicationDbContext(dbOptions))
            {
                Assert.Equal(1, context.Room.Count());
                var savedRoom = context.Room.First();
                var savedPlayer = context.Players.Single(x => x.Id == ownerId);
                Assert.NotNull(savedRoom.Players.Single(pl => pl.Id == savedPlayer.Id && pl.Nickname == savedPlayer.Nickname));
            }
        }

        [Fact]
        public void matches_should_be_initialized()
        {
            Room result;
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("MuzziqDB_CreateRoom3").Options;
            using (var context = new ApplicationDbContext(dbOptions))
            {
                var ownerId = _fixture.Create<int>();
                var owner = _fixture.Build<Player>().With(player => player.Id, ownerId).Create();
                _utilsService.GetPlayerById(ownerId).Returns(owner);
                var sut = new RoomService(context, _matchService, _utilsService);

                result = sut.CreateRoom(ownerId, _fixture.Create<string>(), _fixture.CreateMany<int>().ToArray());
            }

            Assert.NotNull(result.Matches);
        }
    }
}
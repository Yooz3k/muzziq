using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Muzziq.Data;
using Xunit;

namespace IntegrationTests
{
    public class RoomRouteShould : IClassFixture<TestSetup>
    {
        private readonly HttpClient _client;
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions;

        public RoomRouteShould(TestSetup setup)
        {
            _client = setup.Client;
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=DESKTOP-EOKV5UH\\MSSQLSERVER2014;Database=MuzziqDB;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
        }

        [Fact]
        public async Task CreateNewRoom()
        {
            var fixture = new Fixture();
            var name = fixture.Create<string>();
            var data = new Dictionary<string, string>
            {
                {"name", name},
                {"songIds", fixture.CreateMany<int>().ToArray().ToString()}
            };
            var content = new FormUrlEncodedContent(data);

            await _client.PostAsync("http://localhost:8888/Room/CreateRoom", content);

            using (var context = new ApplicationDbContext(_dbOptions))
                Assert.NotNull(context.Room.SingleOrDefault(room => room.Name.Equals(name)));
            
        }
    }
}
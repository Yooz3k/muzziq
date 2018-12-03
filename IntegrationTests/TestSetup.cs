using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muzziq.Data;

namespace IntegrationTests
{
    public class TestSetup : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }
        public ApplicationDbContext Context { get; private set; }
        public TestSetup()
        {
            var builder = new WebHostBuilder()
                    .UseStartup<Muzziq.Startup>()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.SetBasePath(Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "..\\..\\..\\..\\Muzziq"));
                        config.AddJsonFile("appsettings.json");
                    });
            _server = new TestServer(builder);
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:8888");
        }
        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;
using System.Net.Http.Json;
using SampleService.Test.Mocks;

namespace SampleService.Tests.IntegrationTests
{
    public class SampleIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public SampleIntegrationTest(CustomWebApplicationFactory<Program> factory)
        {
            // setup
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanDoIntegrationTests()
        {
            var httpResponse = await _client.GetAsync("/swagger");

            httpResponse.EnsureSuccessStatusCode();

            Assert.True(httpResponse.IsSuccessStatusCode);
        }

    }
}
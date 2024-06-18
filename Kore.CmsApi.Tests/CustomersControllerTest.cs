using Kore.CmsApi.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace Kore.CmsApi.Tests
{
    public class CustomersControllerTest : IAsyncLife
    {
        private readonly Mock<ICustomersService> _customersServiceMock = new();

        private HttpClient _httpClient = null!;

        public async Task InitializeAsync()
        {
            var hostBuilder = Program.CreateHostBuilder(new string[0])
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder.UseTestServer();
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(_customersServiceMock.Object);
                });

            var host = await hostBuilder.StartAsync();
            _httpClient = host.GetTestClient();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetProfile_HappyPath()
        {
            var username = "foo";
            var profile = new FullProfile
            {
                Username = username,
                PersonalInfo = TestUtils.TestPersonalInfo,
                EmployerInfo = TestUtils.TestEmployerInfo,
            };

            _customersServiceMock.Setup(profileService => profileService.GetFullProfile(username))
                .ReturnsAsync(profile);

            var response = await _httpClient.GetAsync($"api/profile/{username}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedJson = await response.Content.ReadAsStringAsync();
            var returnedProfile = JsonConvert.DeserializeObject<FullProfile>(returnedJson);
            Assert.Equal(profile, returnedProfile);
        }

        [Fact]
        public Task GetProfile_ProfileNotFoundException_404()
        {
            return AssertThatGetFullProfileHandlesGivenException(
                givenException: new ProfileNotFoundException("foo"),
                resultingStatusCode: HttpStatusCode.NotFound);
        }
    }
}
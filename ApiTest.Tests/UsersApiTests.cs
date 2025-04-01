using System.Net;
using ApiTest.Business.Models;
using ApiTest.Clients;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace ApiTest.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("API")]
    public class UsersApiTests
    {
        private JsonPlaceholderClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new JsonPlaceholderClient();
        }

        [Test]
        public async Task GetUsers_ReturnsValidList()
        {
            var response = await _client.GetUsersAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var users = JsonConvert.DeserializeObject<List<User>>(response.Content!) 
                ?? throw new InvalidOperationException("Response content is null");
            
            Assert.Multiple(() =>
            {
                Assert.That(users, Is.Not.Empty);
                Assert.That(users[0].Id, Is.Not.EqualTo(0));
                Assert.That(users[0].Name, Is.Not.Empty);
                Assert.That(users[0].Company.Name, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Response_Has_Correct_Headers()
        {
            var response = await _client.GetUsersAsync();
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentType, Does.StartWith("application/json"));
            });
        }

        [Test]
        public async Task ValidateUsersArray()
        {
            var response = await _client.GetUsersAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(response.Content!) 
                ?? throw new InvalidOperationException("Response content is null");

            Assert.Multiple(() =>
            {
                Assert.That(users, Has.Count.EqualTo(10));
                Assert.That(users.Select(u => u.Id), Is.Unique);
                Assert.That(users.All(u => !string.IsNullOrEmpty(u.Name)));
                Assert.That(users.All(u => !string.IsNullOrEmpty(u.Company.Name)));
            });
        }

        [Test]
        public async Task CreateUser_ReturnsCreated()
        {
            var userData = new { name = "Test User", username = "testuser" };
            var response = await _client.CreateUserAsync(userData);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                var createdUser = JsonConvert.DeserializeObject<User>(response.Content!);
                Assert.That(createdUser?.Id, Is.Not.EqualTo(0));
            });
        }

        [Test]
        public async Task InvalidEndpoint_ReturnsNotFound()
        {
            var response = await _client.ExecuteAsync(new RestRequest("/invalid_endpoint"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
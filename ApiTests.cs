using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using NLog;

namespace ApiTestProject
{

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ApiTests
    {
        private RestClient _client;
       
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region Global Setup and Configuration

        [OneTimeSetUp]
        public void GlobalSetup()
        {

            var config = new NLog.Config.LoggingConfiguration();

            var consoleTarget = new NLog.Targets.ConsoleTarget("console");
            var fileTarget = new NLog.Targets.FileTarget("file")
            {
                FileName = "logs/api_tests.log",
                ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
                MaxArchiveFiles = 7
            };


            config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
            Logger.Info("GlobalSetup: Logging is configured.");
        }

        [SetUp]
        public void Setup()
        {

            _client = new RestClient("https://jsonplaceholder.typicode.com");
        }

        #endregion

        #region Builder Pattern for Request Construction

        /// <summary>

        /// </summary>
        public class RequestBuilder
        {
            private readonly RestRequest _request;

            public RequestBuilder(string resource, Method method)
            {
                _request = new RestRequest(resource, method);
            }

            public RequestBuilder AddJsonBody(object body)
            {
                _request.AddJsonBody(body);
                return this;
            }

            public RequestBuilder AddQueryParameter(string name, string value)
            {
                _request.AddQueryParameter(name, value);
                return this;
            }

            public RestRequest Build()
            {
                return _request;
            }
        }

        #endregion

        #region Business Models

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public Address Address { get; set; } = new Address();
            public string Phone { get; set; } = string.Empty;
            public string Website { get; set; } = string.Empty;
            public Company Company { get; set; } = new Company();
        }

        public class Address
        {
            public string Street { get; set; } = string.Empty;
            public string Suite { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Zipcode { get; set; } = string.Empty;
        }

        public class Company
        {
            public string Name { get; set; } = string.Empty;
        }

        #endregion

        #region API Tests


        /// Task #1.
 

        [Test, Category("API")]
        public async Task ValidateListOfUsers()
        {
            Logger.Info("Task #1: Validate that list of users can be received successfully.");
            var request = new RequestBuilder("/users", Method.Get).Build();
            var response = await _client.ExecuteAsync(request);

            Logger.Info($"Response Code: {response.StatusCode}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var users = JsonConvert.DeserializeObject<List<User>>(response.Content);
            Assert.That(users, Is.Not.Null);
            Assert.That(users!.Count, Is.GreaterThan(0));


            foreach (var user in users)
            {
                Assert.That(user.Id, Is.Not.EqualTo(0));
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Email, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Address, Is.Not.Null);
                Assert.That(user.Phone, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Website, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Company, Is.Not.Null);
            }
            Logger.Info("Task #1 passed: Users list validated successfully.");
        }


        /// Task #2.
 
        [Test, Category("API")]
        public async Task ValidateResponseHeader()
        {
            Logger.Info("Task #2: Validate response header for list of users.");
            var request = new RequestBuilder("/users", Method.Get).Build();
            var response = await _client.ExecuteAsync(request);

            Logger.Info($"Response Code: {response.StatusCode}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Используем свойство ContentType, так как оно возвращает значение заголовка Content-Type.
            var contentType = response.ContentType;
            Logger.Info($"Response Content-Type: {contentType}");
            // Проверяем, что значение начинается с "application/json"
            Assert.That(contentType, Is.Not.Null.And.Not.Empty);
            Assert.That(contentType, Does.StartWith("application/json"), "Content-Type header does not match expected value.");
            Logger.Info("Task #2 passed: Response header validated successfully.");
        }


        /// Task #3.

        [Test, Category("API")]
        public async Task ValidateUserContent()
        {
            Logger.Info("Task #3: Validate response body for list of users.");
            var request = new RequestBuilder("/users", Method.Get).Build();
            var response = await _client.ExecuteAsync(request);

            Logger.Info($"Response Code: {response.StatusCode}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var users = JsonConvert.DeserializeObject<List<User>>(response.Content);
            Assert.That(users, Is.Not.Null);
            Assert.That(users!.Count, Is.EqualTo(10), "Expected 10 users.");

            // Проверяем, что все ID уникальны.
            var distinctIds = users.Select(u => u.Id).Distinct().Count();
            Assert.That(distinctIds, Is.EqualTo(users.Count), "User IDs are not unique.");

            foreach (var user in users)
            {
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "User name is missing.");
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "Username is missing.");
                Assert.That(user.Company?.Name, Is.Not.Null.And.Not.Empty, "Company name is missing.");
            }
            Logger.Info("Task #3 passed: User content validated successfully.");
        }


        /// Task #4.

        [Test, Category("API")]
        public async Task ValidateUserCreation()
        {
            Logger.Info("Task #4: Validate that a user can be created.");
            var newUser = new { name = "Test User", username = "testuser" };
            var request = new RequestBuilder("/users", Method.Post)
                .AddJsonBody(newUser)
                .Build();

            var response = await _client.ExecuteAsync(request);
            Logger.Info($"Response Code: {response.StatusCode}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var createdUser = JsonConvert.DeserializeObject<User>(response.Content);
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser!.Id, Is.Not.EqualTo(0), "User ID is missing in the response.");
            Logger.Info("Task #4 passed: User creation validated successfully.");
        }


        /// Task #5.

        [Test, Category("API")]
        public async Task ValidateResourceNotFound()
        {
            Logger.Info("Task #5: Validate that user is notified if resource doesn’t exist.");
            var request = new RequestBuilder("/invalidendpoint", Method.Get).Build();
            var response = await _client.ExecuteAsync(request);

            Logger.Info($"Response Code: {response.StatusCode}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Logger.Info("Task #5 passed: Not Found response validated successfully.");
        }

        #endregion
    }
}

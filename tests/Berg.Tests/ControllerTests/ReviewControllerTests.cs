using Berg.Controllers.Api;
using Berg.Data;
using Berg.Models;
using Berg.Tests.Mocks;
using Microsoft.AspNetCore.Identity;
using Moq;
using RestSharp;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace Berg.Tests.ControllerTests {
    public class ReviewControllerTests {

        private readonly ReviewController _controller;
        private static readonly string API_URI = "https://localhost:44305/api/reviews/";

        public ReviewControllerTests() {
            MockDbSet<Item> itemDbSet = new MockDbSet<Item>();
            MockDbSet<ItemReview> itemReviewDbSet = new MockDbSet<ItemReview>();
            Mock<BergContext> context = new Mock<BergContext>();
            Mock<UserManager<BergUser>> userManager = new Mock<UserManager<BergUser>>();
            context.Setup(x => x.Item).Returns(itemDbSet.Object);
            context.Setup(x => x.ItemReview).Returns(itemReviewDbSet.Object);

            _controller = new ReviewController(context.Object, userManager.Object);
        }

        [Fact]
        public async Task ReviewController_GetExisting_ReturnsOk() {
            RestClient client = new RestClient(API_URI);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReviewController_GetNonexisting_ReturnsNotFound() {
            RestClient client = new RestClient(API_URI);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutTest() {

        }

        [Fact]
        public async Task DeleteTest() {

        }
    }
}

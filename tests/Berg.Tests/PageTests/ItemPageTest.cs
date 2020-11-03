using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Berg.Tests.PageTests {
    public class ItemPageTest : BergTestDataTemplate {

        public ItemPageTest() : base(DbType.SqliteInMemory) {
            SeedItems().Wait();
        }

        [Fact]
        public async Task IndexModel_OnGet_PageIsPopulated() {
            using (BergContext context = new BergContext(ContextOptions)) {
                // Arrange
                Pages.Items.IndexModel pageModel = new Pages.Items.IndexModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Assert
                List<Item> resultList = Assert.IsAssignableFrom<List<Item>>(pageModel.Item);
                Assert.Equal(
                    ITEM_LIST.OrderBy(item => item.ID),
                    resultList.OrderBy(item => item.ID)
                    );
            }
        }

        [Fact]
        public void CreateModel_OnGet_ReturnPage() {
            using (BergContext context = new BergContext(ContextOptions)) {
                // BindProperty is for POST form, not GET
                Pages.Items.CreateModel pageModel = new Pages.Items.CreateModel(context);

                IActionResult result = pageModel.OnGet();

                Assert.IsType<PageResult>(result);
            }
        }

        [Fact]
        public async Task CreateModel_OnPostInvalidModel_ReturnErrorPage() {
            string newName = TestUtilities.RandomItemName();
            decimal newPrice = TestUtilities.RNG.Next(100, 10000) / 100M;
            Item newItem = new Item(newName, newPrice);

            using (BergContext context = new BergContext(ContextOptions)) {
                HttpContext httpContext = new DefaultHttpContext();
                ModelStateDictionary modelState = new ModelStateDictionary();
                ModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
                ViewDataDictionary viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
                TempDataDictionary tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
                ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
                PageContext pageContext = new PageContext(actionContext) {
                    ViewData = viewData
                };
                Pages.Items.CreateModel pageModel = new Pages.Items.CreateModel(context) {
                    PageContext = pageContext,
                    TempData = tempData,
                    Url = new UrlHelper(actionContext)
                };
                pageModel.Item = newItem;
                pageModel.ModelState.AddModelError("Message.Text", "The text field is required");

                IActionResult result = await pageModel.OnPostAsync();

                Assert.IsType<PageResult>(result);
            }

            using (BergContext context = new BergContext(ContextOptions)) {
                // A custom query is used here as newItem is never saved to the database,
                // and so its ID field is never updated. Therefore it would be a bad idea
                // to use FindAsync with an ID.
                IQueryable<Item> query =
                    from item in context.Item
                    where item.Name.Equals(newItem.Name) &&
                    item.Price.Equals(newItem.Price)
                    select item;
                Assert.Equal(0, query.Count());
            }
        }

        [Fact]
        public async Task CreateModel_OnPostValidModel_RedirectToPage() {
            string newName = TestUtilities.RandomItemName();
            decimal newPrice = TestUtilities.RNG.Next(100, 10000) / 100M;
            Item newItem = new Item(newName, newPrice);

            using (BergContext context = new BergContext(ContextOptions)) {
                HttpContext httpContext = new DefaultHttpContext();
                ModelStateDictionary modelState = new ModelStateDictionary();
                ModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
                ViewDataDictionary viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
                TempDataDictionary tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
                ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
                PageContext pageContext = new PageContext(actionContext) {
                    ViewData = viewData
                };
                Pages.Items.CreateModel pageModel = new Pages.Items.CreateModel(context) {
                    PageContext = pageContext,
                    TempData = tempData,
                    Url = new UrlHelper(actionContext)
                };
                pageModel.Item = newItem;

                IActionResult result = await pageModel.OnPostAsync();

                Assert.IsType<RedirectToPageResult>(result);
            }

            using (BergContext context = new BergContext(ContextOptions)) {
                Item createdItem = await context.Item.FindAsync(newItem.ID);
                Assert.Equal(newItem, createdItem);
            }
        }

        [Fact]
        public async Task DetailsModel_OnGetValidItem_ReturnPage() {
            using (BergContext context = new BergContext(ContextOptions)) {
                int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count);
                Item chosenItem = ITEM_LIST[chosenIndex];
                Pages.Items.DetailsModel pageModel = new Pages.Items.DetailsModel(context);

                IActionResult result = await pageModel.OnGetAsync(chosenItem.ID);

                Assert.IsType<PageResult>(result);
                Assert.Equal(chosenItem, pageModel.Item);
            }
        }

        [Fact]
        public async Task DetailsModel_OnGetInvalidItem_ReturnNotFound() {
            using (BergContext context = new BergContext(ContextOptions)) {
                // Autogeneration uses positive indices by default
                int badIndex = -1 * TestUtilities.RNG.Next() - 1;
                Pages.Items.DetailsModel pageModel = new Pages.Items.DetailsModel(context);

                IActionResult result = await pageModel.OnGetAsync(badIndex);

                Assert.IsType<NotFoundResult>(result);
                Assert.Null(pageModel.Item);
            }
        }

        [Fact]
        public async Task EditModel_OnGetValidItem_ReturnPage() {
            using (BergContext context = new BergContext(ContextOptions)) {
                int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count);
                Item chosenItem = ITEM_LIST[chosenIndex];
                Pages.Items.EditModel pageModel = new Pages.Items.EditModel(context);

                IActionResult result = await pageModel.OnGetAsync(chosenItem.ID);

                Assert.IsType<PageResult>(result);
                Assert.Equal(chosenItem, pageModel.Item);
            }
        }

        [Fact]
        public async Task EditModel_OnGetInvalidItem_ReturnNotFound() {
            using (BergContext context = new BergContext(ContextOptions)) {
                // Autogeneration uses positive indices by default
                int badIndex = -1 * TestUtilities.RNG.Next() - 1;
                Pages.Items.EditModel pageModel = new Pages.Items.EditModel(context);

                IActionResult result = await pageModel.OnGetAsync(badIndex);

                Assert.IsType<NotFoundResult>(result);
                Assert.Null(pageModel.Item);
            }
        }

        [Fact]
        public async Task EditModel_OnPostValidModel_RedirectToPage() {

        }

        [Fact]
        public async Task EditModel_OnPostInvalidModel_ReturnErrorPage() {

        }

        [Fact]
        public async Task DeleteModel_OnGetValidItem_ReturnPage() {

        }

        [Fact]
        public async Task DeleteModel_OnGetInvalidItem_ReturnNotFound() {

        }

        [Fact]
        public async Task DeleteModel_OnPostValidItem_ReturnPage() {

        }

    }
}

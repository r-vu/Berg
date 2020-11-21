using Berg.Data;
using Berg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Berg.Tests.ModelTests {
    public class ItemReviewTest : BergTestDataTemplate {

        public ItemReviewTest() : base(DbType.SqliteInMemory) {
            SeedItems().Wait();
            SeedUsers().Wait();
            SeedReviews().Wait();
        }

        [Fact]
        public async Task ItemReview_GetAll_ContextReturnsAllAsList() {
            using (BergContext context = new BergContext(ContextOptions)) {
                // Act
                List<ItemReview> resultList = await context.ItemReview.ToListAsync();

                // Assert
                Assert.Equal(
                    REVIEW_LIST.OrderBy(x => x.ReviewId),
                    resultList.OrderBy(x => x.ReviewId)
                    );
            }
        }

        [Fact]
        public async Task ItemReview_GetAllIncludeAll_ContextReturnsAllAsList() {
            using (BergContext context = new BergContext(ContextOptions)) {
                List<ItemReview> resultList = await context.ItemReview.Include("Item").Include("Owner").ToListAsync();

                foreach (ItemReview review in resultList) {
                    Assert.NotNull(review.Item);
                    Assert.Equal(review.ItemId, review.Item.Id);
                    Assert.NotNull(review.Owner);
                    Assert.Equal(review.OwnerId, review.Owner.Id);
                }
            }
        }

        [Fact]
        public async Task ItemReview_CreateReview_ContextUpdated() {
            ItemReview newReview = new ItemReview() {
                Owner = USER_LIST[TestUtilities.RNG.Next(USER_LIST.Count)],
                Item = ITEM_LIST[TestUtilities.RNG.Next(ITEM_LIST.Count)],
                Rating = TestUtilities.RNG.NextDouble() * 5
            };

            using (BergContext context = new BergContext(ContextOptions)) {
                context.ItemReview.Attach(newReview);
                await context.SaveChangesAsync();
            }

            ItemReview savedReview;
            using (BergContext context = new BergContext(ContextOptions)) {
                savedReview = await context.ItemReview.FindAsync(newReview.ReviewId);
            }
            Assert.NotNull(savedReview);
        }

        [Fact]
        public async Task ItemReview_EditReview_ContextUpdated() {
            ItemReview chosenReview = REVIEW_LIST[TestUtilities.RNG.Next(REVIEW_LIST.Count)];
            chosenReview.Body = "edited";

            using (BergContext context = new BergContext(ContextOptions)) {
                context.Entry(chosenReview).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            ItemReview savedReview;
            using (BergContext context = new BergContext(ContextOptions)) {
                savedReview = await context.ItemReview.FindAsync(chosenReview.ReviewId);
            }
            Assert.Equal(chosenReview.Body, savedReview.Body);
        }

        [Fact]
        public async Task ItemReview_DeleteReview_ContextUpdated() {
            ItemReview chosenReview = REVIEW_LIST[TestUtilities.RNG.Next(REVIEW_LIST.Count)];

            using (BergContext context = new BergContext(ContextOptions)) {
                context.ItemReview.Remove(chosenReview);
                await context.SaveChangesAsync();
            }

            ItemReview deletedReview;
            using (BergContext context = new BergContext(ContextOptions)) {
                deletedReview = await context.ItemReview.FindAsync(chosenReview.ReviewId);
            }
            Assert.Null(deletedReview);
        }
    }
}

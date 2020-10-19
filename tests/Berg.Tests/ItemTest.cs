using Berg.Data;
using Berg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Berg.Tests {
    public class ItemTest : BergTestDataTemplate {

        [Fact]
        public async Task ItemCreateAndGetTest() {
            using (BergContext context = new BergContext(TestUtilities.TestDbContextOptions())) {

                // Arrange
                await context.Item.AddRangeAsync(ITEM_LIST);
                await context.SaveChangesAsync();

                // Act
                List<Item> resultList = await context.Item.ToListAsync();

                // Assert
                Assert.Equal(
                    ITEM_LIST.OrderBy(item => item.ID),
                    resultList.OrderBy(item => item.ID)
                    );
            }
        }

        [Fact]
        public async Task ItemEditTest() {
            using (BergContext context = new BergContext(TestUtilities.TestDbContextOptions())) {
                await context.Item.AddRangeAsync(ITEM_LIST);
                await context.SaveChangesAsync();

                string newName = TestUtilities.RandomString();
                decimal newPrice = TestUtilities.RNG.Next(100, 10000) / 100M;
                int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count());
                Item chosenItem = await context.Item.FindAsync(chosenIndex);
                chosenItem.Name = newName;
                chosenItem.Price = newPrice;
                context.Attach(chosenItem).State = EntityState.Modified;
                await context.SaveChangesAsync();

                Item resultItem = await context.Item.FindAsync(chosenIndex);
                Assert.Equal(resultItem, chosenItem);
            }
        }

        [Fact]
        public async Task ItemDeleteTest() {
            using (BergContext context = new BergContext(TestUtilities.TestDbContextOptions())) {
                await context.Item.AddRangeAsync(ITEM_LIST);
                await context.SaveChangesAsync();

                int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count());
                Item chosenItem = await context.Item.FindAsync(chosenIndex);
                context.Item.Remove(chosenItem);
                await context.SaveChangesAsync();

                Item resultItem = await context.Item.FindAsync(chosenIndex);
                Assert.Null(resultItem);
            }
        }

    }
}

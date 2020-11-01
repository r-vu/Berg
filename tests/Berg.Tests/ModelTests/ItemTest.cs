using Berg.Data;
using Berg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Berg.Tests.ModelTests {
    public class ItemTest : BergTestDataTemplate {

        public ItemTest() : base(DbType.SqliteInMemory) { }

        [Fact]
        public async Task Item_GetAll_ContextReturnsAllAsList() {
            using (BergContext context = new BergContext(ContextOptions)) {
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
        public async Task Item_EditAndSave_ContextUpdated() {
            int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count);
            Item chosenItem;

            using (BergContext context = new BergContext(ContextOptions)) {
                chosenItem = await context.Item.FindAsync(ITEM_LIST[chosenIndex].ID);
                chosenItem.Name = TestUtilities.RandomString();
                chosenItem.Price = TestUtilities.RNG.Next(100, 100000) / 100M;
                context.Attach(chosenItem).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            using (BergContext context = new BergContext(ContextOptions)) {
                Item resultItem = await context.Item.FindAsync(ITEM_LIST[chosenIndex].ID);
                Assert.Equal(chosenItem, resultItem);
            }
        }

        [Fact]
        public async Task Item_DeleteAndSave_ContextUpdated() {
            int chosenIndex = TestUtilities.RNG.Next(ITEM_LIST.Count);

            using (BergContext context = new BergContext(ContextOptions)) {
                Item chosenItem = await context.Item.FindAsync(ITEM_LIST[chosenIndex].ID);
                context.Item.Remove(chosenItem);
                await context.SaveChangesAsync();
            }

            using (BergContext context = new BergContext(ContextOptions)) {
                Item resultItem = await context.Item.FindAsync(ITEM_LIST[chosenIndex].ID);
                Assert.Null(resultItem);
            }
        }

    }
}

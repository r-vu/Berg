using Berg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Berg.Tests.ModelTests {
    public class BergUserTest : BergTestDataTemplate {

        private readonly UserManager<BergUser> _userManager;

        public BergUserTest() : base(DbType.SqliteInMemory) {
            SeedUsers().Wait();
            _userManager = serviceProvider.GetService<UserManager<BergUser>>();
        }

        [Fact]
        public void UserManager_ClassConstructed_NotNull() {
            Assert.NotNull(_userManager);
        }

        [Fact]
        public async Task BergUser_GetAll_ContextReturnsAllAsList() {
            // Act
            List<BergUser> resultList = await _userManager.Users.ToListAsync();

            // Assert
            Assert.Equal(
                USER_LIST.OrderBy(x => x.Id),
                resultList.OrderBy(x => x.Id)
            );
        }

        [Fact]
        public async Task BergUser_CreateUser_ContextUpdated() {
            BergUser newUser = new BergUser(TestUtilities.RandomUserName());
            await _userManager.CreateAsync(newUser);

            BergUser savedUser = await _userManager.FindByIdAsync(newUser.Id);
            Assert.NotNull(savedUser);
        }

        [Fact]
        public async Task BergUser_DeleteUser_ContextUpdated() {
            BergUser chosenUser = GetRandomUser();

            await _userManager.DeleteAsync(chosenUser);

            BergUser deletedUser = await _userManager.FindByIdAsync(chosenUser.Id);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task BergUser_EditUserName_ContextUpdated() {
            BergUser chosenUser = GetRandomUser();
            chosenUser.UserName = TestUtilities.RandomUserName();

            await _userManager.UpdateAsync(chosenUser);

            BergUser updatedUser = await _userManager.FindByIdAsync(chosenUser.Id);
            Assert.Equal(chosenUser.UserName, updatedUser.UserName);
        }

        [Fact]
        public async Task BergUser_DisableUser_ContextUpdated() {
            BergUser chosenUser = GetRandomUser();
            // Need to implement account disabling before this can be tested
        }

        private static BergUser GetRandomUser() {
            return USER_LIST[TestUtilities.RNG.Next(USER_LIST.Count)];
        }

    }
}

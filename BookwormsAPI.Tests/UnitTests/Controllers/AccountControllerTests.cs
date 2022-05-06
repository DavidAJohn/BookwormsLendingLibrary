using BookwormsAPI.Contracts;
using BookwormsAPI.Controllers;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class AccountControllerTests : TestBase
    {
        [Fact]
        public async Task CreateNewUser_IsSuccessful()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var context2 = BuildIdentityContext(databaseName);
            var createdUserEmail = context2.Users.FirstOrDefault().Email;

            Assert.Equal("test@test.com", createdUserEmail);
        }

        [Fact]
        public async Task RegisterNewUser_ExistingEmail_ReturnsBadRequest()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var regDetails = new RegisterDTO() { Email = "test@test.com", DisplayName = "Already Exists", Password = "Aa123456!" };
            var response = await controller.Register(regDetails);

            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task RegisterNewUser_UnregisteredEmail_ReturnsUserDTO()
        {
            var databaseName = Guid.NewGuid().ToString();

            var controller = await BuildAccountController(databaseName);
            var regDetails = new RegisterDTO() { Email = "not_previously_registered@test.com", DisplayName = "Not Regged", Password = "Aa123456!" };
            var response = await controller.Register(regDetails);

            Assert.IsType<ActionResult<UserDTO>>(response);

            Assert.Equal("not_previously_registered@test.com", response.Value.Email);
        }

        [Fact]
        public async Task Login_IncorrectPassword_ReturnsUnauthorisedResponse()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var loginDetails = new LoginDTO() { Email = "test@test.com", Password = "badPassword" };
            var response = await controller.Login(loginDetails);

            Assert.IsType<UnauthorizedObjectResult>(response.Result);
        }

        [Fact]
        public async Task Login_UnregisteredEmail_ReturnsUnauthorisedResponse()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var loginDetails = new LoginDTO() { Email = "not_registered@test.com", Password = "Aa123456!" };
            var response = await controller.Login(loginDetails);

            Assert.IsType<UnauthorizedObjectResult>(response.Result);
        }

        [Fact]
        public async Task Login_CorrectDetails_ReturnsUserDTO()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var loginDetails = new LoginDTO() { Email = "test@test.com", Password = "Aa123456!" };
            var response = await controller.Login(loginDetails);

            Assert.IsType<ActionResult<UserDTO>>(response);

            Assert.Equal("test@test.com", response.Value.Email);
        }

        [Fact]
        public async Task CheckEmailExists_UnregisteredEmail_ReturnsFalse()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var response = await controller.CheckEmailExistsAsync("not_registered@test.com");

            Assert.False(response.Value);
        }

        [Fact]
        public async Task CheckEmailExists_RegisteredEmail_ReturnsTrue()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateRegisteredUser(databaseName);

            var controller = await BuildAccountController(databaseName);
            var response = await controller.CheckEmailExistsAsync("test@test.com");

            Assert.True(response.Value);
        }


        // Helper methods
        // --------------

        private async Task CreateRegisteredUser(string databaseName)
        {
            var accountController = await BuildAccountController(databaseName);
            var user = new RegisterDTO() { Email = "test@test.com", DisplayName = "Test", Password = "Aa123456!" };
            await accountController.Register(user);
        }

        private async Task<AccountController> BuildAccountController(string databaseName)
        {
            var context = BuildIdentityContext(databaseName);

            var testUserStore = new UserStore<AppUser>(context);
            
            var testRoleStore = new RoleStore<IdentityRole>(context);
            await CreateUserRoles(BuildRoleManager(testRoleStore));

            var userManager = BuildUserManager(testUserStore);

            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);
            var signInManager = BuildSignInManager(userManager, httpcontext);

            return new AccountController(userManager, signInManager, Mock.Of<ITokenService>(), mapper);
        }

        private static async Task CreateUserRoles(RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole
            {
                Name = "ADMINISTRATOR"
            };

            await roleManager.CreateAsync(adminRole);

            var borrowerRole = new IdentityRole
            {
                Name = "BORROWER"
            };

            await roleManager.CreateAsync(borrowerRole);
        }

        private RoleManager<TRole> BuildRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;

            var roleManager = new RoleManager<TRole>(store, null, null, null, null);

            return roleManager;
        }

        private UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();

            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);

            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        private static SignInManager<TUser> BuildSignInManager<TUser>(UserManager<TUser> manager,
            HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
            IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);
            identityOptions = identityOptions ?? new IdentityOptions();

            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);

            var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);
            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;

            var sm = new SignInManager<TUser>(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;

            return sm;
        }

        private Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            var auth = new Mock<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }
    }
}

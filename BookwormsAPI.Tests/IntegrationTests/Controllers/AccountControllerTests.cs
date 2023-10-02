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
using System.Security.Claims;

namespace BookwormsAPI.Tests.IntegrationTests.Controllers
{
    public class AccountControllerTests : TestBase
    {
        private readonly string _databaseName;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DefaultHttpContext _context;

        public AccountControllerTests()
        {
            _databaseName = Guid.NewGuid().ToString();
            _context = new DefaultHttpContext();
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext = _context;
        }

        [Fact]
        public async Task Register_ReturnsUserDTO_WhenRegistrationSuccessful()
        {
            // Arrange
            var regDetails = new RegisterDTO()
            {
                Email = "not_previously_registered@test.com",
                DisplayName = "Not Regged",
                Password = "Aa123456!"
            };

            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "not_previously_registered@test.com"),
                new Claim(ClaimTypes.Role, "Borrower")
            }));

            var sut = await BuildAccountController(_databaseName, httpContext);

            // Act
            var result = (OkObjectResult)await sut.Register(regDetails);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<UserDTO>();
            result.Value.As<UserDTO>().Email.Should().Be(regDetails.Email);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, "Borrower")
            }));

            var sut = await BuildAccountController(_databaseName, httpContext);

            await CreateRegisteredUser(sut);
            var regDetails = new RegisterDTO()
            {
                Email = "test@test.com",
                DisplayName = "Already Exists",
                Password = "Aa123456!"
            };

            // Act
            var result = (BadRequestObjectResult)await sut.Register(regDetails);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Login_ReturnsUserDTO_WhenLoginIsValid()
        {
            // Arrange
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, "Borrower")
            }));

            var sut = await BuildAccountController(_databaseName, httpContext);

            await CreateRegisteredUser(sut);

            var loginDetails = new LoginDTO() { Email = "test@test.com", Password = "Aa123456!" };

            // Act
            var result = (OkObjectResult)await sut.Login(loginDetails);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<UserDTO>();
            result.Value.As<UserDTO>().Email.Should().Be(loginDetails.Email);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorisedResponse_WhenPasswordIsIncorrect()
        {
            // Arrange
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, "Borrower")
            }));

            var sut = await BuildAccountController(_databaseName, httpContext);

            await CreateRegisteredUser(sut);

            var loginDetails = new LoginDTO() { Email = "test@test.com", Password = "badPassword" };

            // Act
            var result = (UnauthorizedObjectResult)await sut.Login(loginDetails);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorisedResponse_WhenLoginDoesNotExist()
        {
            // Arrange
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, "Borrower")
            }));

            var sut = await BuildAccountController(_databaseName, httpContext);

            await CreateRegisteredUser(sut);

            var loginDetails = new LoginDTO() { Email = "not_registered@test.com", Password = "Aa123456!" };

            // Act
            var result = (UnauthorizedObjectResult)await sut.Login(loginDetails);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }


        // Helper methods
        // --------------

        private static async Task CreateRegisteredUser(AccountController sut)
        {
            var user = new RegisterDTO() { Email = "test@test.com", DisplayName = "Test", Password = "Aa123456!" };
            await sut.Register(user);
        }

        private async Task<AccountController> BuildAccountController(string databaseName, HttpContext httpContext)
        {
            var context = BuildIdentityContext(databaseName);

            var testUserStore = new UserStore<AppUser>(context);

            var testRoleStore = new RoleStore<IdentityRole>(context);
            await CreateUserRoles(BuildRoleManager(testRoleStore));

            var userManager = BuildUserManager(testUserStore);

            var mapper = BuildMap();

            httpContext ??= new DefaultHttpContext();
            MockAuth(httpContext);
            var signInManager = BuildSignInManager(userManager, httpContext);

            return new AccountController(userManager, signInManager, Mock.Of<ITokenService>(), mapper, _httpContextAccessor);
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

        private static RoleManager<TRole> BuildRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;

            var roleManager = new RoleManager<TRole>(store, null, null, null, null);

            return roleManager;
        }

        private static UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
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
            sm.Logger = logger ?? new Mock<ILogger<SignInManager<TUser>>>().Object;

            return sm;
        }

        private static IAuthenticationService MockAuth(HttpContext context)
        {
            var authService = Mock.Of<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(authService).BuildServiceProvider();
            return authService;
        }
    }
}

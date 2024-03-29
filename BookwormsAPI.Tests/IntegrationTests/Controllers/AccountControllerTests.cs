﻿using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookwormsAPI.Tests.IntegrationTests.Controllers;

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

    [Fact]
    public async Task GetCurrentUser_ShouldReturnUserDTO_WhenUserIsAuthenticated()
    {
        // Arrange
        var email = "test@test.com";

        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "Borrower")
        }));

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        //await CreateRegisteredUser(sut);

        // Act
        var result = (OkObjectResult)await sut.GetCurrentUser();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<UserDTO>();
        result.Value.As<UserDTO>().Email.Should().Be(email);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturnUnauthorisedResponse_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        // Act
        var result = (UnauthorizedObjectResult)await sut.GetCurrentUser();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task GetUserAddress_ShouldReturnAddressDTO_WhenUserIsAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, "test@test.com")
        }));

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        // Act
        var result = (OkObjectResult)await sut.GetUserAddress();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<AddressDTO>();
    }

    [Fact]
    public async Task GetUserAddress_ShouldReturnUnauthorisedResponse_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        // Act
        var result = (UnauthorizedObjectResult)await sut.GetUserAddress();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task UpdateUserAddress_ShouldReturnAddressDTO_WhenUserIsAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, "test@test.com")
        }));

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        var addressDTO = new AddressDTO()
        {
            FirstName = "Updated",
            LastName = "User",
            Street = "Test Street",
            City = "Test City",
            County = "Test County",
            PostCode = "TE5T 1NG"
        };

        // Act
        var result = (OkObjectResult)await sut.UpdateUserAddress(addressDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<AddressDTO>();
        result.Value.As<AddressDTO>().FirstName.Should().Be(addressDTO.FirstName);
    }

    [Fact]
    public async Task UpdateUserAddress_ShouldReturnUnauthorisedResponse_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        var addressDTO = new AddressDTO()
        {
            FirstName = "Updated",
            LastName = "User",
            Street = "Test Street",
            City = "Test City",
            County = "Test County",
            PostCode = "TE5T 1NG"
        };

        // Act
        var result = (UnauthorizedObjectResult)await sut.UpdateUserAddress(addressDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task UpdateUserAddress_ShouldReturnBadRequest_WhenAddressDTOIsNull()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, "test@test.com")
        }));

        var sut = await BuildAccountController(_databaseName, _httpContextAccessor.HttpContext);

        // Act
        var result = (BadRequestObjectResult)await sut.UpdateUserAddress(null);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
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
        await context.Database.EnsureCreatedAsync();
        context.Users.RemoveRange(context.Users);

        await context.Users.AddAsync(new AppUser
        {
            Email = "test@test.com",
            UserName = "test@test.com",
            DisplayName = "Test",
            Address = new Address()
            {
                FirstName = "Test",
                LastName = "User",
                Street = "Test Street",
                City = "Test City",
                County = "Test County",
                PostCode = "TE5T 1NG"
            }
        });

        var testUserStore = new UserStore<AppUser>(context);

        var testRoleStore = new RoleStore<IdentityRole>(context);
        await CreateUserRoles(BuildRoleManager(testRoleStore));

        var userManager = BuildUserManager(testUserStore);

        var mapper = BuildMap();

        httpContext ??= new DefaultHttpContext();
        MockAuth(httpContext);
        var signInManager = BuildSignInManager(userManager, httpContext);

        return new AccountController(userManager, signInManager, Substitute.For<ITokenService>(), mapper, _httpContextAccessor, Substitute.For<ILogger<AccountController>>());
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
        store ??= Substitute.For<IRoleStore<TRole>>();

        var roleManager = new RoleManager<TRole>(store, null, null, null, null);

        return roleManager;
    }

    private static UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
    {
        store ??= Substitute.For<IUserStore<TUser>>();
        var options = Substitute.For<IOptions<IdentityOptions>>();
        var idOptions = new IdentityOptions();
        idOptions.Lockout.AllowedForNewUsers = false;

        options.Value.Returns(idOptions);

        var userValidators = new List<IUserValidator<TUser>>();

        var validator = Substitute.For<IUserValidator<TUser>>();
        userValidators.Add(validator);

        var pwdValidators = new List<PasswordValidator<TUser>>
        {
            new PasswordValidator<TUser>()
        };

        var userManager = new UserManager<TUser>(store, options, new PasswordHasher<TUser>(),
            userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(), null,
            Substitute.For<ILogger<UserManager<TUser>>>());

        validator.ValidateAsync(userManager, Arg.Any<TUser>())
            .Returns(Task.FromResult(IdentityResult.Success));

        return userManager;
    }

    private static SignInManager<TUser> BuildSignInManager<TUser>(UserManager<TUser> manager,
        HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
        IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
    {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(context);
        identityOptions ??= new IdentityOptions();

        var options = Substitute.For<IOptions<IdentityOptions>>();
        options.Value.Returns(identityOptions);

        var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options);
        schemeProvider ??= Substitute.For<IAuthenticationSchemeProvider>();

        var sm = new SignInManager<TUser>(manager, contextAccessor, claimsFactory, options, null, schemeProvider, new DefaultUserConfirmation<TUser>())
        {
            Logger = logger ?? Substitute.For<ILogger<SignInManager<TUser>>>()
        };

        return sm;
    }

    private static IAuthenticationService MockAuth(HttpContext context)
    {
        var authService = Substitute.For<IAuthenticationService>();
        context.RequestServices = new ServiceCollection().AddSingleton(authService).BuildServiceProvider();
        return authService;
    }
}

using BookwormsAPI.Entities.Identity;
using BookwormsAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace BookwormsAPI.Tests.UnitTests.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppUser _user;

    public TokenServiceTests()
    {
        var inMemoryConfig = new Dictionary<string, string> {
            {"Token:Key", "£MockJwtKeyMockJwtKeyMockJwtKey$_£MockJwtKeyMockJwtKeyMockJwtKey$"},
            {"Token:Issuer", "http://testissuer"}
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfig)
            .Build();

        _user = new AppUser { Email = "test@test.com", DisplayName = "Test User" };

        var store = Substitute.For<IUserStore<AppUser>>();
        _userManager = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

        _tokenService = new TokenService(_config, _userManager);
    }

    [Fact]
    public async Task CreateToken_ShouldReturnToken_WhenInvoked()
    {
        // Act
        var token = await _tokenService.CreateToken(_user);

        // Assert
        token.Should().BeOfType<string>();
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateToken_ShouldReturnTokenWithClaims_WhenInvoked()
    {
        // Act
        var token = await _tokenService.CreateToken(_user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        securityToken.Should().NotBeNull();
        securityToken.Issuer.Should().Be(_config["Token:Issuer"]);
        securityToken.Claims.Should().NotBeEmpty();
        securityToken.Claims.Should().Contain(claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == _user.Email);
        securityToken.Claims.Should().Contain(claim => claim.Type == JwtRegisteredClaimNames.GivenName && claim.Value == _user.DisplayName);
    }
}

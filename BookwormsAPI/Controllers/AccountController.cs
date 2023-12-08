using API.Errors;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities.Identity;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ITokenService tokenService, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = await _userManager.FindUserByEmailFromClaimsPrincipal(httpContext.User);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var userDTO = new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

            return Ok(userDTO);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(httpContext.User);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            return Ok(_mapper.Map<Address, AddressDTO>(user.Address));
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDTO addressDTO)
        {
            if (addressDTO is null)
            {
                return BadRequest(new ApiResponse(400, "Invalid address supplied"));
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(httpContext.User);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            user.Address = _mapper.Map<AddressDTO, Address>(addressDTO);
            
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(_mapper.Map<Address, AddressDTO>(user.Address));
            }

            return BadRequest(new ApiResponse(400, "There was a problem updating the address"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                _logger.LogInformation("AccountController -> Login: User '{email}' not found", loginDTO.Email);
                return Unauthorized(new ApiResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                _logger.LogInformation("AccountController -> Login: Login failed for user '{email}'", loginDTO.Email);
                return Unauthorized(new ApiResponse(401));
            }

            var authenticatedUser = new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

            return Ok(authenticatedUser);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (CheckEmailExistsAsync(registerDTO.Email).Result)
            {
                return new BadRequestObjectResult(
                    new ApiValidationErrorResponse{
                        Errors = new [] { "Registration failed" }
                    }
                );
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("AccountController -> Register: Registration failed because the user '{email}' could not be created by the UserManager", user.Email);
                return BadRequest(new ApiResponse(400));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Borrower");

            if (!roleResult.Succeeded)
            {
                _logger.LogWarning("AccountController -> Register: Registration failed because the role 'Borrower' could not be added to the user '{email}' by the UserManager", user.Email);
                return BadRequest(new ApiResponse(400));
            }

            var userDTO = new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email
            };

            _logger.LogInformation("AccountController -> Register: Registration for user '{email}' was completed successfully", user.Email);

            return Ok(userDTO);
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
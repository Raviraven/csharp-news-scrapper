using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using news_scrapper.api.Attributes;
using news_scrapper.application.Data;
using news_scrapper.application.Interfaces;
using news_scrapper.domain.Models.UsersAuth;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService { get; set; }
        private IDateTimeProvider _dateTimeProvider { get; set; }

        private const string REFRESH_TOKEN_COOKIE_NAME = "refreshToken";

        public UsersController(IUserService userService, IDateTimeProvider dateTimeProvider)
        {
            _userService = userService;
            _dateTimeProvider = dateTimeProvider;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_NAME];
            var response = _userService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies[REFRESH_TOKEN_COOKIE_NAME];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            _userService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user.RefreshTokens);
        }


        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions 
            { 
                HttpOnly = true,
                Expires = _dateTimeProvider.Now.AddDays(7)
            };
            Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}

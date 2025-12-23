using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Tokens;

namespace Users.API.Controllers
{
    /// <summary>
    /// JWT üretme ve yenileme işlemlerini yapan controller
    /// </summary>
    [ApiController] 
    [Route("api/[controller]")] // Controller adı = Tokens → base route: api/tokens
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator; 
        // MediatR: Controller’ın business logic bilmeden request göndermesini sağlar

        private readonly IConfiguration _configuration; 
        // appsettings.json içindeki değerleri okumak için kullanılır

        public TokensController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Kullanıcıdan gelen bilgilerle JWT + Refresh Token üretir
        /// </summary>
        [HttpPost]
        [Route("~/api/[action]")] // action adı kullanılır → api/Token
        public async Task<IActionResult> Token(TokenRequest request)
        {
            // JWT oluşturmak için gereken config bilgileri request’e eklenir
            request.SecurityKey = _configuration["SecurityKey"];
            request.Audience = _configuration["Audience"];
            request.Issuer = _configuration["Issuer"];

            // Model valid mi kontrol edilir (ApiController otomatik validation yapar)
            if (ModelState.IsValid)
            {
                // Request MediatR üzerinden ilgili Handler’a gönderilir
                var response = await _mediator.Send(request);

                // Token üretildiyse 200 OK döner
                if (response is not null)
                    return Ok(response);

                // Kullanıcı bulunamazsa 404 döner
                return NotFound(_configuration["TokenMessage:NotFound"]);
            }

            // Model geçersizse 400 BadRequest döner
            return BadRequest(_configuration["TokenMessage:BadRequest"]);
        }

        /// <summary>
        /// Süresi dolmuş JWT için yeni JWT ve Refresh Token üretir
        /// </summary>
        [HttpPost]
        [Route("~/api/[action]")] // api/RefreshToken
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            // Token yenileme için gerekli config bilgileri eklenir
            request.SecurityKey = _configuration["SecurityKey"];
            request.Audience = _configuration["Audience"];
            request.Issuer = _configuration["Issuer"];

            if (ModelState.IsValid)
            {
                // Refresh işlemi ilgili handler’da yapılır
                var response = await _mediator.Send(request);

                // Yeni token üretildiyse 200 OK
                if (response is not null)
                    return Ok(response);

                // Refresh token geçersizse 404
                return NotFound(_configuration["TokenMessage:NotFound"]);
            }

            // Request hatalıysa 400
            return BadRequest(_configuration["TokenMessage:BadRequest"]);
        }
    }
}

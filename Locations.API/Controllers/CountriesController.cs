using CORE.APP.Models;
using Locations.APP.Features.Countries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly IMediator _mediator;

        // Logger ve Mediator bağımlılıklarını alır
        public CountriesController(ILogger<CountriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //  TÜM ÜLKELERİ GETİR
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new CountryQueryRequest());
                var list = await response.ToListAsync();

                if (list.Any())
                    return Ok(list); // veri varsa döndür
                return NoContent(); // yoksa boş dön
            }
            catch (Exception e)
            {
                _logger.LogError("CountriesGet hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Ülkeleri getirirken hata oluştu."));
            }
        }

        //  ID'YE GÖRE ÜLKE GETİR
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new CountryQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);

                if (item != null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError("CountriesGetById hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Ülke bilgisi alınamadı."));
            }
        }

        //  YENİ ÜLKE EKLE
        [HttpPost]
        public async Task<IActionResult> Post(CountryCreateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccesful)
                        return Ok(response); // başarıyla eklendi
                    ModelState.AddModelError("CountriesPost", response.Message);
                }

                // Validasyon hatalarını birleştirip döndür
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CountriesPost hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Ülke eklenirken hata oluştu."));
            }
        }

        //  ÜLKE GÜNCELLE
        [HttpPut]
        public async Task<IActionResult> Put(CountryUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccesful)
                        return Ok(response); // başarıyla güncellendi
                    ModelState.AddModelError("CountriesPut", response.Message);
                }

                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CountriesPut hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Ülke güncellenirken hata oluştu."));
            }
        }

        //  ÜLKE SİL
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new CountryDeleteRequest() { Id = id });
                if (response.IsSuccesful)
                    return Ok(response); // başarıyla silindi

                ModelState.AddModelError("CountriesDelete", response.Message);
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CountriesDelete hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Ülke silinirken hata oluştu."));
            }
        }
    }
}

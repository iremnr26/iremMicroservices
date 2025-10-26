#nullable disable
using CORE.APP.Models;
using Locations.APP.Features.Cities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IMediator _mediator;

        // Logger ve Mediator bağımlılıklarını alır
        public CitiesController(ILogger<CitiesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // TÜM ŞEHİRLERİ GETİR
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new CityQueryRequest());
                var list = await response.ToListAsync();

                if (list.Any())
                    return Ok(list); // şehir varsa listeyi döndür
                return NoContent(); // yoksa boş döndür
            }
            catch (Exception e)
            {
                _logger.LogError("CitiesGet hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Şehirler alınırken hata oluştu."));
            }
        }

        // ID'YE GÖRE ŞEHİR GETİR
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new CityQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);

                if (item != null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError("CitiesGetById hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Şehir bilgisi alınamadı."));
            }
        }

        // YENİ ŞEHİR EKLE
        [HttpPost]
        public async Task<IActionResult> Post(CityCreateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccesful)
                        return Ok(response); // başarıyla eklendi
                    ModelState.AddModelError("CitiesPost", response.Message);
                }

                // Validasyon hatalarını birleştirip döndür
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CitiesPost hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Şehir eklenirken hata oluştu."));
            }
        }

        // ŞEHİR GÜNCELLE
        [HttpPut]
        public async Task<IActionResult> Put(CityUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccesful)
                        return Ok(response); // güncellendi
                    ModelState.AddModelError("CitiesPut", response.Message);
                }

                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CitiesPut hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Şehir güncellenirken hata oluştu."));
            }
        }

        // ŞEHİR SİL
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new CityDeleteRequest() { Id = id });
                if (response.IsSuccesful)
                    return Ok(response); // silindi

                ModelState.AddModelError("CitiesDelete", response.Message);
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception e)
            {
                _logger.LogError("CitiesDelete hatası: " + e.Message);
                return StatusCode(500, new CommandResponse(false, "Şehir silinirken hata oluştu."));
            }
        }

        // BELİRLİ BİR ÜLKEYE AİT ŞEHİRLERİ GETİR
        // Örnek: GET api/Cities/GetByCountryId/3
        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            var response = await _mediator.Send(new CityQueryRequest() { CountryId = countryId });
            var list = await response.ToListAsync();

            if (list.Any())
                return Ok(list);
            return NoContent();
        }
    }
}

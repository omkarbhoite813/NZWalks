using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            // Get All Regions from Web API
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage =  await client.GetAsync("https://localhost:7061/api/regions");
                
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                //ViewBag.Response = response ;

            }
            catch (Exception ex)
            { 
            
            }
                
                return View(response);
            
        }
    }
}

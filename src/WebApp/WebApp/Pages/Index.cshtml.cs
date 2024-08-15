using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // Property to hold prices
        public List<FurnitureGroupModel> FurniturePrices { get; private set; }

        public async Task OnGetAsync()
        {
            // Load Prices
            var priceClient = _httpClientFactory.CreateClient();
            var priceResponse = priceClient.GetAsync("https://localhost:7000/FurnitureGroup/GetAllGroups").Result;

            if (priceResponse.IsSuccessStatusCode)
            {
                // Desearlize prices
                var priceString = await priceResponse.Content.ReadAsStringAsync();
                var PriceTypeResponse = JsonConvert.DeserializeObject<List<FurnitureGroupModel>>(priceString);

                if (PriceTypeResponse != null)
                {
                    FurniturePrices = PriceTypeResponse;
                }
            }
        }
    }

    public class FurnitureGroupModel
    {
        public int FurnitureGroupId { get; set; }
        public decimal FurnitureGroupPrice { get; set; }
        public bool FurnitureGroupIsDelivered { get; set; }
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.Pages;

namespace BridgmanWholesale.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly ILogger<CatalogModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogModel(ILogger<CatalogModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // Hold parameter
        public List<FurniturePieceModel> FurniturePrices { get; private set; }

        public async Task OnGetAsync()
        {
            string url = $"https://localhost:7000/FurniturePiece/GetPieceByType/{Uri.EscapeDataString(Request.Query["furnitureType"])}";

            // Load Prices
            var furnitureClient = _httpClientFactory.CreateClient();
            var furnitureResponse = furnitureClient.GetAsync(url).Result;

            if (furnitureResponse.IsSuccessStatusCode)
            {
                // Desearlize prices
                var furnitureString = await furnitureResponse.Content.ReadAsStringAsync();
                var furnitureTypeResponse = JsonConvert.DeserializeObject<List<FurniturePieceModel>>(furnitureString);

                if (furnitureTypeResponse != null)
                {
                    FurniturePrices = furnitureTypeResponse;
                }
            }
        }
        public class FurniturePieceModel
        {
            public int FurnitureId { get; set; }
            public string FurnitureName { get; set; } = "";
            public byte[] FurnitureImage { get; set; } = new byte[0];
            public int FurnitureSetPieceId { get; set; }
            public int FurniturePriceGroupId { get; set; }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.Pages;

namespace BridgmanWholesale.Pages
{
    public class ImageUploadModel : PageModel
    {
        private readonly ILogger<ImageUploadModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageUploadModel(ILogger<ImageUploadModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public List<FurnitureGroupModel> FurnitureGroupList { get; private set; }

        // Set dictionary for group and price
        public Dictionary<decimal, int> priceIntPair { get; private set; }

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
                    FurnitureGroupList = PriceTypeResponse;

                    priceIntPair = new Dictionary<decimal, int>();

                    foreach (var item in FurnitureGroupList)
                    {
                        priceIntPair.Add(item.FurnitureGroupPrice, item.FurnitureGroupId);
                    }
                    TempData["temp"] = JsonConvert.SerializeObject(priceIntPair);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(decimal FurnitureGroup, IFormFile File)
        {
            // Write byte to database
            // Set params

            // Get priceIntPair dict
            if (TempData["temp"] is string serializedObject)
            {
                priceIntPair = JsonConvert.DeserializeObject<Dictionary<decimal, int>>(serializedObject);
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                var requestUri = $"https://localhost:7000/FurnitureGroup/EditImage/{priceIntPair[FurnitureGroup]}";

                using (var content = new ByteArrayContent(await ImageToBytes(File)))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    var response = await client.PutAsync(requestUri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("Index");
                    }

                    return StatusCode(500, "Failed to process the image.");
                }
            }
        }

        // Create a function to change the image to Bytes for adding
        static async Task<byte[]> ImageToBytes(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}


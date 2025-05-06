using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
namespace TwoServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpPost]
        public async Task<ActionResult<TotalOrder>> CreateOrder([FromBody] OrderRequest request)
        {
            var newtotalORder=new TotalOrder
            {
                id = request.CustomerId,
                name = "test",
                order = "test"
            };

            //var url = $"https://localhost:7101/api/customers/{request.CustomerId}";


            //var response = await _httpClient.GetAsync(url);

            //var content = await response.Content.ReadAsStringAsync(); // reading the content
            //Console.WriteLine(content);

            //if (!response.IsSuccessStatusCode)
            //    return BadRequest("Customer not found");

            //var json = await response.Content.ReadAsStringAsync();
            //var customer = JsonSerializer.Deserialize<Customer>(json, new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //});

            return Ok(newtotalORder);
        }
    }

    public class OrderRequest
    {
        public int CustomerId { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TotalOrder
    {
        public int id { get; set; }
        public string name { get; set; }
        public string order { get; set; }

        public string ToString()
        {
            return $"Id: {id}, Name: {name}, Order: {order}";
        }
    }
}


using MassTransit;
using System.Threading.Tasks;
using TwoServices.Contracts.obj;
using TwoServices.Controllers;
using OrderRequest = TwoServices.Contracts.obj.OrderRequest;

namespace TwoServices.Setting
{
  
    public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerCreatedConsumer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var customerCreated = context.Message;

            // כאן תוכל להוסיף לוגיקה כלשהי שקשורה ליצירת הזמנה כאשר לקוח נוצר
            // לדוגמה, נשלח בקשה ל-API של הזמנות

            var httpClient = _httpClientFactory.CreateClient();

            var orderRequest = new OrderRequest(customerCreated.Id); // כאן תוכל להעביר את ה-ID של הלקוח שנוצר


            var url = "https://localhost:7101/api/orders";  // ה-API של ההזמנות שלך

            var response = await httpClient.PostAsJsonAsync(url, orderRequest);

            if (response.IsSuccessStatusCode)
            {
                var totalOrder = await response.Content.ReadFromJsonAsync<TotalOrder>();
                Console.WriteLine($"Order created for customer {customerCreated.Name}, TotalOrder: {totalOrder.ToString()}");

            }
            else
            {
                Console.WriteLine($"Failed to create order for customer {customerCreated.Name}");
            }
            

        }
    }
}

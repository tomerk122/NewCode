using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TwoServices.Contracts.obj;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{

   
    private static readonly List<Customer> Customers = new()
    {
        new Customer { Id = 1, Name = "Alice" },
        new Customer { Id = 2, Name = "Bob" },
        new Customer { Id = 3, Name = "Charlie" }
    };



    private readonly IPublishEndpoint _publishEndpoint;

    public CustomersController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    // GET api/customers/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {

        var customer = Customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
    {
        if (customer == null)
            return BadRequest();

        customer.Id = Customers.Max(c => c.Id) + 1;
        Customers.Add(customer);
        // Publish the customer creation event
        await _publishEndpoint.Publish(new CustomerCreated(customer.Id,customer.Name));

        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}
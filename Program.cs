using System.Net.Http.Headers;
using HoneyRaesAPI.Models;
List<Customer> customers = new List<Customer> {
new Customer()
{
    Name = "David",
    Id = 1,
    Address = "123 Address blvd",
},
new Customer()
{
    Name = "Stephanie",
    Id = 2,
    Address = "456 Building ave",
},
new Customer()
{
    Name = "Alejandra",
    Id = 3,
    Address = "789 Important St",
},
};
List<Employee> employees = new List<Employee> {
new Employee()
{
    Name = "Roger",
    Specialty = "IT",
    Id = 11103,
},
new Employee()
{
    Name = "Margret",
    Specialty = "Customer Service",
    Id = 11108,
},
};

List<ServiceTicket> serviceTickets = new List<ServiceTicket> {
new ServiceTicket()
{
    Id = 4,
    CustomerId = 1,
    EmployeeId = 11103,
    Description = "Having issues with internet. Need to look into new company for more stable internet connection.",
    Emergency = false,
    DateCompleted = new DateTime(2024, 6, 4),
},
new ServiceTicket()
{
    Id = 5,
    CustomerId = 3,
    EmployeeId = 11108,
    Description = "Stock is looking damaged and customer would like a refund ASAP!",
    Emergency = true,
    DateCompleted = new DateTime(2024, 8, 3),
},
new ServiceTicket()
{
    Id = 6,
    CustomerId = 2,
    EmployeeId = 11103,
    Description = "Customer is having issues with new product and needs to bring product back in for repair",
    Emergency = false,
    DateCompleted = new DateTime(2024, 8, 3),
},
new ServiceTicket()
{
    Id = 7,
    CustomerId = 3,
    EmployeeId = 11108,
    Description = "Customer got injured and we need to file a incident report",
    Emergency = true,
    DateCompleted = new DateTime(2024, 1, 5),
},
new ServiceTicket()
{
    Id = 8,
    CustomerId = 1,
    EmployeeId = 11103,
    Description = "Need to order new monitors",
    Emergency = false,
    DateCompleted = new DateTime(2024, 12, 29),
},
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/servicetickets", () =>
{
    return serviceTickets;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(e => e.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(serviceTicket);
});

app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(employee);
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(e => e.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(customer);
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);
    return serviceTicket;
});

app.MapDelete("/servicetickets/{id}", (int id) =>
{
    ServiceTicket? serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    int index = serviceTickets.IndexOf(serviceTicket);
    serviceTickets.RemoveAt(index);
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault(st => st.Id == id);
    int ticketIndex = serviceTickets.IndexOf(ticketToUpdate);
    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    //the id in the request route doesn't match the id from the ticket in the request body. That's a bad request!
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }
    serviceTickets[ticketIndex] = serviceTicket;
    return Results.Ok();
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Today;
});
app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
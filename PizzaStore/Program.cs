// import 
using Microsoft.OpenApi.Models;
using PizzaStore.DB;

// init
var builder = WebApplication.CreateBuilder(args);

// middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
  });

// build
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
}

// swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
  {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
  });

// routes
app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

// run
app.Run();

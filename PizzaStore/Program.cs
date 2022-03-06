// import 
using Microsoft.OpenApi.Models;
// using PizzaStore.DB;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

// init
var builder = WebApplication.CreateBuilder(args);

// middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
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

// PizzaDb routes
// app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
// app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
// app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
// app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
// app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

// EF Core Style routes
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizza/{id}", async (PizzaDb db, Guid id) => await db.Pizzas.FindAsync(id));
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    // pizza 物件不用傳入 Id 屬性，且不知何故，Id 會自動補足
    // pizza.Id = Guid.NewGuid(); 
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    // return 201
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});
app.MapPut("/pizza/{id}", async (PizzaDb db, Guid id, Pizza updatepizza) =>
{
    // updatepizza 物件不用包含 Id 屬性，各屬性也選填，只需包含想更新的屬性即可
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    // pizza.Name = updatepizza.Name;
    // pizza.Description = updatepizza.Description;
    if (updatepizza.Name is not null) {pizza.Name = updatepizza.Name;};
    if (updatepizza.Description is not null) {pizza.Description = updatepizza.Description;};
    await db.SaveChangesAsync();
    // return 204
    return Results.NoContent();
});
app.MapDelete("/pizza/{id}", async (PizzaDb db, Guid id) =>
{
  var pizza = await db.Pizzas.FindAsync(id);
  if (pizza is null)
  {
    return Results.NotFound();
  }
  db.Pizzas.Remove(pizza);
  await db.SaveChangesAsync();
  // return 200
  return Results.Ok();
});

// run
app.Run();

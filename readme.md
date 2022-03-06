# setup .net envirement
[source](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)

@ ubuntu 20.04 remotely

```bash
# show os version
lsb_release -a

# add dotnet packages
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# add vscode packages
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > packages.microsoft.gpg
sudo install -o root -g root -m 644 packages.microsoft.gpg /etc/apt/trusted.gpg.d/
sudo sh -c 'echo "deb [arch=amd64,arm64,armhf signed-by=/etc/apt/trusted.gpg.d/packages.microsoft.gpg] https://packages.microsoft.com/repos/code stable main" > /etc/apt/sources.list.d/vscode.list'
rm -f packages.microsoft.gpg

# Install dotnet SDK & vscode
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0 \
  sudo apt install -y code
```

@ macos

```bash
# install Homebrew
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# install code
brew install --cask visual-studio-code

# install git
brew install git

# install dotnet sdk
brew install --cask dotnet-sdk

```

init git

```bash
git config --global user.name "username"
git config --global user.email username@example.com
```

show dotnet version

```bash
dotnet --list-sdks
dotnet --version
```

# init project
[source](https://docs.microsoft.com/en-us/learn/modules/build-web-api-minimal-api/2-what-is-minimal-api)

```bash
dotnet new web -o PizzaStore -f net6.0
cd PizzaStore/
dotnet new gitignore
```

# Configure Swagger

Install Swagger Packages
```bash
dotnet add package Swashbuckle.AspNetCore
```

edit Program.cs
```c#
// import
using Microsoft.OpenApi.Models;

// add Middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
  });
app.UseSwagger();
app.UseSwaggerUI(c =>
  {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
  });
```

run
```bash
dotnet watch
```

# Use EF Core to read data

add EF Core (In Memory) package

```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet list package
```

build data model `Pizza.cs`

```c#
using Microsoft.EntityFrameworkCore;
namespace PizzaStore.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    class PizzaDb : DbContext
    {
        public PizzaDb(DbContextOptions options) : base(options) { }
        public DbSet<Pizza>? Pizzas { get; set; }
    }
}
```

add service in `Program.cs`

```c#
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
```

add CRUD routes in `Program.cs`

```c#
pp.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatepizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    pizza.Name = updatepizza.Name;
    pizza.Description = updatepizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) =>
{
  var pizza = await db.Pizzas.FindAsync(id);
  if (pizza is null)
  {
    return Results.NotFound();
  }
  db.Pizzas.Remove(pizza);
  await db.SaveChangesAsync();
  return Results.Ok();
});
```

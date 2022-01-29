
# show os version

```bash
lsb_release -a
dotnet --list-sdks
dotnet --version
```

# setup .net envirement (@ ubuntu 20.04)
[source](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)

```bash
# add .net package repository
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
# Install the SDK
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
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
```csharp
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
dotnet run
```
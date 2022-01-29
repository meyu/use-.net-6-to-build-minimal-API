// import 
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// add swagger middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
  });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
}

// add swagger api and UI
app.UseSwagger();
app.UseSwaggerUI(c =>
  {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
  });

app.MapGet("/", () => "Hello World!");

app.Run();

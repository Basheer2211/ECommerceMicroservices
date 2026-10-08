using CartServices.BLL;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Register application services, DbContext, repositories and HTTP clients
builder.Services.AddCartServices(builder.Configuration);

// Add OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map Controllers
app.MapControllers();

app.Run();

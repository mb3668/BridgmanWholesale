using Catalog.API.Data;

// Create builder

var builder = WebApplication.CreateBuilder(args);

// Get connection string and initialize the database on startup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString != null)
{
    var dbInitializer = new DataInitializer(connectionString);
    dbInitializer.Initialize();
} else
{
    throw new Exception("No valid connection String!");
}

// Accept cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7126", "http://localhost:5226")
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.UseCors();
app.Run();
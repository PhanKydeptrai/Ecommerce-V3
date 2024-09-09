using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbTestV1")));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();

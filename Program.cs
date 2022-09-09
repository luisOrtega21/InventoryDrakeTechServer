using INVENTORY.SERVER.Extensions;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7170");
                      });
});
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddDBConfiguration(builder.Configuration.GetConnectionString("Inventary.ConnectionString"));
builder.Services.AddAutoMapperConfiguration();
//builder.Services.AddCorsDtosConfiguration();
builder.Services.AddLoggerService();
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
app.UseCors(policy => policy.WithOrigins("https://localhost:7170", "http://localhost:7170")
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
.WithHeaders(HeaderNames.ContentType)
);

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

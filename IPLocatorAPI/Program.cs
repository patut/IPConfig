using System.Linq;
using IPLocatorAPI.Model;
using IPLocatorAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var allowedHosts = builder.Configuration.GetValue<string>("AppSettings:AllowedHosts");


var corsPolicyName = "allow_web_localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
                      policy =>
                      {
                          policy.WithOrigins(allowedHosts);
                      });
});

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

// Loading app settings
var databaseAddress = builder.Configuration.GetValue<string>("AppSettings:DatabaseFileAddress");
var locationRecordWeight = builder.Configuration.GetValue<uint>("AppSettings:LocationRecordWeight");
var ipRangeWeight = builder.Configuration.GetValue<uint>("AppSettings:IPRangeWeight");
var cityIndexWeight = builder.Configuration.GetValue<uint>("AppSettings:CityIndexWeight");


// Initializing and reading the databse
var dbService = new DatabaseService(databaseAddress, locationRecordWeight, ipRangeWeight, cityIndexWeight);
dbService.LoadData();

var locationService = new LocationsService(dbService);


// /ip/location?ip=123.234.123.234
app.MapGet("/ip/location", (string ip) => locationService.FilterByIp(ip));

// /city/locations?city=cit_Gbqw4
app.MapGet("/city/locations", (string city) => locationService.FilterByCity(city));

app.Run();

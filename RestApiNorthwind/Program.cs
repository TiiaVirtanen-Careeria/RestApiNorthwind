using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestApiNorthwind.Models;
using RestApiNorthwind.Services.Interfaces;
using RestApiNorthwind.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy",
       builder => builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------Connection string luetaan app settings.json tiedostosta--------------
//Dependency Injektiolla v‰litetty tietokantatieto kontrollerille
builder.Services.AddDbContext<northwindContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("paikallinen")
    //builder.Configuration.GetConnectionString("pilvi")
    ));

// ------------- tuodaan appSettings.jsoniin tekem‰mme AppSettings m‰‰ritys ------------

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

// ------------- JWT Autentikaatio ---------------------------------------------------------------

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Key);

builder.Services.AddAuthentication(au =>
{
    au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

//----------------------------jwt m‰‰ritys p‰‰ttyy-----------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("MyCorsPolicy");

app.Run();

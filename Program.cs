using System.Reflection;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using RestaurantAPI.Middleware;
using RestaurantAPI.Services.Contracts;
using NLog.Web;
using FluentValidation;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAPI.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configure Authentication and JWT Tokens
var authenticationSettings = new AuthenticationSettings();

builder.Configuration
    .GetSection("Authentication")
    .Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services
    .AddAuthentication(option => 
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer(cfg => 
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
        };
    });

// NLog: Setup NLog for DI
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Dodaj w³asn¹ politykê autoryzacji. Tutaj - czy user ma narodowoœæ
builder.Services.AddAuthorization(options => 
{
    // Drugi parametr - jakie warunki musz¹ byæ spe³nione aby user by³ dopuszczony do akcji która tak¹ politykê narzuca
    // Jako ten drugi parametr przekazujemy builder s³u¿¹cy do dynamicznego budowania polityki autoryzacji
    // Wywo³amy na nim metodê RequireClaim która zagwarantuje nam ¿e dla tej polityki jakiœ konkretny claim (tutaj Nationality)
    // ...musi istnieæ ¿eby spe³niæ wymagania.
    // wiêc dla tej polityki sprawdzamy czy claim o nazwie Nationality istnieje w tokenie JWT czy nie.
    // Wymagamy tutaj tylko istnienia claimu Nationality bez wymagañ dot. wartoœci tego claimu - on ma tylko istnieæ z jak¹kolwiek wartoœci¹
    //options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));

    // Tutaj wymagamy istnienia claimu Nationality Z wymaganiami dot. wartoœci tego claimu -
    // on ma istnieæ i mieæ wartoœci podane jako params drugiego argumentu metody RequireClaim (tutaj "Polish" i "German")
    options.AddPolicy("HasNationality", builder 
        => builder.RequireClaim("Nationality", "Polish", "German"));

    // Customowa polityka autoryzacji z w³asn¹ logik¹ (czy user ma skoñczone 20 lat)
    options.AddPolicy("AtLeast20", builder => builder.AddRequirements(new MinAgeRequirement(20)));
    
    options.AddPolicy("AtLeast2RestaurantsCreated", builder => 
        builder.AddRequirements(new MinimumRestaurantsCreatedRequirement(2)));
});

// Add services for policy handlers
builder.Services.AddScoped<IAuthorizationHandler, MinAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumRestaurantsCreatedRequirementHandler>();

// Add services to the container.
builder.Services.AddControllers();

// Add FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Add DI containers
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<RestaurantSeeder>();

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
builder.Services.AddScoped<IValidator<CreateRestaurantDto>, CreateRestaurantDtoValidator>();
builder.Services.AddScoped<IValidator<ModifyRestaurantDto>, ModifyRestaurantDtoValidator>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

// Add CORS Policy for frontend application
builder.Services.AddCors(options => 
{
    options.AddPolicy("RestaurantFrontEndClient", policyBuilder => 
    {
        policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"]);
    });
});


var app = builder.Build();
var scope = app.Services.CreateScope();

// Use CORS for RestaurantFrontEndClient
app.UseCors("RestaurantFrontEndClient");

var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
seeder.Seed();

// Configure Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant UI"));

//app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
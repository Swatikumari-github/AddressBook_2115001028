
//using BusinessLayer.Interface;
//using BusinessLayer.Service;
//using Microsoft.EntityFrameworkCore;
//using System.Text;

//using RepositoryLayer.Context;
//using RepositoryLayer.Interface;
//using RepositoryLayer;
//using AutoMapper;
//using BusinessLayer.AddressBookValidator;
//using FluentValidation;
//using BussinessLayer.Interface;
//using BussinessLayer.Service;
//using RepositoryLayer.Service;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using ModelLayer.DTO;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();

//builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
//builder.Services.AddScoped<IUserBL, UserBL>();
//builder.Services.AddScoped<IUserRL, UserRL>();

//builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterValidator>();

//builder.Services.AddAutoMapper(typeof(BusinessLayer.AutoMapperProfile.AutoMapperProfile));
//builder.Services.AddSingleton<EmailService>();
//builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });




//// Database Connection

//var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
//builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();


//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers(); //  Controllers ko register kar raha hai(UserController,v)
//});


//app.Run();
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using System.Text;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer;
using AutoMapper;
using BusinessLayer.AddressBookValidator;
using FluentValidation;
using BussinessLayer.Interface;
using BussinessLayer.Service;
using RepositoryLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTO;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Address Book services
builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();

// Register User services
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();

// Register DTO validators
builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterValidator>();

// Register AutoMapper for DTO mappings
builder.Services.AddAutoMapper(typeof(BusinessLayer.AutoMapperProfile.AutoMapperProfile));

// Register email service (if required)
builder.Services.AddSingleton<EmailService>();

// Add FluentValidation for AddressBookDTO
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();




// Authentication configuration using JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var redisConnectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddSingleton<RedisCacheService>();


// Database Connection - UserContext for user-related dataSystem.ArgumentNullException: 'Value cannot be null. (Parameter 'configuration')'

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();

// Enable Authentication and Authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Configure endpoints to map the controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Register all controllers, including AddressBookController and UserController
});

app.Run();

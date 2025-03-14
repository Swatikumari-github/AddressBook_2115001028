using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using RepositoryLayer.Context;
using RepositoryLayer.Interface;

using System.Collections.Generic;
using RepositoryLayer;
using AutoMapper;
using BusinessLayer.AddressBookValidator;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();

builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();

builder.Services.AddAutoMapper(typeof(BusinessLayer.AutoMapperProfile.AutoMapperProfile));
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();



// Database Connection

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
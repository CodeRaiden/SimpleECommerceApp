using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;
using SharedLibPackage.DependencyInjection;
using SimpleECommerce.Application.IService;
using SimpleECommerce.Infrastructure.Data;
using SimpleECommerce.Infrastructure.DependencyInjection;
using SimpleECommerce.Infrastructure.Repositories;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SharedLibPackage.Logs.Logger.Configure);

builder.Services.AddCors(options =>
{
    options.DefaultPolicyName = "CorsPolicy";
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

//// sql connection
//builder.Services.AddDbContext<SimpleECommerceDbContext>(opt =>
//  opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);
var app = builder.Build();

app.UseInfrastructurePolicy();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.Run();
using Api.Configuration;
using Api.Configurations;
using Api.Configurations.Captcha;
using Api.Configurations.Jwt;
using BaseCore.Configuration;
using BaseCore.Core.AAA;
using BaseCore.EFCore;
using BaseCore.Helper;
using BaseCore.Middlewares;
using BaseCore.Sms;
using BaseCore.Utilities;
using Core.Repositories.Base;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var _siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(_siteSetting.JwtSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
builder.Services.AddCustomApiVersioning();
builder.Services.AddScoped(typeof(IJwtService), typeof(JwtService));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ConstantItem>>().Value);
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<UrlHelper>>().Value);
builder.Services.Configure<UrlHelper>(builder.Configuration.GetSection(nameof(UrlHelper)));
builder.Services.Configure<ConstantItem>(builder.Configuration.GetSection(nameof(ConstantItem)));
builder.Services.AddScoped<ICaptchaService, CaptchaService>();

#region Mapper
var myAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "ViewModels.dll"));
var myAssembly1 = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Core.dll"));
builder.Services.InitializeAutoMapper(myAssembly, myAssembly1);
#endregion

#region SqlServer Dependencies

//// use in-memory database
//services.AddDbContext<OrderContext>(c =>
//    c.UseInMemoryDatabase("OrderConnection"));

// use real database
builder.Services.AddDbContext<BaseContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

#endregion

#region Project Dependencies

// Add Infrastructure Layer
builder.Services.AddSingleton<MemoryCache>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IDataInitializer), typeof(UserDataInitializer));
builder.Services.AddScoped(typeof(IDataInitializer), typeof(SettingDataInitializer));
builder.Services.AddScoped(typeof(ISmsService), typeof(SmsService));
builder.Services.AddTransient<GoogleService>();

builder.Services.AddScoped(ctx => UserDataValue.getUserData());
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
//here is where you set you accessor
var accessor = builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>();
UserDataValue.SetHttpContextAccessor(accessor);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

#endregion
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins(builder.Configuration["AllowedOrigins"].Split('|').ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed((host) => true);
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.IntializeDatabase();
app.UseCustomExceptionHandler();
app.UseStaticFiles();
app.UseHsts(app.Environment);
app.UseRouting();
app.UseMiddleware<CorsMiddleware>();
// Change It in Production
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

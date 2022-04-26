global using ParkyAPI.Models;
global using ParkyAPI.Repos.NationalParkRepos;
global using ParkyAPI.Repos.TrailRepos;
global using ParkyAPI.Repos.UserRepos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.MapperProfiles;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ParkyAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(provider => new MapperConfiguration(conf =>
{
    conf.AddProfile(new NationalParkProfile());
    conf.AddProfile(new TrailProfile());

}).CreateMapper());
// Repos
builder.Services.AddScoped<INationalParkRepo,NationalParkRepo>();
builder.Services.AddScoped<ITrailRepo, TrailRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
// Add services to the container.

builder.Services.AddControllers();
// API VERSION CONFIG
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
// Swagger config
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurations>();
builder.Services.AddSwaggerGen();
#region Old Swagger Gen config
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("ParkyAPI", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "Parky API",
//        Description = "An API for national parks in the us",
//        TermsOfService = new Uri("https://example.com/terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Mattias Akerstrom",
//            Url = new Uri("https://github.com/mattez1992")
//        },
//        License = new OpenApiLicense
//        {
//            Name = "MIT License",
//            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
//        }
//    });
//    //options.SwaggerDoc("NationalParks", new OpenApiInfo
//    //{
//    //    Version = "v1",
//    //    Title = "Parky API",
//    //    Description = "An API for national parks in the us",
//    //    TermsOfService = new Uri("https://example.com/terms"),
//    //    Contact = new OpenApiContact
//    //    {
//    //        Name = "Mattias Akerstrom",
//    //       Url = new Uri("https://github.com/mattez1992")
//    //    },
//    //    License = new OpenApiLicense
//    //    {
//    //        Name = "MIT License",
//    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
//    //    }
//    //});
//    //options.SwaggerDoc("Trails", new OpenApiInfo
//    //{
//    //    Version = "v1",
//    //    Title = "Parky API",
//    //    Description = "An API for trails in national parks in the us",
//    //    TermsOfService = new Uri("https://example.com/terms"),
//    //    Contact = new OpenApiContact
//    //    {
//    //        Name = "Mattias Akerstrom",
//    //        Url = new Uri("https://github.com/mattez1992")
//    //    },
//    //    License = new OpenApiLicense
//    //    {
//    //        Name = "MIT License",
//    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
//    //    }
//    //});
//    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
//    options.IncludeXmlComments(cmlCommentsFullPath);
//});
#endregion
// This will make our json web token work with authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
// create provider for API versioning
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        foreach (var desc in provider.ApiVersionDescriptions)
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                desc.GroupName.ToUpperInvariant());
    });
    #region Old Non Dynamically Swagger UI
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/ParkyAPI/swagger.json", "Parky API");
    //    //options.SwaggerEndpoint("/swagger/NationalParks/swagger.json", "Parky API");
    //    //options.SwaggerEndpoint("/swagger/Trails/swagger.json", "Parky API TRail");
    //});
    #endregion

}

app.UseHttpsRedirection();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
// add middleware to make authorization work 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

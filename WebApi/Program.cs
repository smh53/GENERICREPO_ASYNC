using Business.Abstract;
using Business.Concrete;
using Business.SignalR.Hubs;
using Business.Validation.FluentValidation;
using Core.Utilities.Mail;
using DataAccess.Context;
using DataAccess.Entities.Section;
using DataAccess.Entities.User;
using DataAccess.EntityFramework;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var allowedOrigins = "_allowedOrigins";



// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Context
builder.Services.AddDbContext<GenericRepoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

//Identity
builder.Services.AddIdentity<User, IdentityRole>(opt=>
    { 
        opt.User.RequireUniqueEmail = true;
        opt.Lockout.AllowedForNewUsers = true;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
        opt.Lockout.MaxFailedAccessAttempts = 3;
    })
    .AddEntityFrameworkStores<GenericRepoContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(2));

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
       
    };
});
//CORS policy for frontend framework
builder.Services.AddCors(options => options.AddPolicy(name: allowedOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();

    }
    ));
// Mail implentation
builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

//DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductAttachmentService, ProductAttachmentService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//SignalR
builder.Services.AddSignalR();

//FluentValidation DI
builder.Services.AddTransient<IValidator<Section>, SectorValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseWebSockets();
app.UseCors(allowedOrigins);


app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notification");
app.Run();

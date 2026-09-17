using Liftraza.Api.Middleware;
using Liftraza.Application;
using Liftraza.Infrastructure;
using Liftraza.Infrastructure.Data;
using Liftraza.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddControllers()
	.AddJsonOptions(options =>
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Paste your JWT token here"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddDbContext<LiftrazaDbContext>(options =>
	options.UseNpgsql(
		builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
	options.Password.RequireNonAlphanumeric = true;
	options.User.RequireUniqueEmail = true;
})
	.AddEntityFrameworkStores<LiftrazaDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidateAudience = true,
			ValidAudience = builder.Configuration["Jwt:Audience"],
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromMinutes(1)
		};
	});

builder.Services.AddInfrastructure(builder.Configuration);

const string frontendCorsPolicy = "Frontend";

var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]?
	.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
	?? [];

// Only wired up when origins are configured. App Service can also answer CORS at
// the platform level, and having both active would duplicate the response header.
if (allowedOrigins.Length > 0)
{
	builder.Services.AddCors(options =>
		options.AddPolicy(frontendCorsPolicy, policy =>
			policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
}

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

var enableSwagger = app.Environment.IsDevelopment()
	|| builder.Configuration.GetValue<bool>("Swagger:Enabled");

if (enableSwagger)
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (allowedOrigins.Length > 0)
{
	app.UseCors(frontendCorsPolicy);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
	name = "Liftraza API",
	status = "running",
	version = "2.0",
	swagger = "/swagger",
	documentation = "https://github.com/Sergtran/Liftraza",
	baseUrl = "https://peakset-api.azurewebsites.net"
}));

app.Run();

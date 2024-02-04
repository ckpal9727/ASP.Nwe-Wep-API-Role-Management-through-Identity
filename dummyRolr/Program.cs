using dummyRolr.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });

	// Add JWT authentication in Swagger
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "JWT Authorization",
		Description = "Enter your JWT token in the input box below.",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = JwtBearerDefaults.AuthenticationScheme
		}
	};

	c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ securityScheme, new List<string>() }
	});
});

builder.Services.AddDbContext<AppDbContext>(option =>
	option.UseSqlServer(builder.Configuration.GetConnectionString("dummyDb")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{

	option.SaveToken = true;
	option.RequireHttpsMetadata = false;
	option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration.GetSection("JwtSettings")["Issuer"],
		ValidAudience = builder.Configuration.GetSection("JwtSettings")["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings")["SecretKey"]))
	};
}
	

	)  ;

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyHeader()
			   .AllowAnyMethod();
	});

	// You can configure named policies for specific use cases
	// options.AddPolicy("MyPolicy", builder =>
	// {
	//     builder.WithOrigins("http://example.com")
	//            .AllowAnyHeader()
	//            .AllowAnyMethod();
	// });
});

        // Other services...
    



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Text;
using log4net.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleOJ.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 配置跨域
builder.Services.AddCors(options => {
    options.AddPolicy("cors",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
// 添加HTTP相关的接口
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// middleware
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// 读取jwt配置
var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json")
    .Build();
var jwtConfig = configurationRoot.GetSection("Jwt");
// 生成密钥
var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64!);
var signingKey = new SymmetricSecurityKey(keyByteArray);
// 添加认证
builder.Services.AddAuthentication("Bearer").AddJwtBearer(
    o => {
        o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = false, //是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
            ValidIssuer = jwtConfig.GetValue<string>("Issuer"), //发行人
            ValidateAudience = false, //是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
            ValidAudience = jwtConfig.GetValue<string>("Audience"), //订阅人
            ValidateLifetime = true, //是否验证过期时间，过期了就拒绝访问
            ClockSkew = TimeSpan.Zero, //这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，可以直接设置为0
            RequireExpirationTime = true
        };
    });

// Swagger添加认证
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "SimpleOJ",
            Version = "v1"
        });
    opt.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please Enter Token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// DI
builder.Services.RegisterService();

// 读取log4net配置
XmlConfigurator.Configure(new FileInfo("log4net.config"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// use forwarded headers
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

// cors
app.UseCors("cors");

// middleware
app.UseForwardedHeaders();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

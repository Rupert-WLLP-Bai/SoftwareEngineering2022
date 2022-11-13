using System.Text;
using log4net.Config;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 读取jwt配置
var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
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
            RequireExpirationTime = true,
        };
    });

// 读取log4net配置
XmlConfigurator.Configure(new FileInfo("log4net.config"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

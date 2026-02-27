using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicioWeb;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//1AxpQ+mHf7JQWVMMLomkLVPQnPfF/XQ0YPFdn7ynYu2oFnbiB+rLsLPuRR7zbj9G4jMG1DoW+RhzZhSbEQoKnsMbeQb9AZG1zBubstzmtDtf6HM7qJOmL0ZcvEdFY01afsKGc41lsMiqFl9CWO23ow==
// Add services to the container.
//string desc = AE.Decrypt("UyonCQ5cC5l1vnH/EzGYGVZPQ4gxycnkIV7/YkXDPWmlEaiybYA8HeKZmB1cscnKmdjHSp542kqxDpX0RQrmL3IaNxg09IcFgp+8HYrhUpzYvi4/Nl2yJuuRL2R1/K7Zv3V3P5zeqPX2IQrxsWZnNY9n8UBgRzOrqh4JnzMrvh3IkMkgJ2r23ZjrtdSpZaD4+GuAirr+vGTzRWb75TT9KQDhPRMAC8xo21P+funvmN7jvgfDa7eJrLwbAGzCWKCSDXuikZK/55Spy6wGYvyAhXV/fjNLW4vcEHrRcmiYsKka1m01XPtJK6bRNIifJeoX");
//string server = "workstation id=SistemaWeb.mssql.somee.com;packet size=4096;user id=u20191e814_SQLLogin_1;pwd=m9dig78f4q;data source=SistemaWeb.mssql.somee.com;persist security info=False;initial catalog=SistemaWeb;TrustServerCertificate=True";
string server = "Server=LAPTOP-BBA1QK9B\\SQLEXPRESS; user id=sa;pwd=123456789; TrustServerCertificate=True;";
string encryptar = AE.Encrypt(server);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Servicio web"


    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Bearer Authentication with JWT Token"


    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },


            },
            new List<string>()
          }
        });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateActor = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,

        ClockSkew = TimeSpan.Zero,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

});
builder.Services.AddAuthorization();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{

    builder.WithOrigins(
                        "http://localhost:7050", "https://localhost:44317"
                       )
                            .WithMethods("PUT", "DELETE", "GET", "POST");

}));
var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.Map("/Error/405", configure =>
{
    configure.Run(async context =>
    {
        HttpStatusCode statusCode = HttpStatusCode.NotFound;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        string mensaje = "{ \"Message\":\"Endpoint not found\" }";
        await context.Response.WriteAsync(mensaje);
    });
});
app.Map("/Error/404", configure =>
{
    configure.Run(async context =>
    {
        HttpStatusCode statusCode = HttpStatusCode.NotFound;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        string mensaje = "{ \"Message\":\"Endpoint not found\" }";
        await context.Response.WriteAsync(mensaje);

    });
});
app.Map("/Error/400", configure =>
{
    configure.Run(async context =>
    {
        HttpStatusCode statusCode = HttpStatusCode.BadRequest;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        string mensaje = "{ \"Message\":\"Required fields\" }";
        await context.Response.WriteAsync(mensaje);
    });
});
app.Map("/Error/401", configure =>
{
    configure.Run(async context =>
    {
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        string mensaje = "{ \"Message\":\"It's not authorized\" }";
        await context.Response.WriteAsync(mensaje);

    });
});
app.UseSwagger();
app.UseSwaggerUI(x =>
{

    x.SwaggerEndpoint("/swagger/v1/swagger.json", "swagger");
});
app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
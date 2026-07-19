using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Forma de autenticacao.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
// Parametros de validacao do token.
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quem esta solicitando.
        ValidateIssuer = true,
        // Valida quem esta recebendo.
        ValidateAudience = true,
        // Define se o tempo de expiracao sera validado.
        ValidateLifetime = true,
        // Criptografia e validacao da chave de autenticacao.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        // Valida o tempo de expiracao do token.
        ClockSkew = TimeSpan.FromMinutes(30),
        // Nome do issuer, da origem.
        ValidIssuer = "exoapi.webapi",
        // Nome do audience, para o destino.
        ValidAudience = "exoapi.webapi"
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Habilita a autenticacao.
app.UseAuthentication();

// Habilita a autorizacao.
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

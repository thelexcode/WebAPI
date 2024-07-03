using TestAPI.Data;
using TestAPI.Repository;
using TestAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurazione delle impostazioni
builder.Configuration.AddJsonFile("appsettings.json");

// Aggiungere servizi al contenitore
builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<AuthService>(); 

// Configurare CORS per consentire tutte le origini
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configurazione del middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Aggiungere il middleware per l'autenticazione JWT
app.UseAuthentication();

app.UseAuthorization();

// Aggiungere il middleware CORS
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();

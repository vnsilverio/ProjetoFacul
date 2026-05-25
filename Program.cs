using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cria as tabelas do banco de dados se não existirem
InicializarBanco();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void InicializarBanco()
{
    string banco = "Data Source=Data/petshop.db";
    using var conn = new SqliteConnection(banco);
    conn.Open();
    var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS usuario (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nome TEXT NOT NULL,
            CPF TEXT NOT NULL UNIQUE,
            Endereco TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS produto (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nome TEXT NOT NULL,
            Categoria TEXT NOT NULL,
            Preco REAL NOT NULL
        );
    ";
    cmd.ExecuteNonQuery();
}

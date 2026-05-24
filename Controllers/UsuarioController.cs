using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ProjetoFacul.Models;

namespace ProjetoFacul.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public UsuarioController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Monta o caminho absoluto do banco, independente de onde a aplicação for executada
        private string GetBanco()
        {
            string caminho = Path.Combine(_env.ContentRootPath, "Data", "petshop.db");
            return $"Data Source={caminho}";
        }

        // GET: /Usuario/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: /Usuario/Cadastro
        public IActionResult Cadastro()
        {
            return View();
        }

        // GET: /Usuario/Inserir
        public IActionResult Inserir(string nome, string cpf, string endereco)
        {
            try
            {
                using var conn = new SqliteConnection(GetBanco());
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO usuario (Nome, CPF, Endereco)
                    VALUES ($nome, $cpf, $endereco);
                ";
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$cpf", cpf);
                cmd.Parameters.AddWithValue("$endereco", endereco);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    return Json("Cadastro realizado com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum registro foi inserido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // GET: /Usuario/Consultar
        public IActionResult Consultar(string? dado)
        {
            List<object> listaUsuarios = new List<object>();

            using var conn = new SqliteConnection(GetBanco());
            conn.Open();
            var cmd = conn.CreateCommand();

            if (!string.IsNullOrEmpty(dado))
            {
                cmd.CommandText = @"
                    SELECT * FROM usuario
                    WHERE Nome LIKE $dado OR CPF LIKE $dado;
                ";
                cmd.Parameters.AddWithValue("$dado", "%" + dado + "%");
            }
            else
            {
                cmd.CommandText = "SELECT * FROM usuario;";
            }

            SqliteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Dictionary<string, object> usuario = new Dictionary<string, object>();
                usuario["id"]       = dr["Id"];
                usuario["nome"]     = dr["Nome"];
                usuario["cpf"]      = dr["CPF"];
                usuario["endereco"] = dr["Endereco"];
                listaUsuarios.Add(usuario);
            }

            return Json(listaUsuarios);
        }

        // GET: /Usuario/Alterar
        public IActionResult Alterar(string cpf, string nome, string endereco)
        {
            try
            {
                using var conn = new SqliteConnection(GetBanco());
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE usuario
                    SET Nome = $nome, Endereco = $endereco
                    WHERE CPF = $cpf;
                ";
                cmd.Parameters.AddWithValue("$cpf", cpf);
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$endereco", endereco);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    return Json("Cadastro atualizado com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum registro foi atualizado. Verifique o CPF fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // GET: /Usuario/Excluir
        public IActionResult Excluir(string cpf)
        {
            try
            {
                using var conn = new SqliteConnection(GetBanco());
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM usuario
                    WHERE CPF = $cpf;
                ";
                cmd.Parameters.AddWithValue("$cpf", cpf);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    return Json("Cadastro excluído com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum registro foi excluído. Verifique o CPF fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }
    }
}

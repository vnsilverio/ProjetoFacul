using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ProjetoFacul.Models;

namespace ProjetoFacul.Controllers
{
    public class UsuarioController : Controller
    {
        string banco = "Data Source=Data/petshop.db";

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

        // POST: /Usuario/Inserir
        public IActionResult Inserir(string nome, string senha, string endereco)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO usuario (Nome, Senha, Endereco)
                    VALUES ($nome, $senha, $endereco);
                ";
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$senha", senha);
                cmd.Parameters.AddWithValue("$endereco", endereco);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

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

            using var conn = new SqliteConnection(banco);
            conn.Open();
            var cmd = conn.CreateCommand();

            if (!string.IsNullOrEmpty(dado))
            {
                cmd.CommandText = @"
                    SELECT * FROM usuario
                    WHERE Nome LIKE $dado;
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
                usuario["id"] = dr["Id"];
                usuario["nome"] = dr["Nome"];
                usuario["senha"] = dr["Senha"];
                usuario["endereco"] = dr["Endereco"];
                listaUsuarios.Add(usuario);
            }

            return Json(listaUsuarios);
        }

        // POST: /Usuario/Alterar
        public IActionResult Alterar(string nome, string senha, string endereco)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE usuario
                    SET Senha = $senha, Endereco = $endereco
                    WHERE Nome = $nome;
                ";
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$senha", senha);
                cmd.Parameters.AddWithValue("$endereco", endereco);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas > 0)
                {
                    return Json("Cadastro atualizado com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum registro foi atualizado. Verifique o nome fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // POST: /Usuario/Excluir
        public IActionResult Excluir(string nome)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM usuario
                    WHERE Nome = $nome;
                ";
                cmd.Parameters.AddWithValue("$nome", nome);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas > 0)
                {
                    return Json("Cadastro excluído com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum registro foi excluído. Verifique o nome fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }
    }
}

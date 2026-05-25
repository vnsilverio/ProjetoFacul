using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ProjetoFacul.Models;

namespace ProjetoFacul.Controllers
{
    public class ProdutoController : Controller
    {
        string banco = "Data Source=Data/petshop.db";

        // GET: /Produto/Produtos
        public IActionResult Produtos()
        {
            return View();
        }

        // POST: /Produto/Inserir
        public IActionResult Inserir(string nome, string categoria, double preco)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO produto (Nome, Categoria, Preco)
                    VALUES ($nome, $categoria, $preco);
                ";
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$categoria", categoria);
                cmd.Parameters.AddWithValue("$preco", preco);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas > 0)
                {
                    return Json("Produto cadastrado com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum produto foi inserido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // GET: /Produto/Consultar
        public IActionResult Consultar(string? dado)
        {
            List<object> listaProdutos = new List<object>();

            using var conn = new SqliteConnection(banco);
            conn.Open();
            var cmd = conn.CreateCommand();

            if (!string.IsNullOrEmpty(dado))
            {
                cmd.CommandText = @"
                    SELECT * FROM produto
                    WHERE Nome LIKE $dado OR Categoria LIKE $dado;
                ";
                cmd.Parameters.AddWithValue("$dado", "%" + dado + "%");
            }
            else
            {
                cmd.CommandText = "SELECT * FROM produto;";
            }

            SqliteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Dictionary<string, object> produto = new Dictionary<string, object>();
                produto["id"] = dr["Id"];
                produto["nome"] = dr["Nome"];
                produto["categoria"] = dr["Categoria"];
                produto["preco"] = dr["Preco"];
                listaProdutos.Add(produto);
            }

            return Json(listaProdutos);
        }

        // POST: /Produto/Alterar
        public IActionResult Alterar(int id, string nome, string categoria, double preco)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE produto
                    SET Nome = $nome, Categoria = $categoria, Preco = $preco
                    WHERE Id = $id;
                ";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.Parameters.AddWithValue("$nome", nome);
                cmd.Parameters.AddWithValue("$categoria", categoria);
                cmd.Parameters.AddWithValue("$preco", preco);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas > 0)
                {
                    return Json("Produto atualizado com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum produto foi atualizado. Verifique o ID fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // POST: /Produto/Excluir
        public IActionResult Excluir(int id)
        {
            try
            {
                using var conn = new SqliteConnection(banco);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM produto
                    WHERE Id = $id;
                ";
                cmd.Parameters.AddWithValue("$id", id);

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas > 0)
                {
                    return Json("Produto excluído com sucesso!");
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Nenhum produto foi excluído. Verifique o ID fornecido." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }
    }
}

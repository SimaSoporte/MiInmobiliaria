using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioUsoInmueble : RepositorioBase
    {
        public RepositorioUsoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public List<UsoInmueble> Listar()
        {
            var res = new List<UsoInmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Nombre)} FROM {nameof(UsoInmueble)}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var t = new UsoInmueble
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                        res.Add(t);
                    }

                    con.Close();
                }
            }
            return res;
        }



        // TUTORIAL
        // https://www.youtube.com/watch?v=tiG71g9YnMw

        /// <summary>
        /// Retorna una List<SelectListItem> para poder llenar un DropDownList
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListarSelectListItem()
        {
            List<UsoInmueble> lst = Listar();

            List<SelectListItem> items = lst.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.Id.ToString()
                };
            });

            return items;
        }

        public UsoInmueble Obtener(int id)
        {
            UsoInmueble usoInmueble = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Nombre)} " +
                    $"FROM {nameof(UsoInmueble)} " +
                    $"WHERE id={@id}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        usoInmueble = new UsoInmueble
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }

                    con.Close();
                }
            }

            return usoInmueble;
        }

        public int Create(UsoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(UsoInmueble)} ({nameof(UsoInmueble.Nombre)}) " +
                    $"VALUES (@nombre);" +
                    $"SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Editar(UsoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(UsoInmueble)} SET {nameof(UsoInmueble.Nombre)} = @nombre " +
                    $"WHERE {nameof(UsoInmueble.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM {nameof(UsoInmueble)} WHERE {nameof(UsoInmueble.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return res;
        }

    }
}

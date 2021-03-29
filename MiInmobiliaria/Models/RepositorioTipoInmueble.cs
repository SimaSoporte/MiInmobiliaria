using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioTipoInmueble : RepositorioBase
    {
        public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public List<TipoInmueble> Listar()
        {
            var res = new List<TipoInmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(TipoInmueble.Id)}, {nameof(TipoInmueble.Nombre)} FROM {nameof(TipoInmueble)}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var t = new TipoInmueble
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
            List<TipoInmueble> lst = Listar();

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

        public TipoInmueble Obtener(int id)
        {
            TipoInmueble tipoInmueble = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(TipoInmueble.Id)}, {nameof(TipoInmueble.Nombre)} " +
                    $"FROM {nameof(TipoInmueble)} " +
                    $"WHERE id={@id}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tipoInmueble = new TipoInmueble
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }

                    con.Close();
                }
            }

            return tipoInmueble;
        }

        public int Create(TipoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(TipoInmueble)} ({nameof(TipoInmueble.Nombre)}) " +
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

        public int Editar(TipoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(TipoInmueble)} SET {nameof(TipoInmueble.Nombre)} = @nombre " +
                    $"WHERE {nameof(TipoInmueble.Id)} = @id";
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
                string sql = $"DELETE FROM {nameof(TipoInmueble)} WHERE {nameof(TipoInmueble.Id)} = @id";
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

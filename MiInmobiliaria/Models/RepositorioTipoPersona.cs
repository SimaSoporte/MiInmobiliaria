using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioTipoPersona : RepositorioBase
    {
        public RepositorioTipoPersona(IConfiguration configuration) : base(configuration)
        {
        }

        public List<TipoPersona> Listar() {
            var res = new List<TipoPersona>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(TipoPersona.Id)}, {nameof(TipoPersona.Nombre)} FROM {nameof(TipoPersona)}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        var t = new TipoPersona
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
        public TipoPersona Obtener(int id)
        {
            TipoPersona tipoPersona = null;

            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(tipoPersona.Id)}, {nameof(TipoPersona.Nombre)} " +
                    $"FROM {nameof(TipoPersona)} " +
                    $"WHERE id={@id}";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tipoPersona = new TipoPersona
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }

                    con.Close();
                }
            }

            return tipoPersona;
        }

        public int Create(TipoPersona e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(TipoPersona)} ({nameof(TipoPersona.Nombre)}) " +
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

        public int Editar(TipoPersona e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(TipoPersona)} SET {nameof(TipoPersona.Nombre)} = @nombre " +
                    $"WHERE {nameof(TipoPersona.Id)} = @id";
                using(SqlCommand cmd = new SqlCommand(sql, con))
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

        public int Borrar(int id)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM {nameof(TipoPersona)} WHERE {nameof(TipoPersona.Id)} = @id";
                using(SqlCommand cmd = new SqlCommand(sql, con))
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

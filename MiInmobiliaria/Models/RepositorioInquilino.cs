using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioInquilino : RepositorioBase
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Inquilino> Listar()
        {
            var res = new List<Inquilino>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT [id], [PersonaId], [activo], [apellido], [nombre], " +
                        $"[fechaNac], [dni], [TipoPersonaId], [email], [password], [salt], [foto], [formato], [TipoPersonaNombre] " +
                    $"FROM vInquilinos ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inquilino inquilino = null;

                    while (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(3),
                                Nombre = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5),
                                Dni = reader.GetString(6),
                                TipoPersona = new TipoPersona()
                                {
                                    Id = reader.GetInt32(7),
                                    Nombre = reader.GetString(13)
                                },
                                Email = reader.GetString(8),
                                Password = reader.GetString(9),
                                Salt = reader.GetString(10),
                                Foto = reader.GetString(11),
                                Formato = reader.GetString(12)
                            },
                            Activo = reader.GetBoolean(2)
                        };
                        res.Add(inquilino);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public Inquilino Obtener(int id)
        {
            Inquilino inquilino = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT [id], [PersonaId], [activo], [apellido], [nombre], " +
                        $"[fechaNac], [dni], [TipoPersonaId], [email], [password], [salt], [foto], [formato], [TipoPersonaNombre] " +
                    $"FROM vInquilinos " +
                    $"WHERE id = {@id} " +
                    $"ORDER BY apellido, nombre ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(3),
                                Nombre = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5),
                                Dni = reader.GetString(6),
                                TipoPersona = new TipoPersona()
                                {
                                    Id = reader.GetInt32(7),
                                    Nombre = reader.GetString(13)
                                },
                                Email = reader.GetString(8),
                                Password = reader.GetString(9),
                                Salt = reader.GetString(10),
                                Foto = reader.GetString(11),
                                Formato = reader.GetString(12)
                            },
                            Activo = reader.GetBoolean(2)
                        };
                    }
                    con.Close();
                }
            }
            return inquilino;
        }

        public int Create(Inquilino e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(Inquilino)} ( " +
                    $"PersonaId, {nameof(Inquilino.Activo)} ) " +
                    $"VALUES ( @PersonaId, @activo); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    cmd.Parameters.AddWithValue("@PersonaId", e.Persona.Id);
                    cmd.Parameters.AddWithValue("@activo", e.Activo);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Editar(Inquilino e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(Inquilino)} SET {nameof(Inquilino.Activo)} = @activo " +
                    $"WHERE {nameof(Inquilino.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@activo", e.Activo);
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
                string sql = $"DELETE FROM {nameof(Inquilino)} WHERE {nameof(Inquilino.Id)} = @id";
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

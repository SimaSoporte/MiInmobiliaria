using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RespositorioPropietario : RepositorioBase
    {
        public RespositorioPropietario(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Propietario> Listar()
        {
            var res = new List<Propietario>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT [id], [idPersona], [activo], [apellido], [nombre], " +
                        $"[fechaNac], [dni], [idTipoPersona], [email], [password], [salt], [foto], [formato], [nombreTipoPersona] " +
                    $"FROM vPropietarios ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Propietario propietario = null;

                    while (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(3),
                                Nombre = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5),
                                Dni = reader.GetString(6),
                                Tipo = new TipoPersona()
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
                        res.Add(propietario);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public Propietario Obtener(int id)
        {
            Propietario propietario = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT [id], [idPersona], [activo], [apellido], [nombre], " +
                        $"[fechaNac], [dni], [idTipoPersona], [email], [password], [salt], [foto], [formato], [nombreTipoPersona] " +
                    $"FROM vPropietarios ORDER BY apellido, nombre " +
                    $"WHERE id = @id";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(3),
                                Nombre = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5),
                                Dni = reader.GetString(6),
                                Tipo = new TipoPersona()
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
                        con.Close();
                    }
                }
            }
            return propietario;
        }

        public int Create(Propietario e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(Propietario)} ( {nameof(Propietario.Id)}, " +
                    $"{nameof(Propietario.Persona)}, {nameof(Propietario.Activo)} ) " +
                    $"VALUES ( @id, @id_persona, @activo); " +
                    $"SELECT SPOCE_IDENTITY()";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    cmd.Parameters.AddWithValue("@idPersona", e.Persona.Id);
                    cmd.Parameters.AddWithValue("@activo", e.Activo);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }
    }
}

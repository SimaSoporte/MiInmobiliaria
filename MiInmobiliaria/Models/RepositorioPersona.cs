using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioPersona : RepositorioBase
    {
        public RepositorioPersona(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Persona> Listar()
        {
            var res = new List<Persona>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT P.[id], [apellido], [nombre], [fechaNac], [dni], [idTipoPersona], T.[nombre] AS nombreTipoPersona, " +
                    $"[email], [password], [salt], [foto], [formato] " +
                    $"FROM {nameof(Persona)} P " +
                    $"  INNER JOIN TipoPersona T ON P.idTipoPersona = T.id" +
                    $"ORDER BY {nameof(Persona.Apellido)}, {nameof(Persona.Nombre)}";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Persona persona = null;

                    while (reader.Read())
                    {
                        persona = new Persona
                        {
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            FechaNac = reader.GetDateTime(3),
                            Dni = reader.GetString(4),
                            Tipo = new TipoPersona()
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(6)
                            },
                            Email = reader.GetString(7),
                            Password = reader.GetString(8),
                            Salt = reader.GetString(9),
                            Foto = reader.GetString(10),
                            Formato = reader.GetString(11)
                        };
                        res.Add(persona);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public Persona Obtener(int id)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT P.[id], [apellido], [nombre], [fechaNac], [dni], [idTipoPersona], T.[nombre] AS nombreTipoPersona, " +
                    $"[email], [password], [salt], [foto], [formato] " +
                    $"FROM {nameof(Persona)} P " +
                    $"  INNER JOIN TipoPersona T ON P.idTipoPersona = T.id " +
                    $"WHERE P.{nameof(Persona.Id)} = @id " +
                    $"ORDER BY {nameof(Persona.Apellido)}, {nameof(Persona.Nombre)}";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        persona = new Persona
                        {
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            FechaNac = reader.GetDateTime(3),
                            Dni = reader.GetString(4),
                            Tipo = new TipoPersona()
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(6)
                            },
                            Email = reader.GetString(7),
                            Password = reader.GetString(8),
                            Salt = reader.GetString(9),
                            Foto = reader.GetString(10),
                            Formato = reader.GetString(11)
                        };
                    }
                    con.Close();
                }
            }
            return persona;
        }

        public int Create(Persona e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(Persona)} ( {nameof(Persona.Apellido)}, {nameof(Persona.Nombre)}, " +
                    $"{nameof(Persona.FechaNac)}, {nameof(Persona.Dni)}, idTipoPersona, {nameof(Persona.Email)}, " +
                    $"{nameof(Persona.Password)}, {nameof(Persona.Salt)}, {nameof(Persona.Foto)}, {nameof(Persona.Formato)} ) " +
                    $"VALUES ( @apellido, @nombre, @fechaNac, @dni, @idTipoPersona, @email, @password, @salt, @foto, @formato); " +
                    $"SELECT SCOPE_IDENTITY();";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@apellido", e.Apellido);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }
    }
}

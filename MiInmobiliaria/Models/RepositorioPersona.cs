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

        //ALTER TABLE Persona
        //ADD CONSTRAINT fk_PersonaTipoPersonaId FOREIGN KEY(TipoPersonaId) REFERENCES TipoPersona(Id);

        //ALTER TABLE Agencia
        //ADD CONSTRAINT fk_AgenciaPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

        //ALTER TABLE Propietario
        //ADD CONSTRAINT fk_PropietarioPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

        //ALTER TABLE Inquilino
        //ADD CONSTRAINT fk_InquilinoPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

        //ALTER TABLE Garante
        //ADD CONSTRAINT fk_GarantePersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

        //exec sp_fkeys 'TipoPersona';

        //ALTER TABLE Persona DROP CONSTRAINT fk_TipoPersonaId;


        public List<Persona> Listar()
        {
            var res = new List<Persona>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT P.[id], P.[apellido], P.[nombre], P.[fechaNac], P.[dni], P.[TipoPersonaId], T.[nombre] AS TipoPersonaNombre, " +
                    $"P.[email], P.[password], P.[salt], P.[foto], P.[formato] " +
                    $"FROM {nameof(Persona)} P " +
                    $"  INNER JOIN TipoPersona T ON P.TipoPersonaId = T.id " +
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
                            TipoPersona = new TipoPersona()
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
                string sql = $"SELECT P.[id], [apellido], P.[nombre], [fechaNac], [dni], [TipoPersonaId], T.[nombre] AS TipoPersonaNombre, " +
                    $"[email], [password], [salt], [foto], [formato] " +
                    $"FROM {nameof(Persona)} P " +
                    $"  INNER JOIN TipoPersona T ON P.TipoPersonaID = T.id " +
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
                            TipoPersona = new TipoPersona()
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
        public Persona Obtener(string dni, string email)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT P.[id], P.[apellido], P.[nombre], [fechaNac], [dni], [TipoPersonaId], T.[nombre] AS TipoPersonaNombre, " +
                    $"[email], [password], [salt], [foto], [formato] " +
                    $"FROM {nameof(Persona)} P " +
                    $"  INNER JOIN TipoPersona T ON P.TipoPersonaId = T.id " +
                    $"WHERE P.{nameof(Persona.Dni)} = @dni OR P.{nameof(Persona.Email)} = @email " +
                    $"ORDER BY {nameof(Persona.Apellido)}, P.{nameof(Persona.Nombre)}";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.Parameters.AddWithValue("@email", email);
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
                            TipoPersona = new TipoPersona()
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
                    $"{nameof(Persona.FechaNac)}, {nameof(Persona.Dni)}, TipoPersonaId, {nameof(Persona.Email)}, " +
                    $"{nameof(Persona.Password)}, {nameof(Persona.Salt)}, {nameof(Persona.Foto)}, {nameof(Persona.Formato)} ) " +
                    $"VALUES ( @apellido, @nombre, @fechaNac, @dni, @TipoPersonaId, @email, @password, @salt, @foto, @formato); " +
                    $"SELECT SCOPE_IDENTITY();";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@apellido", e.Apellido);
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@fechaNac", e.FechaNac);
                    cmd.Parameters.AddWithValue("@dni", e.Dni);
                    cmd.Parameters.AddWithValue("@TipoPersonaId", e.TipoPersona.Id);
                    cmd.Parameters.AddWithValue("@email", e.Email);
                    cmd.Parameters.AddWithValue("@password", e.Password);
                    cmd.Parameters.AddWithValue("@salt", e.Salt);
                    cmd.Parameters.AddWithValue("@foto", e.Foto);
                    cmd.Parameters.AddWithValue("@formato", e.Formato);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Editar(Persona e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(Persona)} SET " +
                        $"{nameof(Persona.Apellido)} = @apellido, " +
                        $"{nameof(Persona.Nombre)} = @nombre, " +
                        $"{nameof(Persona.FechaNac)} = @fechaNac, " +
                        $"{nameof(Persona.Dni)} = @dni, " +
                        $"TipoPersonaId = @TipoPersonaId, " +
                        $"{nameof(Persona.Email)} = @email, " +
                        $"{nameof(Persona.Foto)} =  @foto, " +
                        $"{nameof(Persona.Formato)} = @formato " +
                    $"WHERE {nameof(Persona.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@apellido", e.Apellido);
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@fechaNac", e.FechaNac);
                    cmd.Parameters.AddWithValue("@dni", e.Dni);
                    cmd.Parameters.AddWithValue("@TipoPersonaId", e.TipoPersona.Id);
                    cmd.Parameters.AddWithValue("@email", e.Email);
                    cmd.Parameters.AddWithValue("@foto", e.Foto);
                    cmd.Parameters.AddWithValue("@formato", e.Formato);
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
                string sql = $"DELETE FROM {nameof(Persona)} WHERE {nameof(Persona.Id)} = @id";
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

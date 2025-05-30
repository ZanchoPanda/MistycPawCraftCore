using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Utils.Enums;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MistycPawCraftCore.DDBB
{
    public class EventoRepository
    {

        public static void InsertEvento(EventoDTO evento)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Eventos (Nombre, FechaHora, Ubicacion, TipoEvento, Fuente, Link, RecordatorioActivado)
                                 VALUES (@Nombre, @FechaHora, @Ubicacion, @TipoEvento, @Fuente, @Link, @RecordatorioActivado)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", evento.Nombre);
                    cmd.Parameters.AddWithValue("@FechaHora", evento.FechaHora);
                    cmd.Parameters.AddWithValue("@Ubicacion", evento.Ubicacion);
                    cmd.Parameters.AddWithValue("@TipoEvento", evento.TipoEvento);
                    cmd.Parameters.AddWithValue("@Fuente", evento.Fuente);
                    cmd.Parameters.AddWithValue("@Link", evento.Link);
                    cmd.Parameters.AddWithValue("@RecordatorioActivado", evento.RecordatorioActivado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<EventoDTO> GetEventos()
        {
            var eventos = new List<EventoDTO>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Eventos ORDER BY FechaHora ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventos.Add(new EventoDTO
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            FechaHora = reader.GetDateTime(2),
                            Ubicacion = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            TipoEvento = (EnumFormatoJuego)reader.GetInt32(4),
                            Fuente = reader.GetString(5),
                            Link = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            RecordatorioActivado = reader.GetBoolean(7)
                        });
                    }
                }
            }
            return eventos;
        }

        public static void UpdateEvento(EventoDTO evento)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Eventos 
                                 SET Nombre = @Nombre, FechaHora = @FechaHora, Ubicacion = @Ubicacion, 
                                     TipoEvento = @TipoEvento, Fuente = @Fuente, Link = @Link, RecordatorioActivado = @RecordatorioActivado 
                                 WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", evento.Id);
                    cmd.Parameters.AddWithValue("@Nombre", evento.Nombre);
                    cmd.Parameters.AddWithValue("@FechaHora", evento.FechaHora);
                    cmd.Parameters.AddWithValue("@Ubicacion", evento.Ubicacion);
                    cmd.Parameters.AddWithValue("@TipoEvento", evento.TipoEvento);
                    cmd.Parameters.AddWithValue("@Fuente", evento.Fuente);
                    cmd.Parameters.AddWithValue("@Link", evento.Link);
                    cmd.Parameters.AddWithValue("@RecordatorioActivado", evento.RecordatorioActivado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteEvento(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Eventos WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

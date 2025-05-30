using System.Data.SQLite;
using System.IO;

namespace MistycPawCraftCore.DDBB
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath = "eventos.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
                using (var conn = new SQLiteConnection($"Data Source={DbPath};Version=3;"))
                {
                    conn.Open();
                    string createTableQuery = @"
                        CREATE TABLE Eventos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT NOT NULL,
                            FechaHora DATETIME NOT NULL,
                            Ubicacion TEXT,
                            TipoEvento INTEGER NOT NULL,
                            Fuente TEXT NOT NULL,
                            Link TEXT,
                            RecordatorioActivado BOOLEAN
                        );";
                    using (var cmd = new SQLiteCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={DbPath};Version=3;");
        }

    }
}

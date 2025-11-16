using Microsoft.Data.Sqlite;
using ProyectoSubastas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Repository
{
    public class OfertaRepository
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;

        public OfertaRepository(string databaseFilePath = null)
        {
            SQLitePCL.Batteries.Init();

            if (string.IsNullOrEmpty(databaseFilePath))
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                databaseFilePath = Path.Combine(baseDir, "bd_subastas.db");
            }

            _connectionString = new SqliteConnectionStringBuilder { DataSource = databaseFilePath }.ToString();
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            EnsureTable();
        }

        private void EnsureTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Oferta (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Monto REAL NOT NULL,
                    FechaOferta TEXT NOT NULL,
                    IdSubasta INTEGER NOT NULL,
                    IdPostor INTEGER NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        public Oferta Crear(Oferta o)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Oferta (Monto, FechaOferta, IdSubasta, IdPostor)
                VALUES (@monto, @fecha, @idSubasta, @idPostor);
            ";
            cmd.Parameters.AddWithValue("@monto", o.Monto);
            cmd.Parameters.AddWithValue("@fecha", o.FechaOferta.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@idSubasta", o.IdSubasta);
            cmd.Parameters.AddWithValue("@idPostor", o.IdPostor);

            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid();";
            var id = (long)cmd.ExecuteScalar();
            o.IdOferta = (int)id;
            return o;
        }

        public Oferta ObtenerPorId(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Monto, FechaOferta, IdSubasta, IdPostor
                FROM Oferta
                WHERE Id = @id;
            ";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Oferta
                {
                    IdOferta = reader.GetInt32(0),
                    Monto = reader.GetDecimal(1),
                    FechaOferta = DateTime.Parse(reader.GetString(2)),
                    IdSubasta = reader.GetInt32(3),
                    IdPostor = reader.GetInt32(4)
                };
            }

            return null;
        }

        public List<Oferta> ObtenerPorSubasta(int idSubasta)
        {
            var list = new List<Oferta>();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Monto, FechaOferta, IdSubasta, IdPostor
                FROM Oferta
                WHERE IdSubasta = @subasta
                ORDER BY FechaOferta DESC;
            ";
            cmd.Parameters.AddWithValue("@subasta", idSubasta);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Oferta
                {
                    IdOferta = reader.GetInt32(0),
                    Monto = reader.GetDecimal(1),
                    FechaOferta = DateTime.Parse(reader.GetString(2)),
                    IdSubasta = reader.GetInt32(3),
                    IdPostor = reader.GetInt32(4)
                });
            }

            return list;
        }

        public bool Eliminar(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Oferta WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool EliminarPorSubasta(int idSubasta)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Oferta WHERE IdSubasta = @subasta;";
            cmd.Parameters.AddWithValue("@subasta", idSubasta);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool EliminarPorPostor(int idPostor)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Oferta WHERE IdPostor = @postor;";
            cmd.Parameters.AddWithValue("@postor", idPostor);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
        
        public Oferta ObtenerUltimaOferta(int idSubasta)
        {
            using var cmd = _connection.CreateCommand();
                    cmd.CommandText = @"
                SELECT Id, Monto, FechaOferta, IdSubasta, IdPostor 
                FROM Oferta 
                WHERE IdSubasta = @idSubasta
                ORDER BY FechaOferta DESC, Id DESC
                LIMIT 1;
            ";
            cmd.Parameters.AddWithValue("@idSubasta", idSubasta);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Oferta
                {
                    IdOferta = reader.GetInt32(0),
                    Monto = reader.GetDecimal(1),
                    FechaOferta = DateTime.Parse(reader.GetString(2)),
                    IdSubasta = reader.GetInt32(3),
                    IdPostor = reader.GetInt32(4)
                };
            }

            return null;
        }
    }
}

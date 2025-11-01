using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using ProyectoSubastas.Models;

namespace ProyectoSubastas.Repository
{
    public class PostorRepository : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;

        public PostorRepository(string databaseFilePath = null)
        {
            // Si no se pasa ruta, usar la misma base que Subastador
            if (string.IsNullOrEmpty(databaseFilePath))
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                databaseFilePath = Path.Combine(baseDir, "bd_subastas.db");
            }

            _connectionString = new SqliteConnectionStringBuilder { DataSource = databaseFilePath }.ToString();
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            Console.WriteLine($"📂 Base de datos usada: {databaseFilePath}");
            EnsureTable();
        }

        private void EnsureTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Postor (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Mail TEXT NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        public Postor Create(Postor p)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Postor (Nombre, Mail) VALUES (@nombre, @mail);";
            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@mail", p.Mail);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid();";
            var id = (long)cmd.ExecuteScalar();
            p.IdPostor = (int)id;
            return p;
        }

        public Postor GetById(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Mail FROM Postor WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Postor(
                    idPostor: reader.GetInt32(0),
                    nombre: reader.GetString(1),
                    mail: reader.GetString(2)
                );
            }
            return null;
        }

        public List<Postor> GetAll()
        {
            var list = new List<Postor>();
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Mail FROM Postor ORDER BY Nombre;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Postor(
                    idPostor: reader.GetInt32(0),
                    nombre: reader.GetString(1),
                    mail: reader.GetString(2)
                ));
            }
            return list;
        }

        public bool Update(Postor p)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "UPDATE Postor SET Nombre = @nombre, Mail = @mail WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@mail", p.Mail);
            cmd.Parameters.AddWithValue("@id", p.IdPostor);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Postor WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

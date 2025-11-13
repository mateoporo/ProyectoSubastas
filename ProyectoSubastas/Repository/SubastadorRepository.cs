using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using ProyectoSubastas.Models;

namespace ProyectoSubastas.Repository
{
    public class SubastadorRepository
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;

        public SubastadorRepository(string databaseFilePath = null)
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

            Console.WriteLine($"Base de datos usada: {databaseFilePath}");
            EnsureTable();
        }

        private void EnsureTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Subastador (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Mail TEXT NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        public Subastador Crear(Subastador s)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Subastador (Nombre, Mail) VALUES (@nombre, @mail);";
            cmd.Parameters.AddWithValue("@nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@mail", s.Mail);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid();";
            var id = (long)cmd.ExecuteScalar();
            s.IdSubastador = (int)id;
            return s;
        }

        public Subastador ObtenerPorId(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Mail FROM Subastador WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Subastador(
                    idSubastador: reader.GetInt32(0),
                    nombre: reader.GetString(1),
                    mail: reader.GetString(2)
                );
            }
            return null;
        }

        public Subastador ObtenerPorMail(string mail)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Mail FROM Subastador WHERE Mail = @mail;";
            cmd.Parameters.AddWithValue("@mail", mail);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Subastador(
                    idSubastador: reader.GetInt32(0),
                    nombre: reader.GetString(1),
                    mail: reader.GetString(2)
                );
            }
            return null;
        }

        public List<Subastador> ObtenerTodos()
        {
            var list = new List<Subastador>();
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Mail FROM Subastador ORDER BY Nombre;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Subastador(
                    idSubastador: reader.GetInt32(0),
                    nombre: reader.GetString(1),
                    mail: reader.GetString(2)
                ));
            }
            return list;
        }

        public bool Modificar(Subastador s)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "UPDATE Subastador SET Nombre = @nombre, Mail = @mail WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@mail", s.Mail);
            cmd.Parameters.AddWithValue("@id", s.IdSubastador);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool Eliminar(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Subastador WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}
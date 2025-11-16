using Microsoft.Data.Sqlite;
using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Repository
{
    public class SubastaRepository
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;
        private readonly SubastadorRepository _subastadorRepository = new SubastadorRepository();
        private readonly OfertaRepository _ofertaRepository = new OfertaRepository();

        public SubastaRepository(string databaseFilePath = null)
        {
            SQLitePCL.Batteries.Init();

            if (string.IsNullOrEmpty(databaseFilePath))
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                databaseFilePath = Path.Combine(baseDir, "bd_subastas.db");
            }

            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = databaseFilePath
            }.ToString();

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            EnsureTable();
        }

        private void EnsureTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Subasta (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Articulo TEXT NOT NULL,
                    PujaInicial REAL NOT NULL,
                    PujaAumento REAL NOT NULL,
                    FechaInicio TEXT NOT NULL,
                    FechaFin TEXT NOT NULL,
                    IdSubastador INTEGER NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        public Subasta Crear(Subasta s)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Subasta 
                    (Articulo, PujaInicial, PujaAumento, FechaInicio, FechaFin, IdSubastador)
                VALUES 
                    (@articulo, @pi, @pa, @fi, @ff, @ids);
            ";

            cmd.Parameters.AddWithValue("@articulo", s.Articulo);
            cmd.Parameters.AddWithValue("@pi", s.PujaInicial);
            cmd.Parameters.AddWithValue("@pa", s.PujaAumento);
            cmd.Parameters.AddWithValue("@fi", s.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ff", s.FechaFin.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ids", s.IdSubastador);

            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid();";
            long id = (long)cmd.ExecuteScalar();
            s.IdSubasta = (int)id;

            return s;
        }

        public Subasta ObtenerPorId(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Articulo, PujaInicial, PujaAumento, FechaInicio, FechaFin, IdSubastador
                FROM Subasta
                WHERE Id = @id;
            ";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new Subasta
            {
                IdSubasta = reader.GetInt32(0),
                Articulo = reader.GetString(1),
                PujaInicial = reader.GetDecimal(2),
                PujaAumento = reader.GetDecimal(3),
                FechaInicio = DateTime.Parse(reader.GetString(4)),
                FechaFin = DateTime.Parse(reader.GetString(5)),
                IdSubastador = reader.GetInt32(6)
            };
        }

        public List<Subasta> ObtenerTodas()
        {
            var list = new List<Subasta>();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Articulo, PujaInicial, PujaAumento, FechaInicio, FechaFin, IdSubastador
                FROM Subasta
                ORDER BY FechaInicio DESC;
            ";

            using var reader = cmd.ExecuteReader();

            DateTime ahora = DateTime.Now;

            while (reader.Read())
            {
                string fechaInicioTexto = reader.GetString(4);
                DateTime fechaInicioParsed = DateTime.Parse(fechaInicioTexto);
                if (fechaInicioParsed > ahora)
                    continue;
                var subasta = new Subasta
                {
                    IdSubasta = reader.GetInt32(0),
                    Articulo = reader.GetString(1),
                    PujaInicial = reader.GetDecimal(2),
                    PujaAumento = reader.GetDecimal(3),
                    FechaInicio = fechaInicioParsed,
                    FechaFin = DateTime.Parse(reader.GetString(5)),
                    IdSubastador = reader.GetInt32(6)
                };
                subasta.Subastador = _subastadorRepository.ObtenerPorId(subasta.IdSubastador);
                subasta.Ofertas = _ofertaRepository.ObtenerPorSubasta(subasta.IdSubasta);
                subasta.Participantes = subasta.Ofertas
                    .Select(o => o.Postor)
                    .Distinct()
                    .ToList();

                list.Add(subasta);
            }

            return list;
        }

        public bool Modificar(Subasta s)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Subasta SET 
                    Articulo = @articulo,
                    PujaInicial = @pi,
                    PujaAumento = @pa,
                    FechaInicio = @fi,
                    FechaFin = @ff,
                    IdSubastador = @ids
                WHERE Id = @id;
            ";

            cmd.Parameters.AddWithValue("@articulo", s.Articulo);
            cmd.Parameters.AddWithValue("@pi", s.PujaInicial);
            cmd.Parameters.AddWithValue("@pa", s.PujaAumento);
            cmd.Parameters.AddWithValue("@fi", s.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ff", s.FechaFin.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ids", s.IdSubastador);
            cmd.Parameters.AddWithValue("@id", s.IdSubasta);

            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool Eliminar(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Subasta WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}

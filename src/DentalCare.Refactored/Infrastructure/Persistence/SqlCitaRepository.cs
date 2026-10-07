using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Repositories;
using System.Data.SqlClient;

namespace DentalCare.Refactored.Infrastructure.Persistence
{
    public class SqlCitaRepository : ICitaRepository
    {
        private readonly string _connectionString;

        public SqlCitaRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Guardar(Cita cita)
        {
            // Ejecución desacoplada implementando ICitaRepository
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            const string sql = "INSERT INTO Citas (Id, PacienteId, OdontologoId, Fecha, Copago, Estado) " +
                               "VALUES (@id, @p, @o, @f, @c, @e)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", cita.Id);
            cmd.Parameters.AddWithValue("@p", cita.Paciente.Id);
            cmd.Parameters.AddWithValue("@o", cita.Odontologo.Id);
            cmd.Parameters.AddWithValue("@f", cita.FechaHora);
            cmd.Parameters.AddWithValue("@c", cita.CopagoCalculado);
            cmd.Parameters.AddWithValue("@e", cita.Estado.ToString());
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Cita cita)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            const string sql = "UPDATE Citas SET Estado = @e, Penalizacion = @pen WHERE Id = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", cita.Estado.ToString());
            cmd.Parameters.AddWithValue("@pen", cita.PenalizacionCancelacion);
            cmd.Parameters.AddWithValue("@id", cita.Id);
            cmd.ExecuteNonQuery();
        }

        public Cita? ObtenerPorId(string id)
        {
            // Abstracción de lectura
            return null; // Demostración de contrato
        }
    }
}
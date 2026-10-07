using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Repositories;

namespace DentalCare.Refactored.Infrastructure.Persistence
{
    public class InMemoryCitaRepository : ICitaRepository
    {
        private readonly Dictionary<string, Cita> _citas = new();

        public void Guardar(Cita cita)
        {
            _citas[cita.Id] = cita;
            Console.WriteLine($"[DB PERSISTENCIA (IN-MEMORY)] Cita #{cita.Id} guardada con éxito en almacén de datos.");
        }

        public void Actualizar(Cita cita)
        {
            if (_citas.ContainsKey(cita.Id))
            {
                _citas[cita.Id] = cita;
                Console.WriteLine($"[DB PERSISTENCIA (IN-MEMORY)] Cita #{cita.Id} actualizada. Estado: {cita.Estado}, Penalización: ${cita.PenalizacionCancelacion:N2}");
            }
        }

        public Cita? ObtenerPorId(string id)
        {
            return _citas.TryGetValue(id, out var cita) ? cita : null;
        }
    }
}
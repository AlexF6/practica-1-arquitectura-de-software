using DentalCare.Refactored.Domain.Entities;

namespace DentalCare.Refactored.Domain.Repositories
{
    public interface ICitaRepository
    {
        void Guardar(Cita cita);
        void Actualizar(Cita cita);
        Cita? ObtenerPorId(string id);
    }
}
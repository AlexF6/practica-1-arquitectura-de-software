using DentalCare.Refactored.Domain.Entities;

namespace DentalCare.Refactored.Domain.Strategies
{
    public interface IPoliticaCancelacionStrategy
    {
        decimal CalcularPenalizacion(Cita cita, DateTime fechaHoraCancelacion);
    }
}
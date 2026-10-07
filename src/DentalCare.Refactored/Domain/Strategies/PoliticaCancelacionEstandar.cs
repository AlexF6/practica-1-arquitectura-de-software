using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Enums;

namespace DentalCare.Refactored.Domain.Strategies
{
    public class PoliticaCancelacionEstandar : IPoliticaCancelacionStrategy
    {
        private const decimal MultaExtemporaneaBase = 50.0m;
        private const decimal RecargoCirugia = 40.0m;

        public decimal CalcularPenalizacion(Cita cita, DateTime fechaHoraCancelacion)
        {
            TimeSpan margenAntelacion = cita.FechaHora - fechaHoraCancelacion;
            
            if (margenAntelacion.TotalHours >= 24)
            {
                return 0.0m;
            }

            decimal penalizacion = MultaExtemporaneaBase;

            if (cita.Odontologo.Especialidad == Especialidad.Cirugia)
            {
                penalizacion += RecargoCirugia;
            }

            return penalizacion;
        }
    }
}
using DentalCare.Refactored.Domain.Enums;

namespace DentalCare.Refactored.Domain.Entities
{
    public class Cita
    {
        public string Id { get; }
        public Paciente Paciente { get; }
        public Odontologo Odontologo { get; }
        public DateTime FechaHora { get; }
        public decimal CopagoCalculado { get; }
        public EstadoCita Estado { get; private set; }
        public decimal PenalizacionCancelacion { get; private set; }

        public Cita(string id, Paciente paciente, Odontologo odontologo, DateTime fechaHora, decimal copagoCalculado)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            Odontologo = odontologo ?? throw new ArgumentNullException(nameof(odontologo));
            FechaHora = fechaHora;
            CopagoCalculado = copagoCalculado;
            Estado = EstadoCita.Programada;
            PenalizacionCancelacion = 0m;
        }

        public void Cancelar(decimal penalizacion)
        {
            if (Estado == EstadoCita.Cancelada)
                throw new InvalidOperationException("La cita ya se encuentra en estado cancelada.");

            Estado = EstadoCita.Cancelada;
            PenalizacionCancelacion = penalizacion;
        }
    }
}
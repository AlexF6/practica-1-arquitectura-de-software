using DentalCare.Refactored.Application.Interfaces;
using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Repositories;
using DentalCare.Refactored.Domain.Strategies;

namespace DentalCare.Refactored.Application.Services
{
    public class CitaAppService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IServicioNotificacionCitas _notificador;
        private readonly ICalculadoraCopagoStrategy _calculadoraCopago;
        private readonly IPoliticaCancelacionStrategy _politicaCancelacion;

        // Métricas de control de servicio
        public decimal TotalRecaudadoMes { get; private set; }
        public int TotalCitasCanceladas { get; private set; }

        public CitaAppService(
            ICitaRepository citaRepository,
            IServicioNotificacionCitas notificador,
            ICalculadoraCopagoStrategy calculadoraCopago,
            IPoliticaCancelacionStrategy politicaCancelacion)
        {
            _citaRepository = citaRepository ?? throw new ArgumentNullException(nameof(citaRepository));
            _notificador = notificador ?? throw new ArgumentNullException(nameof(notificador));
            _calculadoraCopago = calculadoraCopago ?? throw new ArgumentNullException(nameof(calculadoraCopago));
            _politicaCancelacion = politicaCancelacion ?? throw new ArgumentNullException(nameof(politicaCancelacion));
        }

        public Cita AgendarCita(Paciente paciente, Odontologo odontologo, DateTime fechaHora, bool requiereRadiografia)
        {
            if (paciente == null) throw new ArgumentNullException(nameof(paciente));
            if (odontologo == null) throw new ArgumentNullException(nameof(odontologo));

            if (!odontologo.EstaDisponible)
            {
                throw new InvalidOperationException("El odontólogo no tiene disponibilidad en el horario seleccionado.");
            }

            decimal copago = _calculadoraCopago.CalcularCopago(paciente, odontologo, requiereRadiografia);

            string idUnico = Guid.NewGuid().ToString()[..8];
            var nuevaCita = new Cita(idUnico, paciente, odontologo, fechaHora, copago);

            _citaRepository.Guardar(nuevaCita);
            _notificador.NotificarConfirmacion(nuevaCita);

            TotalRecaudadoMes += copago;
            return nuevaCita;
        }

        public decimal CancelarCita(Cita cita, DateTime fechaHoraCancelacion)
        {
            if (cita == null) throw new ArgumentNullException(nameof(cita));

            decimal penalizacion = _politicaCancelacion.CalcularPenalizacion(cita, fechaHoraCancelacion);
            cita.Cancelar(penalizacion);

            _citaRepository.Actualizar(cita);
            _notificador.NotificarCancelacion(cita);

            TotalCitasCanceladas++;
            return penalizacion;
        }
    }
}
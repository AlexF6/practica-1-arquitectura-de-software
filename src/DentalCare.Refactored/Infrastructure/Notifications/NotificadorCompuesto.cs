using DentalCare.Refactored.Application.Interfaces;
using DentalCare.Refactored.Domain.Entities;

namespace DentalCare.Refactored.Infrastructure.Notifications
{
    public class NotificadorCompuesto : IServicioNotificacionCitas
    {
        private readonly INotificadorEmail _emailSender;
        private readonly INotificadorSms _smsSender;

        public NotificadorCompuesto(INotificadorEmail emailSender, INotificadorSms smsSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public void NotificarConfirmacion(Cita cita)
        {
            string cuerpoEmail = $"Estimado(a) {cita.Paciente.NombreCompleto}, su cita quedó agendada para el " +
                                 $"{cita.FechaHora:yyyy-MM-dd HH:mm}. Copago: ${cita.CopagoCalculado:N2}.";
            _emailSender.EnviarEmail(cita.Paciente.Correo, "Confirmación de Cita Odontológica", cuerpoEmail);

            string textoSms = $"DentaCare: Cita confirmada el {cita.FechaHora:dd/MM HH:mm}. Copago: ${cita.CopagoCalculado:N2}";
            _smsSender.EnviarSms(cita.Paciente.Celular, textoSms);
        }

        public void NotificarCancelacion(Cita cita)
        {
            string cuerpoEmail = $"Su cita #{cita.Id} ha sido CANCELADA. Penalización aplicada: ${cita.PenalizacionCancelacion:N2}.";
            _emailSender.EnviarEmail(cita.Paciente.Correo, "Cancelación de Cita", cuerpoEmail);

            string textoSms = $"DentaCare: Cita #{cita.Id} cancelada. Penalización: ${cita.PenalizacionCancelacion:N2}";
            _smsSender.EnviarSms(cita.Paciente.Celular, textoSms);
        }
    }

    public class CanalEmailSmtp : INotificadorEmail
    {
        public void EnviarEmail(string destinatario, string asunto, string cuerpo)
        {
            // Simulación desacoplada o uso de SmtpClient aislado
            Console.WriteLine($"[EMAIL DISPATCH] Para: {destinatario} | Asunto: {asunto}");
        }
    }

    public class CanalSmsTwilio : INotificadorSms
    {
        public void EnviarSms(string numeroCelular, string mensaje)
        {
            // Simulación desacoplada con cliente HTTP REST de Twilio
            Console.WriteLine($"[SMS DISPATCH] Destino: {numeroCelular} | Mensaje: {mensaje}");
        }
    }
}
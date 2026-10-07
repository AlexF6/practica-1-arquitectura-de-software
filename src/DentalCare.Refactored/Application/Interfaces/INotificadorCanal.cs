using DentalCare.Refactored.Domain.Entities;

namespace DentalCare.Refactored.Application.Interfaces
{
    public interface INotificadorEmail
    {
        void EnviarEmail(string destinatario, string asunto, string cuerpo);
    }

    public interface INotificadorSms
    {
        void EnviarSms(string numeroCelular, string mensaje);
    }

    public interface IServicioNotificacionCitas
    {
        void NotificarConfirmacion(Cita cita);
        void NotificarCancelacion(Cita cita);
    }
}
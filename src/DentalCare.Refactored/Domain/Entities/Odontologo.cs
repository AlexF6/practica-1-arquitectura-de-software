using DentalCare.Refactored.Domain.Enums;

namespace DentalCare.Refactored.Domain.Entities
{
    public class Odontologo
    {
        public string Id { get; }
        public string Nombre { get; }
        public Especialidad Especialidad { get; }
        public bool EstaDisponible { get; private set; }

        public Odontologo(string id, string nombre, Especialidad especialidad, bool estaDisponible)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Especialidad = especialidad;
            EstaDisponible = estaDisponible;
        }

        public void ModificarDisponibilidad(bool disponible)
        {
            EstaDisponible = disponible;
        }
    }
}
using DentalCare.Refactored.Domain.Enums;

namespace DentalCare.Refactored.Domain.Entities
{
    public class Paciente
    {
        public string Id { get; }
        public string NombreCompleto { get; }
        public string Correo { get; }
        public string Celular { get; }
        public TipoConvenio Convenio { get; }
        public bool EsPrimeraVez { get; }

        public Paciente(string id, string nombreCompleto, string correo, string celular, TipoConvenio convenio, bool esPrimeraVez)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            NombreCompleto = nombreCompleto ?? throw new ArgumentNullException(nameof(nombreCompleto));
            Correo = correo ?? throw new ArgumentNullException(nameof(correo));
            Celular = celular ?? throw new ArgumentNullException(nameof(celular));
            Convenio = convenio;
            EsPrimeraVez = esPrimeraVez;
        }
    }
}
namespace DentalCare.Refactored.Domain.Enums
{
    public enum Especialidad
    {
        General = 0,
        Ortodoncia = 1,
        Endodoncia = 2,
        Cirugia = 3,
        Odontopediatria = 4
    }

    public enum TipoConvenio
    {
        Particular = 1,
        EPS = 2,
        Prepagada = 3
    }

    public enum EstadoCita
    {
        Programada,
        Cancelada,
        Completada
    }
}
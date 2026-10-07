using DentalCare.Refactored.Domain.Entities;

namespace DentalCare.Refactored.Domain.Strategies
{
    public interface ICalculadoraCopagoStrategy
    {
        decimal CalcularCopago(Paciente paciente, Odontologo odontologo, bool requiereRadiografia);
    }
}
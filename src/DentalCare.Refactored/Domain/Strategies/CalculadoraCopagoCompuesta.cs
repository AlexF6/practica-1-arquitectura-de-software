using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Enums;

namespace DentalCare.Refactored.Domain.Strategies
{
    public class CalculadoraCopagoCompuesta : ICalculadoraCopagoStrategy
    {
        private const decimal CostoBaseConsulta = 100.0m;
        private const decimal CargoPrimeraVez = 20.0m;
        private const decimal CargoRadiografia = 35.0m;

        private readonly IDictionary<Especialidad, decimal> _multiplicadoresEspecialidad;
        private readonly IDictionary<TipoConvenio, decimal> _coberturasConvenio;

        public CalculadoraCopagoCompuesta()
        {
            _multiplicadoresEspecialidad = new Dictionary<Especialidad, decimal>
            {
                { Especialidad.General, 1.0m },
                { Especialidad.Ortodoncia, 1.2m },
                { Especialidad.Endodoncia, 1.8m },
                { Especialidad.Cirugia, 2.5m },
                { Especialidad.Odontopediatria, 1.1m }
            };

            _coberturasConvenio = new Dictionary<TipoConvenio, decimal>
            {
                { TipoConvenio.Particular, 1.0m },  // 100% copago
                { TipoConvenio.EPS, 0.30m },        // 70% cobertura -> Paga 30%
                { TipoConvenio.Prepagada, 0.10m }   // 90% cobertura -> Paga 10%
            };
        }

        public decimal CalcularCopago(Paciente paciente, Odontologo odontologo, bool requiereRadiografia)
        {
            decimal factorEspecialidad = _multiplicadoresEspecialidad.TryGetValue(odontologo.Especialidad, out var factor) 
                ? factor 
                : 1.0m;

            decimal factorConvenio = _coberturasConvenio.TryGetValue(paciente.Convenio, out var cobertura) 
                ? cobertura 
                : 1.0m;

            decimal subtotal = CostoBaseConsulta * factorEspecialidad;
            decimal copago = subtotal * factorConvenio;

            if (paciente.EsPrimeraVez)
            {
                copago += CargoPrimeraVez;
            }

            if (requiereRadiografia)
            {
                copago += CargoRadiografia;
            }

            return Math.Round(copago, 2);
        }
    }
}
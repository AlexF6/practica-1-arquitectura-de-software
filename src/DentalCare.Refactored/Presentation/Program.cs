using DentalCare.Refactored.Application.Interfaces;
using DentalCare.Refactored.Application.Services;
using DentalCare.Refactored.Domain.Entities;
using DentalCare.Refactored.Domain.Enums;
using DentalCare.Refactored.Domain.Repositories;
using DentalCare.Refactored.Domain.Strategies;
using DentalCare.Refactored.Infrastructure.Notifications;
using DentalCare.Refactored.Infrastructure.Persistence;

Console.WriteLine("====================================================================");
Console.WriteLine(" DENTACARE SYSTEM - ARQUITECTURA REFACTORIZADA (SOLID COMPLIANT)     ");
Console.WriteLine("====================================================================\n");

try
{
    // 1. Configuración del Contenedor / Inversión de Control (DIP)
    // Para producción: ICitaRepository citaRepository = new SqlCitaRepository("Server=sql.dentacare.com;...");
    // Para ejecución local / pruebas sin dependencia externa:
    ICitaRepository citaRepository = new InMemoryCitaRepository();

    INotificadorEmail canalEmail = new CanalEmailSmtp();
    INotificadorSms canalSms = new CanalSmsTwilio();
    IServicioNotificacionCitas notificador = new NotificadorCompuesto(canalEmail, canalSms);

    // Estrategias intercambiables de negocio (OCP / Strategy Pattern)
    ICalculadoraCopagoStrategy estrategiaCopago = new CalculadoraCopagoCompuesta();
    IPoliticaCancelacionStrategy estrategiaCancelacion = new PoliticaCancelacionEstandar();

    // Inyección en la capa de aplicación
    var servicioCitas = new CitaAppService(
        citaRepository, 
        notificador, 
        estrategiaCopago, 
        estrategiaCancelacion
    );

    // 2. Ejecución de Modelos de Dominio
    var paciente1 = new Paciente(
        id: "PAC-101",
        nombreCompleto: "Ana María Gómez",
        correo: "ana.gomez@email.com",
        celular: "3001234567",
        convenio: TipoConvenio.EPS,
        esPrimeraVez: true
    );

    var odontologo1 = new Odontologo(
        id: "ODO-202",
        nombre: "Dr. Roberto Martínez",
        especialidad: Especialidad.Cirugia,
        estaDisponible: true
    );

    Console.WriteLine("---> [FLUJO 1]: AGENDAMIENTO DE CITA REFACTORIZADO");
    DateTime fechaCita = DateTime.Now.AddHours(12);
    Cita citaAgendada = servicioCitas.AgendarCita(
        paciente1, 
        odontologo1, 
        fechaCita, 
        requiereRadiografia: true
    );

    Console.WriteLine($"\n[INFO] Cita generada exitosamente:");
    Console.WriteLine($"       ID Cita: {citaAgendada.Id}");
    Console.WriteLine($"       Copago Calculado: ${citaAgendada.CopagoCalculado:N2}");
    Console.WriteLine($"       Estado: {citaAgendada.Estado}\n");

    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine("---> [FLUJO 2]: CANCELACIÓN DE CITA EXTENSIBLE");
    DateTime fechaCancelacion = DateTime.Now;
    decimal penalizacion = servicioCitas.CancelarCita(citaAgendada, fechaCancelacion);

    Console.WriteLine($"\n[INFO] Cita ID #{citaAgendada.Id} procesada:");
    Console.WriteLine($"       Nuevo Estado: {citaAgendada.Estado}");
    Console.WriteLine($"       Penalización Aplicada: ${penalizacion:N2}\n");

    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine("---> [FLUJO 3]: TOTALES ACUMULADOS EN SERVICIO DE APLICACIÓN");
    Console.WriteLine($" Total Recaudado en Copagos: ${servicioCitas.TotalRecaudadoMes:N2}");
    Console.WriteLine($" Total Citas Canceladas: {servicioCitas.TotalCitasCanceladas}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[CRITICAL ERROR]: {ex.Message}");
}

Console.WriteLine("\n====================================================================");
Console.WriteLine("Ejecución finalizada con éxito.");
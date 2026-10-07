# ADR-001: Desacoplamiento de la Persistencia Relacional y los Canales de Notificación mediante Inversión de Dependencias (DIP) e Interfaces Segregadas (ISP)

## Estatus
Aprobado (Approved)

## Contexto y Definición del Problema
En el sistema legado de `DentaCare`, la clase `GestorCitasOdontologicas` acopla directamente:
1. Acceso a datos relacionales utilizando `SqlServerEjecutor`, el cual contiene cadenas de conexión en texto plano (vulnerabilidad de seguridad) y comandos ADO.NET SQL embebidos.
2. Despacho de notificaciones mediante `NotificacionServicio`, que mezcla de forma rígida el cliente `SmtpClient` y un token simulado de la API de Twilio.

Esta configuración viola el principio de Inversión de Dependencias (DIP) y de Responsabilidad Única (SRP), impidiendo el desarrollo de pruebas unitarias automatizadas (unit testing) sin infraestructura activa y forzando la recompilación del núcleo transaccional ante cualquier ajuste en el esquema SQL o en el proveedor de comunicaciones.

## Criterios de Decisión
* Eliminar dependencias directas entre las reglas de negocio y los controladores de infraestructura.
* Permitir la segregación de canales de comunicación (un paciente puede recibir solo SMS o solo Correo).
* Habilitar la prueba de orquestación de citas en memoria mediante dobles de prueba (Mocks/Stubs).
* Adherirse a la norma ISO/IEC/IEEE 42010:2022 garantizando la trazabilidad entre el stakeholder "Equipo de Operaciones/Desarrollo" y los puntos de vista de modularidad y mantenibilidad.

## Alternativas Consideradas
* **Alternativa 1: Mantener la clase concreta `SqlServerEjecutor` parametrizando la cadena de conexión por configuración.**
  * *Rechazada:* No resuelve el acoplamiento a SQL Server ni permite la prueba aislada del caso de uso.
* **Alternativa 2: Implementar un bus de eventos asíncrono (Mediator Pattern / RabbitMQ).**
  * *Rechazada:* Agrega complejidad accidental desproporcionada para el alcance actual del módulo en memoria.
* **Alternativa 3: Aplicar Inversión de Dependencias (DIP) mediante el Patrón Repositorio (`ICitaRepository`) y Adaptadores Segregados de Notificación (`INotificadorEmail`, `INotificadorSms`).**
  * *Seleccionada.*

## Decisión Arquitectónica
Se decide:
1. Extraer el contrato `ICitaRepository` en el Dominio para abstraer el ciclo de vida de persistencia de la entidad `Cita`.
2. Segregar los contratos de notificación en `INotificadorEmail` e `INotificadorSms` (ISP) y crear un orquestador compuesto `IServicioNotificacionCitas` implementado en la capa de Infraestructura (`NotificadorCompuesto`).
3. Inyectar estas dependencias a través del constructor de `CitaAppService` en la capa de Aplicación.

## Consecuencias y Trade-offs
### Impactos Positivos (+):
* **Testabilidad Inmediata:** La lógica de agendamiento y cancelación puede ser validada al 100% mediante mocks en memoria.
* **Sustituibilidad:** Capacidad de cambiar el proveedor de base de datos (e.g., PostgreSQL o CosmosDB) sin modificar una sola línea del dominio.
* **Seguridad:** Aislamiento de secretos y cadenas de conexión fuera de las clases de lógica transaccional.

### Impactos Negativos / Mitigaciones (-):
* **Sobrecarga de Abstracción:** Incremento en el número total de archivos e interfaces en la solución (mitigado por una estructura de carpetas estandarizada y unificada).

## Vistas Arquitectónicas Afectadas (ISO/IEC/IEEE 42010:2022)
* **Vista Estructural / Módulo:** Desacoplamiento de `DentaCare.Refactored.Application` hacia `DentaCare.Refactored.Infrastructure`.
* **Vista de Información:** Aislamiento del ciclo de vida de los datos de la entidad `Cita` detrás de la abstracción del Repositorio.
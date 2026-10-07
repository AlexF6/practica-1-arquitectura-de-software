# ADR-002: Reestructuración de Reglas Tarifarias y Políticas de Cancelación Dinámicas mediante el Patrón Strategy y Polimorfismo (OCP)

## Estatus
Aprobado (Approved)

## Contexto y Definición del Problema
El método legado `GestorCitasOdontologicas.AgendarCita` calcula el copago del paciente a través de un switch/if anidado evaluando constantes numéricas enteras (`tipoEspecialidad = 1, 2, 3, 4` y `TipoConvenio = 1, 2, 3`). De manera análoga, `CancelarCita` evalúa condicionalmente si el odontólogo asignado posee la especialidad `3` (Cirugía) para sumar una penalización de $40 USD sobre una multa base de $50 USD cuando la cancelación ocurre con menos de 24 horas de antelación.

Este enfoque presenta fallas estructurales críticas:
* **Fragilidad ante la Extensión (Violación OCP):** La creación de una nueva especialidad (ej. Implantología) o convenio institucional requiere abrir y alterar los métodos centrales de la clase.
* **Ofuscación Primitiva (Primitive Obsession):** El uso de enteros sin tipo fuerte para especialidades y convenios incrementa la propensión a errores en tiempo de ejecución.
* **Complejidad Ciclomática Elevada:** Ramificaciones anidadas que dificultan el mantenimiento y la cobertura de pruebas.

## Criterios de Decisión
* Eliminar el acoplamiento a números mágicos mediante enums fuertemente tipados en el modelo de dominio.
* Cumplir con el Principio de Abierto/Cerrado (OCP): habilitar la adición de nuevas reglas tarifarias sin alterar el código existente.
* Centralizar el cálculo de la penalización en un componente dedicado con responsabilidad única (SRP).

## Alternativas Consideradas
* **Alternativa 1: Herencia de clases (`CitaCirugia`, `CitaOrtodoncia`, etc.).**
  * *Rechazada:* Genera una explosión combinatoria inmanejable de subclases al intentar cruzarlas con los diferentes convenios (`CitaCirugiaEPS`, `CitaCirugiaPrepagada`, etc.).
* **Alternativa 2: Reglas parametrizadas en tablas de base de datos cargadas dinámicamente.**
  * *Rechazada:* Introduce dependencias de I/O en validaciones puras de negocio que deben resolverse sincrónicamente en memoria.
* **Alternativa 3: Implementación del Patrón de Diseño Strategy (`ICalculadoraCopagoStrategy` e `IPoliticaCancelacionStrategy`).**
  * *Seleccionada.*

## Decisión Arquitectónica
Se decide:
1. Reemplazar los primitivos enteros por enums fuertemente tipados: `Especialidad` y `TipoConvenio`.
2. Encapsular el algoritmo de fijación de precios en el contrato `ICalculadoraCopagoStrategy`, proveyendo la implementación concreta `CalculadoraCopagoCompuesta`.
3. Encapsular la lógica de penalización por cancelación en el contrato `IPoliticaCancelacionStrategy`, implementando la regla en `PoliticaCancelacionEstandar`.
4. Transferir la responsabilidad de cambiar de estado a la entidad `Cita` mediante el método `Cancelar(penalizacion)`.

## Consecuencias y Trade-offs
### Impactos Positivos (+):
* **Extensibilidad OCP:** Si la clínica suscribe un nuevo convenio o altera las tarifas para días festivos, se crea una nueva clase que implemente `ICalculadoraCopagoStrategy` y se inyecta en el contenedor sin tocar `CitaAppService`.
* **Seguridad de Tipos:** El compilador de C# valida en tiempo de diseño que no se suministren valores arbitrarios fuera de los dominios definidos en los enums.
* **Claridad Semántica:** Eliminación total de variables mágicas y anidamientos `if/else`.

### Impactos Negativos / Mitigaciones (-):
* **Indirección Adicional:** Los desarrolladores deben navegar a través de contratos de estrategia en lugar de ver la fórmula lineal en el método de agendamiento. Esto queda documentado y compensado por la cohesión alcanzada.

## Vistas Arquitectónicas Afectadas (ISO/IEC/IEEE 42010:2022)
* **Vista Lógica:** Modificación del diagrama de clases; paso de una God Class a un ecosistema de colaboración con componentes Strategy.
* **Punto de Vista de Calidad (Mantenibilidad):** Reducción de la complejidad ciclomática de $V(G) = 11$ a $V(G) \le 2$ en las funciones orquestadoras.
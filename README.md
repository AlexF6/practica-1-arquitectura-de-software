# Práctica 1: Diseño Sostenible y Principios SOLID
**Asignatura:** Arquitectura de Software I  
**Estándar de Documentación:** ISO/IEC/IEEE 42010:2022  
**Tecnología:** .NET 8.0 / C#

---

## Descripción del Proyecto
Este repositorio contiene el diagnóstico cuantitativo, la refactorización arquitectónica orientada a objetos y la documentación formal de decisiones para el sistema de administración de citas odontológicas **DentaCare**.

Se transformó un módulo legado monolítico basado en una *God Class* (`DentaCare.Legacy`) hacia una arquitectura desacoplada, testeable y extensible (`DentalCare.Refactored`), fundamentada en los 5 principios SOLID, Inversión de Dependencias (DIP) y patrones de diseño (Strategy, Repository, Composite/Adapter).

---

## Estructura del Repositorio

```text
.
├── src/
│   ├── DentaCare.Legacy/            # Código base original (Línea base monolítica)
│   └── DentalCare.Refactored/       # Solución refactorizada desacoplada
│       ├── Domain/                  # Entidades, Enums, Interfaces de Repositorios y Estrategias
│       ├── Application/             # Casos de uso (CitaAppService) e Interfaces de Notificación
│       ├── Infrastructure/          # Persistencia (SQL / In-Memory) y Canales (Email / SMS)
│       └── Presentation/            # Inversión de Control y Orquestación (Program.cs)
├── docs/
│   ├── adr/                         # Registros de Decisiones de Arquitectura (ISO 42010)
│   │   ├── ADR-001-inversion-dependencias-persistencia-notificaciones.md
│   │   └── ADR-002-ajuste-politicas-cancelacion-tarifas.md
│   └── Informe_Metricas.pdf         # Informe formal de métricas de modularidad
├── DentaCare.slnx                   # Solución .NET
└── README.md
```

---

## Resumen Comparativo de Métricas (Baseline vs. Refactorizado)

| Métrica Arquitectónica | Código Legado (`GestorCitasOdontologicas`) | Código Refactorizado (`CitaAppService` + Dominio) | Impacto / Beneficio |
| :--- | :---: | :---: | :--- |
| **LCOM96b** *(Falta de Cohesión)* | `0.50` (Henderson-Sellers: `0.67`) | `0.33` (Estrategias: `0.00`) | **Alta Cohesión:** Se separó la persistencia y la mensajería del cálculo transaccional. |
| **Acoplamiento Eferente Concreto ($C_e$)** | `5` tipos concretos | `0` dependencias directas | **Cumplimiento DIP:** La capa de aplicación solo depende de abstracciones e interfaces. |
| **Abstracción ($A$)** | `0.00` | `0.50` | Transición a un diseño gobernado por contratos e interfaces polimórficas. |
| **Zona de Dolor ($A=0, I=0$)** | **Riesgo Crítico** | **Completamente Mitigado** | El módulo soporta clientes entrantes sin rigidez arquitectónica. |
| **Complejidad Ciclomática $V(G)$** | $V(G) = 11$ (Branches anidados) | $V(G) \le 2$ (Lookup dictionaries) | **Extensibilidad OCP:** Se eliminó la obsesión primitiva y los bloques condicionales extensos. |
| **Testabilidad Unitaria** | `0%` (Requiere BD y SMTP activos) | `100%` Aislable | Se permite la ejecución y validación inmediata con dobles de prueba en memoria. |

---

## Decisiones de Arquitectura (ADRs - ISO/IEC/IEEE 42010)
Las justificaciones formales y sus trade-offs se encuentran documentadas en la carpeta `docs/adr/`:
* [ADR-001: Desacoplamiento de Persistencia y Notificaciones (DIP / ISP)](docs/adr/ADR-001-inversion-dependencias-persistencia-notificaciones.md)
* [ADR-002: Reestructuración de Políticas Tarifarias y Cancelación con Patrón Strategy (OCP)](docs/adr/ADR-002-ajuste-politicas-cancelacion-tarifas.md)

---

## Instrucciones de Compilación y Ejecución

### Prerrequisitos
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.

### 1. Clonar el repositorio
```bash
git clone https://github.com/AlexF6/practica-1-arquitectura-de-software.git
cd practica-1-arquitectura-de-software.git
```

### 2. Compilar la solución
```bash
dotnet build
```

### 3. Ejecutar la versión refactorizada
```bash
dotnet run --project src/DentalCare.Refactored/DentalCare.Refactored.csproj
```

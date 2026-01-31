# BankingSolution — Instrucciones para ejecutar el proyecto y las pruebas

## Descripción
Proyecto de prueba técnica con arquitectura por capas (Clean Architecture + CQRS).  
Incluye implementaciones de handlers (Commands / Queries) y un conjunto de **pruebas unitarias** para validar la creación de cuentas, clientes y transacciones.

Los tests usan **EF Core InMemory** y están diseñados para ser rápidos, repetibles y sin tocar bases de datos reales.

---

## Requisitos previos
- [.NET 8 SDK](https://dotnet.microsoft.com/) (o la versión usada en el proyecto) instalado.
- Visual Studio 2022 o VS Code (opcional, para ejecutar/depurar).
- Git (para clonar y push).

---

## Restaurar, compilar y ejecutar (línea de comando)

Desde la raíz del repositorio:

```bash
# Restaurar paquetes
dotnet restore

# Compilar la solución
dotnet build

# Ajusta la ruta al proyecto API si tiene otro nombre en Default project:
dotnet run --project ./src/BankinSolution.Api/BankinSolution.Api.csproj
```
---

## Ejecutar todas las pruebas unitarias

Desde la raíz de la solución:

```bash
dotnet test
```

O ejecutar el proyecto de tests específico:

```bash
dotnet test ./test/BankingSolution.Application.UnitTest/BankingSolution.Application.UnitTest.csproj
```

### Ejecutar un test individual (ejemplo)
Puedes filtrar por el nombre completo de la prueba (FullyQualifiedName):

```bash
dotnet test --filter "FullyQualifiedName=BankingSolution.Application.UnitTest.Features.Accounts.Commands.CreateAccountCommandHandlerXUnitTests.Handle_Should_Create_Bank_Account"
```

O por parte del nombre de la prueba (con `~` para coincidencia parcial):

```bash
dotnet test --filter "DisplayName~Create_Bank_Account"
```




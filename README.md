# Como inicializar un proyecto de Nunit

## Paso 1: Crear la carpeta raiz

`mkdir PRUEBAS_UNITARIAS_PROYECTO`
`cd PRUEBAS_UNITARIAS_PROYECTO`

## Paso 2: Crear proyecto principal

`dotnet new classlib -n Proyecto`

## Paso 3: Crear el proyecto de pruebas

`dotnet new nunit -n Proyecto.Tests`

## Paso 4: Agregar referencias del proyecto principal al de pruebas

`dotnet add Proyecto.Tests reference Proyecto`

## Paso 5: Crear el archivo de solución y agregar los proyectos

`dotnet new sln -n pruebas_unitarias`
`dotnet sln add Proyecto/Proyecto.csproj`
`dotnet sln add Proyecto.Tests/Proyecto.Tests.csproj`

## Paso 6: Abrir en VSCode

Abrirlo `code .`

## Paso 7: Ejecutar pruebas/compile

Para test:
`dotnet test`

Para verificar que compile:
`dotnet build`

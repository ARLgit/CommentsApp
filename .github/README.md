# CommentsApp

## Tabla de Contenidos.

- [Descripcion](#Descripcion)
- [TechStack](#TechStack)
- [Pre-Requisitos](#Pre-Requisitos)
- [Instalacion](#Instalacion)

## Descripcion.
CommentsApp es un proyecto creado durante Enero de 2024 con el proposito de actualizar mis conocimientos de .NET core, APIs, SQL server, Angular y manejo de JWT tokens. 

El back end del proyecto cuenta con una API creada con .NET core 8 y una base de datos de SQL Server Express creada a traves de Migraciones de Entity Framework, diseñada con la metodologia Code-First.

El front end es una Web App creada usando Angular 17 la cual consulta la API utilizando pedidos http.

CommentsApp consiste de un foro en el cual los usuarios pueden buscar y/o crear hilos de discusion y responder a los mismos. Los usuarios pueden crear, editar y borrar cuentas, hilos y comentarios.

## TechStack.
### Back End.
- [.NET Core 8](https://learn.microsoft.com/es-es/dotnet/core/whats-new/dotnet-8/overview).

    - [Entity Framework Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0).
        - [Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity).
        - [SQL Server](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/8.0.0).
        - [Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/8.0.0).

    - [Microsoft AspNetCore Authentication: JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/8.0.0).
    - [AutoMapper](https://automapper.org/).
    - [Newtonsoft.Json](https://www.newtonsoft.com/json).

- [SQL Server Express](https://www.microsoft.com/es-ar/sql-server/sql-server-downloads).

### Front End.
- [Angular 17](https://angular.io/).
    
    - [sweetalert2](https://sweetalert2.github.io/) 11.10.3
    - [ngx-cookie-service](https://github.com/stevermeister/ngx-cookie-service#readme) 17.0.1
    - [jwt-decode](https://github.com/auth0/jwt-decode#readme) 4.0.0
    - [material](https://material.angular.io/) 17.1.0

## Pre-Requisitos.
Asegurese de contar con las siguientes herramientas antes de clonar este proyecto:

- [Git](https://git-scm.com/downloads)

### Back End.
- [Visual Studio](https://visualstudio.microsoft.com/es/downloads/)
    - Durante la instalacion seleccione "ASP.NET and web developement."
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [SQL Server Express](https://www.microsoft.com/es-ar/sql-server/sql-server-downloads)

### Front End.
- IDE de preferencia, ejemplo: [Visual Studio Code](https://code.visualstudio.com/download)
- [NodeJS](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/cli)

## Instalacion.
0. Instale todos los Pre-Requisitos.
1. Clone el repositorio.

### Back End.
2. Ir a ..\CommentsApp\CommentsAPI
3. Abrir CommentsAPI.sln
4. Espere a que se instalen las dependencias.
    - Si lo desea, vaya a appsettings.json y cambie la connection string.
    - Si las dependencias no se instalan automaticamente salte al proximo paso y ejecute el comando "dotnet restore"
5. Vaya a Herramientas -> Administrador de paquetes NuGet -> Consola del Administador de Paquetes.
6. Ingrese el comando "Update-Database"

### Front End.
2. Abra CommentsApp en su IDE y abra una consola de comando.
3. Ingrese el comando "cd AppComments"
4. Ingrese el comando "npm install -g @angular/cli"
5. Ingrese el comando "npm install"
6. Si desea cambiar la url de la API vaya a ..\CommentsApp\AppComments\src\environments\environment.ts

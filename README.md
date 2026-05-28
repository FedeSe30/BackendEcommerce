# ECommerce API

REST API para una plataforma de e-commerce construida con .NET 8 y Clean Architecture. Incluye gestión de productos, categorías, usuarios y órdenes, con autenticación JWT y autorización por roles.

---

## Tecnologías

| Tecnología | Uso |
|---|---|
| .NET 8 / ASP.NET Core | Framework principal |
| Entity Framework Core 8 | ORM y migraciones |
| SQLite | Base de datos (por defecto) |
| MediatR 12 | CQRS — Commands, Queries y Handlers |
| JWT Bearer | Autenticación |
| BCrypt.Net | Hashing de contraseñas |
| Swagger / OpenAPI | Documentación interactiva |

---

## Arquitectura

El proyecto sigue Clean Architecture con cuatro capas:

```
src/
├── ECommerce.Domain/          # Entidades, Value Objects, excepciones de dominio
├── ECommerce.Application/     # Commands, Queries, Handlers, DTOs, interfaces
├── ECommerce.Infrastructure/  # Repositorios, DbContext, servicios externos
└── ECommerce.WebApi/          # Controllers, startup, configuración
```

### CQRS con MediatR

Toda la lógica de negocio vive en la capa Application, organizada por feature:

```
Application/Features/
├── Auth/
│   └── Commands/   Login
├── Products/
│   ├── Commands/   CreateProduct, UpdateProduct, DeleteProduct
│   └── Queries/    GetAllProducts, GetProductById, SearchProducts
├── Orders/
│   ├── Commands/   CreateOrder
│   └── Queries/    GetOrderById, GetOrdersByUser
├── Users/
│   ├── Commands/   CreateUser, UpdateUser, DeleteUser
│   └── Queries/    GetAllUsers, GetUserById
└── Categories/
    ├── Commands/   CreateCategory
    └── Queries/    GetAllCategories, GetCategoryById
```

Los controllers solo despachan Commands o Queries al mediator — no contienen lógica de negocio ni acceden directamente a repositorios.

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022+ o VS Code con extensión C#

---

## Instalación y ejecución

```powershell
# 1. Clonar el repositorio
git clone <url-del-repo>
cd ECommerce

# 2. Restaurar dependencias
dotnet restore

# 3. Aplicar migraciones (crea ecommerce.db)
dotnet ef database update --project src/ECommerce.Infrastructure --startup-project src/ECommerce.WebApi

# 4. Ejecutar la API
dotnet run --project src/ECommerce.WebApi
```

La API estará disponible en `http://localhost:5000`.  
Swagger UI se abre automáticamente en `http://localhost:5000/swagger`.

---

## Configuración

El archivo principal de configuración es `src/ECommerce.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ecommerce.db"
  },
  "Jwt": {
    "Key": "<clave-secreta-minimo-32-caracteres>",
    "Issuer": "ecommerce-api",
    "Audience": "ecommerce-frontend",
    "ExpirationHours": 1
  }
}
```

> **Importante:** Reemplaza `Key` con una clave secreta propia antes de desplegar en producción. Nunca expongas la clave real en el repositorio.

---

## Endpoints

### Autenticación

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/auth/login` | Login — devuelve JWT token |

### Usuarios

| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| POST | `/api/users` | Público | Registrar nuevo usuario |
| GET | `/api/users` | Admin | Listar todos los usuarios |
| GET | `/api/users/{id}` | Admin | Obtener usuario por ID |
| PUT | `/api/users/{id}` | Admin | Actualizar nombre o rol |
| DELETE | `/api/users/{id}` | Admin | Eliminar usuario |

### Productos

| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| GET | `/api/products` | Público | Listar todos los productos |
| GET | `/api/products/{id}` | Público | Obtener producto por ID |
| GET | `/api/products/search?term={texto}` | Público | Buscar productos por nombre |
| POST | `/api/products` | Admin | Crear producto |
| PUT | `/api/products/{id}` | Admin | Actualizar producto |
| DELETE | `/api/products/{id}` | Admin | Eliminar producto |

### Categorías

| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| GET | `/api/categories` | Público | Listar categorías |
| GET | `/api/categories/{id}` | Público | Obtener categoría por ID |
| POST | `/api/categories` | Admin | Crear categoría |

### Órdenes

| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| POST | `/api/orders` | Autenticado | Crear orden |
| GET | `/api/orders/{id}` | Autenticado | Obtener orden por ID |
| GET | `/api/orders/my-orders` | Autenticado | Listar las órdenes del usuario autenticado |
| GET | `/api/orders/user/{userId}` | Admin | Listar órdenes de cualquier usuario |

### Autenticación en los requests

Incluir el token JWT en el header de cada request protegido:

```
Authorization: Bearer <token>
```

En Swagger, usar el botón **Authorize** e ingresar `Bearer <token>`.

---

## Base de datos

El esquema cuenta con cinco tablas principales:

| Tabla | Descripción |
|---|---|
| `Users` | Cuentas de usuario (roles: `User` / `Admin`) |
| `Categories` | Categorías de productos |
| `Products` | Catálogo de productos con stock y precio |
| `Orders` | Órdenes de compra con estado y total |
| `OrderItems` | Ítems de cada orden (producto, cantidad, precio unitario) |

**Estados de una orden:** `Pending` → `Confirmed` → `Shipped` → `Delivered` / `Cancelled`

### Migraciones

```powershell
# Crear nueva migración
dotnet ef migrations add <NombreMigracion> --project src/ECommerce.Infrastructure --startup-project src/ECommerce.WebApi

# Aplicar migraciones pendientes
dotnet ef database update --project src/ECommerce.Infrastructure --startup-project src/ECommerce.WebApi
```

---

## Build y publicación

```powershell
# Build
dotnet build

# Publicar para producción
dotnet publish src/ECommerce.WebApi -c Release -o ./publish
```

---

## Roles y permisos

| Rol | Permisos |
|---|---|
| `User` | Registrarse, hacer login, crear y ver sus órdenes |
| `Admin` | Todo lo anterior + gestión completa de productos, categorías y usuarios |

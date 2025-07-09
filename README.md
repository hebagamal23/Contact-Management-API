# 📇 Contact Management API

This is a simple .NET Core Web API application for **user registration and contact management**.  
Users can register, log in, and manage their personal address book securely.

---

## 🚀 Features

- ✅ User registration & login (JWT-based authentication)
- ✅ Password hashing using ASP.NET Identity
- ✅ Add, get, and delete contacts
- ✅ Server-side validation for all inputs
- ✅ Clean, RESTful API responses with status codes and messages

---

## 🔐 Authentication

All contact-related APIs require JWT token authorization.  
After login, use the token in the request header like this:

```
Authorization: Bearer your_token_here
```

---

## 📁 API Endpoints

### 🔑 Auth

| Method | Endpoint              | Description          |
|--------|-----------------------|----------------------|
| POST   | `/api/auth/register`  | Register a new user  |
| POST   | `/api/auth/login`     | Log in and get token |

### 👥 Contacts (Requires JWT)

| Method | Endpoint              | Description                             |
|--------|-----------------------|-----------------------------------------|
| POST   | `/api/contacts`       | Add a contact                           |
| GET    | `/api/contacts`       | Get all contacts (with sorting & pagination) |
| GET    | `/api/contacts/{id}`  | Get a contact by ID                     |
| DELETE | `/api/contacts/{id}`  | Delete a contact                        |

---

## 📦 Request Example (Add Contact)

```json
POST /api/contacts

{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@example.com",
  "phoneNumber": "+201234567890",
  "birthDate": "1990-01-01"
}
```

---

## 🛠️ Technologies Used

- ASP.NET Core Web API (.NET 8 or later)
- Entity Framework Core (Code First)
- SQL Server
- JWT Authentication
- Microsoft Identity PasswordHasher
- Manual input validation
- Swagger (for API testing)

---

## ▶️ Running the Project Locally

1. **Clone the repository**  
```bash
git clone https://github.com/hebagamal23/Contact-Management-API.git
```

2. **Open the solution** in Visual Studio

3. **Update the connection string** in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ContactDb;Trusted_Connection=True;"
}
```

4. **Run EF migrations & start the app**  
```bash
dotnet ef database update
dotnet run
```

5. Open **Swagger UI** at:  
```
https://localhost:{your-port}/swagger
```

---

## 🧪 Example Credentials for Testing

```json
POST /api/auth/register

{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "StrongPass1"
}
```

After login, use the token in your API requests as:
```
Authorization: Bearer your_token_here
```

---


## 📄 Project Structure

```
├── Controllers/
│   └── AuthController.cs
│   └── ContactsController.cs
├── DTOs/
├── Models/
├── Data/
├── Program.cs
├── appsettings.json
└── README.md
```

---

## ✨ Author

**Heba Gamal**  
GitHub: [@hebagamal23](https://github.com/hebagamal23)

# 📇 Contact Management API

This is a simple .NET Core Web API application for **user registration and contact management**.  
Users can register, log in, and manage their personal address book securely.

---

## 🚀 Features

- ✅ User registration & login (JWT-based authentication)
- ✅ Password hashing using ASP.NET Identity
- ✅ Add, get, and delete contacts
- ✅ Server-side validation for all inputs
- ✅ Pagination & Sorting of contacts
- ✅ Clean, RESTful API responses with status codes and messages

---

## 🔐 Authentication

All contact-related APIs require JWT token authorization.  
After login, use the token in the header like this:


---

## 📁 API Endpoints

### 🔑 Auth

| Method | Endpoint           | Description          |
|--------|--------------------|----------------------|
| POST   | `/api/auth/register` | Register a new user |
| POST   | `/api/auth/login`    | Log in and get token |

### 👥 Contacts (Requires JWT)

| Method | Endpoint              | Description                    |
|--------|-----------------------|--------------------------------|
| POST   | `/api/contacts`       | Add a contact                  |
| GET    | `/api/contacts`       | Get all contacts (with sorting & pagination) |
| GET    | `/api/contacts/{id}`  | Get a contact by ID            |
| DELETE | `/api/contacts/{id}`  | Delete a contact               |

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

🛠️ Technologies Used
ASP.NET Core Web API (.NET 7 or later)

Entity Framework Core (Code First)

SQL Server

JWT Authentication

Microsoft Identity PasswordHasher

Fluent Validation (manually)

Swagger (for testing)


# Secure Login System

This is a **secure login system** built with **VB.NET** Windows Forms and **SQL Server** as the backend database.

---

## Features

- Secure user registration and login
- Password hashing with **PBKDF2** (using `Rfc2898DeriveBytes`) and a unique salt for each user
- Uses parameterized SQL queries to prevent SQL Injection
- Reusable design that can be adapted for other projects
- Connection to local SQL Server Express database

---

## Technologies Used

- VB.NET (Windows Forms)
- SQL Server Express
- Visual Studio 2022
- Git and GitHub for version control

---

## How to Use

1. Clone or download this repository.
2. Open the solution file (`.sln`) in Visual Studio 2022.
3. Update the connection string in `App.config` to match your SQL Server instance.
4. Create the database and `Users` table using the provided SQL script (`UsersTable.sql`).
5. Build and run the application.
6. Register new users and login securely.

---

## Database Schema

```sql
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARBINARY(64) NOT NULL,
    Salt VARBINARY(16) NOT NULL
);


## Security

- Passwords are never stored in plain text.
- Each password is hashed with a unique salt using PBKDF2 with SHA256.
- SQL commands use parameters to prevent injection attacks.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

Created by [bilal-ilyas](https://github.com/bilal-ilyas).

Feel free to open issues or contribute!


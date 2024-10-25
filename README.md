# RuneFlipper Server

Overview
The backend for RuneFlipper connects the front end and database to create a seamless experience for RuneScape players to track their sales on the Grand Exchange. This API enables tracking sales, monitoring items, and managing user data efficiently.

Tech Stack
Language: C#
Framework: ASP.NET Core
Database: PostgreSQL
ORM: Entity Framework Core
Authentication: ASP.NET Core Identity
Prerequisites
.NET 8
PostgreSQL
Setup Instructions
1. Clone the repository
bash
Copy code
git clone <repository-url>
cd RuneFlipper
2. Configure Environment Variables
Create an .env file in the root of the project and add the following:

plaintext
Copy code
DB_CONNECTION_STRING=<your-postgresql-connection-string>
JWT_SECRET=<your-jwt-secret>
3. Install Dependencies
Run the following command to install dependencies:

bash
Copy code
dotnet restore
4. Database Setup
To set up the database, run migrations:

bash
Copy code
dotnet ef database update
5. Start the Server
bash
Copy code
dotnet run
Key Features
User Authentication & Authorization: Uses ASP.NET Core Identity to handle secure user authentication and role-based access.
API Endpoints: Supports CRUD operations for tracking sales and monitoring item status on the Grand Exchange.
Contributing
Fork and clone the repo.
Create a new branch for your feature or bug fix.
Submit a pull request with a description of changes.
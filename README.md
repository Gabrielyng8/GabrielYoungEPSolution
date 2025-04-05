# GabrielYoungEPSolution

This project is a web-based polling application built using ASP.NET Core. It allows users to create polls, vote on options, and view poll results. The application supports user authentication and ensures that each user can vote only once per poll.

## Features

- User authentication with ASP.NET Identity.
- Create, view, and vote on polls.
- Real-time vote count visualization using Chart.js.
- Data persistence using Entity Framework Core with SQL Server.
- Option to switch between database and file-based repositories.

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core 9.0
- SQL Server
- Bootstrap 5
- Chart.js

## Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd GabrielYoungEPSolution
   ```

2. **Configure the Database**:
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. **Apply Migrations**:
   ```bash
   dotnet ef database update --project DataAccess
   ```

4. **Run the Application**:
   ```bash
   dotnet run --project GabrielYoungEPSolution
   ```

5. **Access the Application**:
   Open your browser and navigate to `https://localhost:7052` or `http://localhost:5028`.

## Usage

1. **Register and Login**:
   - Create an account or log in to access poll creation and voting features.

2. **Create a Poll**:
   - Navigate to the "Create Poll" page and fill in the poll details.

3. **Vote on a Poll**:
   - View poll details and vote for your preferred option.

4. **View Results**:
   - Poll results are displayed in a bar chart format.

## Development Notes

- The project uses dependency injection to switch between `PollRepository` (database) and `PollFileRepository` (file-based) implementations.
- Ensure that migrations are up-to-date when making changes to the database schema.
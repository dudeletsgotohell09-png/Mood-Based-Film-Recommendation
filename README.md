# 🎬 MoodFlick — Mood-Based Film Recommendation

**MoodFlick** is a premium, modern C# WPF Desktop Application designed to solve "choice overload" by guiding users to the perfect movie matching their emotional state. Instead of scrolling through infinite grids of movies, users take a short 4-question pop quiz, and the application's priority-weighted recommendation engine dynamically queries and ranks films.

---

## 🌟 Key Features

*   **Interactive Pop Quiz**: Captures current mood, genre preference, language, and film era in a modern visual card form.
*   **Priority-Weighted Recommendation Engine**: Ranks database films by matching user answers in order of emotional importance: **Mood (Weight: 8) → Genre (Weight: 4) → Language (Weight: 2) → Era (Weight: 1)**.
*   **Analytical Dashboard**:
    *   Dynamic metrics cards (Total films, active monthly users, average database rating).
    *   Visually rich **Mood Popularity Chart** drawn dynamically using WPF vectors from quiz logging history.
*   **Comprehensive Tabular Film Catalog**:
    *   Presents movies alongside their pop quiz parameter classifications.
    *   Includes a real-time keyword search bar (filters by title, genre, language, or director).
*   **Role-Based Access Control**:
    *   *General Users*: Take quizzes, browse the catalog, search, and view movie descriptions.
    *   *Administrators*: Exclusive access to add, edit, and delete movie records and manage mood tag maps.
*   **Automatic Database Initialization**: Configures and auto-seeds the MySQL database and sample data from `schema.sql` on startup.

---

## 🏗️ Architecture & Technologies

*   **Framework**: .NET 8.0 Windows Presentation Foundation (WPF)
*   **Database**: MySQL Server 8.0+
*   **Driver**: `MySql.Data` (v9.7.0+)
*   **Design Paradigm**: Modular Service-based architecture separating Data layer, Repository/Auth services, and WPF Views/Pages.
*   **Styling**: Modern, high-contrast Slate & Teal color palette, custom ControlTemplates for buttons, text fields, cards, sidebar selectors, and drop shadows.

---

## 💾 Database Schema & Algorithm

MoodFlick operates on a clean relational schema containing:
1.  `users` (username, full_name, password_hash, role)
2.  `films` (title, year, genre, language, rating, era, director, cast, description)
3.  `mood_tags` (mood_id, mood_name)
4.  `film_mood_map` (many-to-many relationship linking films to moods)
5.  `quiz_history` (quiz logs for analytics)

## 🚀 Getting Started

### Prerequisites
1.  **.NET 8.0 SDK** or later installed.
2.  **MySQL Server** (v8.0 or later) running locally.

### Setup Instructions

1.  **Configure Database Connection**:
    Open `appsettings.json` in the root folder and configure your local MySQL server credentials:
    ```json
    {
      "Database": {
        "Server": "localhost",
        "Port": 3306,
        "Database": "mood_flick",
        "User": "root",
        "Password": "YOUR_PASSWORD"
      }
    }
    ```

2.  **Restore & Compile**:
    Run the following command in the project directory to restore dependencies and compile the code:
    ```powershell
    dotnet build
    ```

3.  **Run the Application**:
    Launch the desktop app using:
    ```powershell
    dotnet run
    ```
    *Note: The application will automatically connect to MySQL, verify the database schema, run the script in `Database/schema.sql` to construct tables, and seed sample data.*

---

## 👥 Seeded Logins (for Testing)

You can log in immediately using the pre-seeded accounts:

| Role          | Username | Password   | Privileges |
| :------------ | :------- | :--------- | :--------- |
| **Admin**     | `admin`  | `admin123` | Full CRUD, can add/edit/delete movies in catalog |
| **User**      | `user`   | `user123`  | Can take the quiz, search, and browse the catalog |

---

## 📂 Project Structure

```text
├── Data/
│   └── Db.cs               # Connection management & auto-schema loader
├── Database/
│   └── schema.sql          # DB initialization & seeding script
├── Models/
│   ├── User.cs             # User accounts representation
│   ├── Film.cs             # Movie structure and matching weights
│   └── RecommendationHistory.cs # Log schema for analytical widgets
├── Services/
│   ├── Auth.cs             # Session context & SHA-256 password hashing
│   └── Repo.cs             # Database queries, updates, and metrics
├── Views/
│   ├── LoginWindow.xaml    # User login forms
│   ├── RegisterWindow.xaml # User sign up registration
│   ├── MainWindow.xaml     # Shell containing navigation frame
│   └── Pages/
│       ├── DashboardPage.xaml      # Welcome banner, metrics, and popularity chart
│       ├── RecommendationPage.xaml # ComboBox quiz and weighted results view
│       ├── CatalogPage.xaml        # Movie grid table and admin action bars
│       ├── FilmEditWindow.xaml     # Admin edit / add movie modal dialog
│       ├── ProfilePage.xaml        # Logged user profile form (name & password change trigger)
│       ├── ChangePasswordWindow.xaml # User change password dialog
│       ├── UsersPage.xaml          # Admin user management panel
│       ├── UserAddWindow.xaml      # Admin add user dialog
│       └── ResetPasswordWindow.xaml # Admin password reset override dialog
├── appsettings.json        # Database configuration details
└── MoodFlick.csproj        # MSBuild compilation instructions
```

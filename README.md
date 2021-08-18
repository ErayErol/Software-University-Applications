# MiniFootball

- MiniFootball makes it simple to discover mini football activities happening in your city or nearby, as well as the people that want to participate in them or the people who looking for players.
- If you are Mini Football Field Manager you can also add it to our database and gain more customers.

:dart:  My project for the ASP.NET Core course at SoftUni. (June 2021) 

## :information_source: How It Works

- Manager
   - Only manager can approve games and fields.
   - You can Delete/Update/View all games and fields.

- Admin
  - Only admin can create game, field and cities.
  - You can view all approved games and fields.
  - You can update/delete only yours games and fields.

- User
  - You can view all approved games and fields.

## :hammer_and_pick: Built With

- ASP.NET Core 5.0
- Entity Framework (EF) Core .50
- Microsoft SQL Server Express
- ASP.NET Identity System
- MVC Areas with Multiple Layouts
- MyTested.AspNetCore.Mvc
- Razor Pages, Sections, Partial Views
- Auto Мapping
- Dependency Injection
- Status Code Pages Middleware
- Exception Handling Middleware
- Sorting, Filtering, and Paging with EF Core
- Data Validation, both Client-side and Server-side
- Data Validation in the Models and Input View Models
- Responsive Design
- Bootstrap
- jQuery

## :gear: Application Configurations

### 1. The Connection string 
is in `appsettings.json`. If you use SQLEXPRESS, you should replace `Server=.;` with `Server=.\\SQLEXPRESS;`

### 2. Seeding sample data
would happen once you run the application, including Test Accounts:
  - User: user@user.com / password: 123456
  - Mini Football Field Manager: manager@manager.com / password: 123456
  - Admin: admin@admin.com / password: 123456

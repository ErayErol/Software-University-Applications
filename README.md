# :soccer: MiniFootball 
- My project for the ASP.NET Core course at SoftUni. (June 2021) 
# 🎯 Purpose
- MiniFootball makes it simple to discover mini football activities happening in your city or nearby, as well as the people that want to participate in them or the people who looking for players.
- If you are Mini Football Field Manager you can also add it to our database and gain more customers.
# :information_source: How It Works
- Guest visitors:
  - Can view all approved games and fields.
  - Can register as a user.
- Logged Users:
  - Can join game
  - Can view all approved games and fields.
  - Can become an admin.
- Admin
  - Can join game
  - Only admin can create game, field and cities.
  - Can view all approved games and fields.
  - Can update/delete only yours games and fields.
- Manager (user role):
   - Can join game
   - Only manager can approve games and fields.
   - Can Delete/Update/View all games and fields.
# 🛠 Built with:
- ASP.NET Core 5.0
- Entity Framework (EF) Core 5.0
- MyTested.AspNetCore.Mvc 5.0
- Microsoft SQL Server Express
- ASP.NET Identity System
- MVC Areas with Multiple Layouts
- Razor Pages, Sections, Partial Views
- Auto Мapping
- Dependency Injection
- Sorting, Filtering, and Paging with EF Core
- Data Validation, both Client-side and Server-side
- Data Validation in the Models and Input View Models
- Responsive Design
- Bootstrap
- jQuery
# :gear: Application Configurations
### 1. The Connection string 
is in `appsettings.json`. If you use SQLEXPRESS, you should replace `Server=.;` with `Server=.\\SQLEXPRESS;`
### 2. Database Migrations 
would be applied when you run the application, since the `ASPNETCORE-ENVIRONMENT` is set to `Development`. If you change it, you should apply the migrations yourself.
### 3. Seeding sample data
would happen once you run the application, including Test Accounts:
  - User: user4@user.com / password: 123456
  - Admin: admin1@admin.com / password: 123456
  - Field Manager: manager@manager.com / password: 123456
# :camera: Screenshots

### Login (Guest)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/login.png?raw=true"/>

### Register (Guest)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/register.png?raw=true"/>

### Home Page (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/home-page-logged-user.png?raw=true"/>

### All games (Guest)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/all-games.png?raw=true"/>

### All games (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/all-games2-manager.png?raw=true"/>

### Create game (Admin)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/create-game.png?raw=true"/>

### Create field (Admin)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/create-field.png?raw=true"/>

### Create city (Admin)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/create-city.png?raw=true"/>

### All fields (Guest)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/all-fields.png?raw=true"/>

### All fields (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/all-fields2-manager.png?raw=true"/>

### Field Details 1 (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/field-details.png?raw=true"/>

### Field Details 2 (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/field-details2.png?raw=true"/>

### Game Details 1 (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/game-details.png?raw=true"/>

### Game Details 2 (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/game-details2.png?raw=true"/>

### Fields Approving (Manager)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/approving-fields-manager.png?raw=true"/>

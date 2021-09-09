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
- xUnit
- Microsoft SQL Server
- ASP.NET Identity System
- MVC Areas with Multiple Layouts
- Razor Pages, Sections, Partial Views
- Auto Мapping
- Dependency Injection
- Sorting, Filtering, and Paging with EF Core
- Data Validation, both Client-side and Server-side
- Data Validation in the Models and Input View Models
- Responsive Design
- Bootstrap v5.0 and my own design
- SignalR
- jQuery
# :gear: Application Configurations
### 1. The Connection string 
is in `appsettings.json`. If you use SQLEXPRESS, you should replace `Server=.;` with `Server=.\\SQLEXPRESS;`
### 2. Database Migrations 
would be applied when you run the application, since the `ASPNETCORE-ENVIRONMENT` is set to `Development`. If you change it, you should apply the migrations yourself.
### 3. Seeding sample data
would happen once you run the application, including Test Accounts:
  - User: user@user.com / password: 123456
  - Admin: admin@admin.com / password: 123456

# :scroll: General Requirements

<table width="100%" align="center">
  <tbody>
    <tr>
      <th align="left" width="90%">ASP.NET Core 5.0</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">25+ Views</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">6+ Entity Models</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">6+ Controllers</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">41+ Service methods</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Visual Studio 2019</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Razor + section and partial views</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Web API and RESTful service</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Microsoft SQL Server</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Entity Framework (EF) Core 5.0</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">MVC Areas (Identity, Admin and Manager)</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Bootstrap v5.0 and my own design</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Responsive design</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">ASP.NET Identity System</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">AJAX</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Tests covered at least 65% of business logic</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Error Handling and Data Validation</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Both client-side and server-side validation</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Handle correctly the special HTML characters</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Dependency Injection</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">AutoMapping</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Prevent from security vulnerabilities</th>
      <th width="10%" align="left">✔</th>
    </tr>
  </tbody>
</table>

# :scroll: Additional Requirements

<table width="100%" align="center">
  <tbody>
    <tr>
      <th align="left" width="90%">Well-structured Architecture</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Well-configured Control Flow</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Best practices for Object Oriented design</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">High-quality code</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">OOP principles</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Exception handling</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Strong cohesion and loose coupling</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Readable code</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">UI good-looking and easy to use</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Supporting all major modern Web browsers</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Caching</th>
      <th width="10%" align="left">✔</th>
    </tr>
  </tbody>
</table>


# :scroll: Source Control

<table width="100%" align="center">
  <tbody>
    <tr>
      <th align="left" width="90%">Public source code repository</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Have commits in at least 5 DIFFERENT days</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Have at least 20 commits</th>
      <th width="10%" align="left">✔</th>
    </tr>
  </tbody>
</table>

# :scroll: Bonuses

<table width="100%" align="center">
  <tbody>
    <tr>
      <th align="left" width="90%">SignalR</th>
      <th width="10%" align="left">✔</th>
    </tr>
    <tr>
      <th align="left" width="90%">Host the application in a cloud environment [Azure]</th>
      <th width="10%" align="left">✔</th>
    </tr>
  </tbody>
</table>

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

### All fields (User)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/all-fields-user.png?raw=true"/>

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

### Chat (User)
<img src="https://github.com/ErayErol/MiniFootball/blob/main/MiniFootball/wwwroot/images-github/chat.png?raw=true"/>

## ⭐️Leave a feedback
 - If you like the app, star the repository and show it to your friends!

## 🤝 Acknowledgments
- My Trainer from SoftUni [Ivaylo Kenov](https://github.com/ivaylokenov)

## :standing_person: Author Contacts:
- [Github](https://github.com/erayerol)
- [Instagram](https://www.instagram.com/eray.errol/)
- [Facebook](https://www.facebook.com/profile.php?id=100001781550068)
- [Twitter](https://twitter.com/errayerrol?fbclid=IwAR3KLJXbPc7FI4HS8MDIFtRXquu7VMvKEhTivDxzDN7MhZQPpu3c8mgvXL8)
- [LinkedIn](https://www.linkedin.com/in/eray-erol-a05789170/)

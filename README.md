# eDoctor

HealthTech web app built with ASP.NET Core 10 MVC.

## Prerequisites

- .NET 10 SDK
- Visual Studio
- SSMS
- PayPal Account
- Metered Account

## Setup

Fill out `secrets.json` using this template:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "PayPal": {
    "OAuthClientId": "",
    "OAuthClientSecret": ""
  },
  "Metered": {
    "Username": "",
    "Password": ""
  }
}
```

Right-click `libman.json` in Visual Studio and select "Restore Client-Side Libraries"

Open Package Manager Console and type:

```powershell
Update-Database
```

Open SSMS and run `assets/data/initial-data.sql` to load the initial data

## Testing Data

### PayPal Account

Email: `alexnguyen@test.com`  
Password: `Default@123`

### Doctor Account

Login name: `stephenstrange`, `victorfrankenstein`, `hanalee`, `michaelstone`, `amaranthajohnson`, `elenarossi`, `junpark`, `hiroshitanaka`, `yurihan`, `kangminseo`  
Password: `Default@123`

## Layered Architecture

![Layered architecture](assets/images/layered-architecture.png)

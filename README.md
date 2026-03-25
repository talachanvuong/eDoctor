# eDoctor

HealthTech web app built with ASP.NET Core 10 MVC.

## Preview

### Home

![Home](assets/images/home.png)

### Doctors

![Doctors](assets/images/doctors.png)

### Detail Doctor

![Detail doctor](assets/images/detail-doctor.png)

### User Schedules

![User schedules](assets/images/user-schedules.png)

### User Detail Schedule

![User detail schedule](assets/images/user-detail-schedule.png)

### Medical Record

![Medical record](assets/images/medical-record.png)

### Payment

![Payment](assets/images/payment.png)

### Invoice

![Invoice](assets/images/invoice.png)

### Meeting

![Meeting](assets/images/meeting.png)

### Assistant

![Assistant](assets/images/assistant.png)

## Prerequisites

- .NET 10 SDK
- Visual Studio
- SSMS
- PayPal Account
- Metered Account
- [Another repository](https://github.com/talachanvuong/edoctor-ai)

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
  },
  "WebSocketServer": ""
}
```

Right-click `libman.json` in Visual Studio and select "Restore Client-Side Libraries"

Open Package Manager Console and type:

```powershell
Update-Database
```

Open SSMS and run `assets/data/initial-data.sql` to load the initial data

## Note

Access `/Doctor` for the doctor site

## Testing Data

### PayPal Account

Email: `alexnguyen@test.com`  
Password: `Default@123`

### Doctor Account

Login name: `stephenstrange`, `victorfrankenstein`, `hanalee`, `michaelstone`, `amaranthajohnson`, `elenarossi`, `junpark`, `hiroshitanaka`, `yurihan`, `kangminseo`  
Password: `Default@123`

## Overall System Architecture

![Overall system architecture](assets/images/overall-system-architecture.png)

## Layered Architecture

![Layered architecture](assets/images/layered-architecture.png)

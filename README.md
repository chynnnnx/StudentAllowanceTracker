# Student Allowance Tracker

A **Blazor Server** web application built using **ASP.NET Core and SQL Server** to manage student allowances. This project demonstrates backend and frontend development skills, secure authentication, and modern architecture practices.

---

## Tech Stack

- **Frontend:** Blazor Server  
- **Backend:** ASP.NET Core Web API  
- **Database:** MSSQL  
- **UI:** MudBlazor + TailwindCSS  
- **Authentication:** JWT + Refresh Tokens + ASP.NET Core Identity  
- **Email Integration:** Brevo API for notifications  
- **Architecture:** Clean Architecture with **CQRS + MediatR**  

---

## Features

- User registration and login with **JWT authentication**  
- Role-based access (Student, Admin)  
- Add, update, and track student allowances  
- **CQRS + MediatR** for separation of commands and queries  
- Automatic email notifications for allowance events  
- Dashboard with summary of balances and transactions  
- Responsive UI with MudBlazor components and Tailwind styling  
- Secure backend with proper authentication and authorization  

---

## How to Run

1. Clone the repository  
2. Configure your **appsettings.json** with database and Brevo API credentials  
3. Run database migrations  
4. Start the Blazor Server app  
5. Open the app in your browser

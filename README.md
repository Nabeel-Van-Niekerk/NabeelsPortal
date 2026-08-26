# Agri-Energy Connect Platform

## 📌 Project Overview
The **Agri-Energy Connect** platform is a web application designed to enable farmers, green energy experts, and enthusiasts to collaborate seamlessly. The platform features an integrated marketplace, an educational resource hub, and community forums for discussions aimed at driving innovation in renewable energy solutions and sustainable agricultural practices.

---

## 🛠️ Technical Details

* **Framework**: Built with **C# ASP.NET Core Web App (MVC)**—an ideal architecture for building scalable and robust web applications. It uses the Model-View-Controller design pattern to cleanly separate core business logic, front-end views, and user request handling.
* **Front-end Views**: Utilizes **Razor Pages** to render responsive, resource-efficient interfaces. By combining Razor syntax with standard HTML, only the necessary server-side code is executed per request, reducing overall overhead (Microsoft, 2023).
* **Database**: Powered by a local **SQL Server Database** with a planned cloud migration path for the final product. SQL Server was selected for its structured relational model, seamless cloud compatibility, and ability to handle scaling as the platform expands.
* **Authentication**: Implements **ASP.NET Core Identity** to manage secure authentication, role access, and credential verification for registered users.

---

## 💼 Business Value & Strategic Impact

### 🚀 High Availability & Operational Resilience
By leveraging ASP.NET Core MVC’s decoupled architecture, the platform separates core business logic from user interfaces. 
* **Minimized Downtime**: Fault isolation ensures that a failure in one feature (e.g., forum rendering) does not trigger a full application crash. 
* **Hot-Fix Capabilities**: Developers can deploy targeted patches and updates directly to production without taking the platform offline, ensuring continuous service delivery for commercial operations.
* **Reduced Technical Debt**: Modular components lower long-term maintenance costs and simplify onboarding for new development team members.

### ⚡ Optimized Performance & User Engagement
Rural agricultural areas often face network bandwidth constraints. The combination of ASP.NET Core and Razor Pages directly addresses this infrastructure challenge:
* **Low Latency & High Speed**: Razor pages process logic server-side and transmit lightweight HTML to the client, drastically reducing bandwidth consumption and page load times (Microsoft, 2023).
* **Cross-Device Accessibility**: Efficient server-side rendering ensures smooth performance across low-spec mobile devices and modern desktop browsers alike, maximizing user retention among farmers in remote locations.

### 📈 Scalable Data Architecture & Business Intelligence
Data integrity and structured growth are essential as the platform scales from a localized tool into an industry marketplace:
* **Data Integrity & Efficiency**: Using a normalized SQL Server schema eliminates data redundancy, leading to faster query execution, lower database storage overhead, and quick transactional execution (Nagy, 2018).
* **Seamless Cloud Migration**: Building on SQL Server provides a frictionless path to Microsoft Azure SQL Database, enabling instant scaling as transaction volumes and user concurrency increase.
* **Actionable Analytics**: Clean, relational data structures simplify analytics reporting on green energy adoption rates, forum engagement, and marketplace trends.

### 🔒 Enterprise-Grade Security & Compliance
Trust is critical when handling user credentials and marketplace interactions:
* **Mitigated Risk**: ASP.NET Core Identity automatically enforces industry-standard hashing, salting, anti-forgery tokens (CSRF protection), and session protection, eliminating human error in custom auth implementations (Microsoft, 2023).
* **Regulatory Compliance**: Built-in security mechanisms align with data privacy frameworks (such as POPIA / GDPR), safeguarding user identities and building ecosystem trust among agricultural and energy stakeholders.

---

### 📊 Business Value Matrix

| Architectural Layer | Technical Implementation | Business & Financial Outcome |
| :--- | :--- | :--- |
| **Application Layer** | ASP.NET Core MVC Decoupled Architecture | **High Uptime & Low Maintenance**: Hot-patching reduces system outages and operational overhead. |
| **Presentation Layer** | Optimized Razor Pages | **Maximum Reach**: Performs reliably on low-bandwidth rural networks and budget devices. |
| **Data Layer** | Relational SQL Server Engine | **Scalable Foundation**: Eliminates storage bloat and guarantees smooth Azure cloud migration. |
| **Security Layer** | ASP.NET Core Identity Framework | **Brand Protection**: Industry-grade security mitigates breach risks and builds platform credibility. |


## 🚀 How to Run the Project

### Prerequisites
* **.NET 8.0 SDK** (or higher)
* **SQL Server Express / LocalDB** (included automatically with Visual Studio)

---

### Method 1: Using Visual Studio 2022 (Easiest)
1. **Open the Project**:
   * Double-click the `.sln` (Solution) file in your project folder, **OR**
   * Open Visual Studio, click **Open a project or solution**, and select the `.sln` file.
2. **Apply Database Migrations**:
   * Open the Package Manager Console (**Tools** > **NuGet Package Manager** > **Package Manager Console**).
   * Run:
     ```powershell
     Update-Database
     ```
3. **Run**:
   * Press `F5` or click the green **Play** button (**IIS Express / AgriEnergyConnect**) in the top toolbar.

---

### Method 2: Using Visual Studio Code
1. **Open the Project Folder**:
   * Launch VS Code, click **File** > **Open Folder...**, and select the root project directory.
2. **Open Terminal**:
   * Open the built-in terminal (`Ctrl` + `` ` `` or **Terminal** > **New Terminal**).
3. **Update Database & Start**:
   ```bash
   dotnet ef database update
   dotnet run

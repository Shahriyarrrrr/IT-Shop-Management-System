# 🛒 IT Shop Management System

A complete, multi-role **desktop application** built with **C# (.NET WinForms)** and **SQL Server**, designed to streamline operations for an IT product retail store. This system supports separate dashboards for **Admins**, **Employees**, and **Customers**, ensuring a smooth and intuitive experience for every user type.

---

## 🚀 Features

### 👤 User Roles
- **Admin**: Add, update, and manage employees and customers.
- **Employee**: Manage product inventory (GPU, CPU, RAM, Monitor, Mechanical Keyboard).
- **Customer**: Browse products, add to cart, and place orders with integrated payment methods.

### 📦 Product Management
- Add, update, delete product details including image, category, price, and stock.
- View categorized product listings.
- Real-time stock management and cart interaction.

### 🛒 Cart & Checkout
- Add items to cart from any category.
- View cart with editable quantities and total price.
- Checkout with **Cash on Delivery** or **Card Payment** options.

### 🔐 Authentication & Security
- Role-based login system.
- Separate registration and login for Admins, Employees, and Customers.

---

## 🗂️ Tech Stack

| Layer              | Technology                                |
|--------------------|--------------------------------------------|
| **Frontend**       | C# Windows Forms (WinForms)                |
| **Backend**        | SQL Server (with normalized schema)        |
| **Database Access**| ADO.NET                                    |
| **IDE**            | Visual Studio 2022                         |
| **Version Control**| Git & GitHub                               |

---

## 🖼️ Screenshots

> ✅ Add your own screenshots here (e.g., login screen, dashboards, product list, cart, etc.)

---

## ⚙️ Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/Shahriyarrrrr/IT-Shop-Management-System.git

2. Open in Visual Studio
Open IT-Shop-Management-System.sln.

3. Setup the Database
Open SQL Server Management Studio (SSMS).

Restore or attach ITShopDB.mdf (provided in the repo or from original system).

Make sure the connection string in code matches your local server.

4. Run the Application
Build and run from Visual Studio.

Login using default credentials or create new users from the app.

├── Form*.cs            # All WinForms UI screens
├── CartItem, Product   # Product & Cart Forms per category
├── FormAdmin, FormCustomerDashboard, etc.
├── SQLQuery1.sql       # Sample SQL setup/query file
├── ITShopDB.mdf        # SQL Server DB file
├── .gitignore
└── README.md
🙌 Author
Shahriyar Rahman
https://github.com/Shahriyarrrrr


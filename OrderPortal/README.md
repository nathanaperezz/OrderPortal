# Online Store Portal

A simple .NET MVC web application for an online store, built with jQuery and Razor syntax without complex frameworks like Entity Framework.

## Features

- **Product Catalog**: Browse products by category with filtering
- **Shopping Cart**: Add, update, and remove items from cart
- **Order Management**: Submit orders and track cart status
- **Responsive Design**: Modern UI using Bootstrap 5
- **Real-time Updates**: Cart count updates dynamically

## Database Structure

The application uses SQL Server with the following tables:
- `Login` - User authentication
- `Products` - Product catalog
- `Cart` - Shopping cart headers
- `CartItem` - Shopping cart items
- `CustomerShipTo` - Shipping addresses
- `vwCartItems` - View for cart items with product details

## Setup Instructions

### 1. Database Setup

1. Create the database using the provided SQL script in SQL Server Management Studio:
```sql
CREATE DATABASE OnlineStore;
GO
USE OnlineStore;
GO

-- Login table
CREATE TABLE Login (
    LoginPK INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NULL,
    CustomerId INT NOT NULL
);

-- Products table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    SKU NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Category NVARCHAR(100),
    Price DECIMAL(10,2) NOT NULL,
    UoM NVARCHAR(20),
    EachesToPallet INT,
    Image NVARCHAR(255)
);

-- Cart table
CREATE TABLE Cart (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    CustomerPO NVARCHAR(50),
    DueDate DATE,
    Submitted BIT DEFAULT 0,
    LoginPK INT NOT NULL,
    FOREIGN KEY (LoginPK) REFERENCES Login(LoginPK)
);

-- CartItem table
CREATE TABLE CartItem (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Qty INT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Cart(CartId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- CustomerShipTo table
CREATE TABLE CustomerShipTo (
    CustomerShipToPK INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    ShipToAddressId NVARCHAR(50),
    Address1 NVARCHAR(255),
    Address2 NVARCHAR(255),
    City NVARCHAR(100),
    State NVARCHAR(50),
    Zip NVARCHAR(20),
    Phone NVARCHAR(20)
);

-- View for CartItems
CREATE VIEW vwCartItems AS
SELECT 
    ci.CartId,
    ci.ProductId,
    ci.Qty,
    p.SKU,
    p.Description,
    p.Price,
    (ci.Qty / NULLIF(p.EachesToPallet, 0)) AS Pallets,
    (ci.Qty * p.Price) AS ExtendedAmount
FROM CartItem ci
JOIN Products p ON ci.ProductId = p.ProductId;
```

2. Run the `SampleData.sql` script to populate the database with sample data.

### 2. Connection String

Update the connection string in `appsettings.json` to point to your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "StoreContext": "Server=localhost\\SQLEXPRESS;Database=OnlineStore;Trusted_Connection=True;"
  }
}
```

### 3. Run the Application

1. Open the solution in Visual Studio or run from command line:
```bash
dotnet run
```

2. Navigate to `https://localhost:5001` or `http://localhost:5000`

## Technology Stack

- **.NET 8.0** - Framework
- **ASP.NET Core MVC** - Web framework
- **Dapper** - Micro ORM for data access
- **Bootstrap 5** - CSS framework
- **jQuery** - JavaScript library
- **SQL Server** - Database

## Project Structure

```
OrderPortal/
├── Controllers/
│   ├── HomeController.cs
│   └── StoreController.cs
├── Data/
│   └── StoreRepository.cs
├── Models/
│   ├── Product.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── CartItemView.cs
│   └── Login.cs
├── Views/
│   ├── Store/
│   │   ├── Index.cshtml
│   │   └── Cart.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   ├── css/
│   └── js/
├── appsettings.json
├── Program.cs
└── README.md
```

## Key Features Implementation

### Data Access Pattern
The application uses the provided data access pattern with Dapper:
```csharp
public List<Product> GetAllProducts()
{
    try
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT * FROM Products ORDER BY Description";
            return con.Query<Product>(sql).ToList();
        }
    }
    catch (Exception)
    {
        return new List<Product>();
    }
}
```

### Cart Management
- Automatic cart creation for users
- Add/remove items with quantity updates
- Real-time cart count updates
- Order submission functionality

### Product Catalog
- Category-based filtering
- Product cards with images
- Add to cart functionality
- Responsive grid layout

## Demo Mode

The application currently runs in demo mode with hardcoded customer and login IDs:
- CustomerId: 1
- LoginPK: 1

In a production environment, these would be managed through proper authentication and session management.

## Future Enhancements

- User authentication and authorization
- Order history and tracking
- Payment processing integration
- Admin panel for product management
- Email notifications
- Advanced search and filtering

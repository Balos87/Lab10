SELECT 
	ProductName, UnitPrice, CategoryID = Categories.CategoryName 
FROM 
	Products 
JOIN 
	Categories 
ON 
	Categories.CategoryID = Products.CategoryID
ORDER BY 
    CategoryName ASC,
    ProductName ASC;
GO

SELECT 
    Customers.CompanyName, 
    COUNT(Orders.OrderID) AS NumberOfOrders
FROM 
    Customers 
JOIN 
    Orders 
ON 
    Customers.CustomerID = Orders.CustomerID
GROUP BY 
    Customers.CompanyName
ORDER BY 
    NumberOfOrders DESC;
GO

SELECT 
    EmployeeID = Employees.EmployeeID,
    LastName,
    FirstName,
    TerritoryDescription
FROM 
    Employees 
JOIN 
    EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN 
    Territories ON Territories.TerritoryID = EmployeeTerritories.TerritoryID
ORDER BY 
    LastName, FirstName;


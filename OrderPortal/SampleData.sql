-- Sample data for OnlineStore database
USE OnlineStore;

-- Insert sample login
INSERT INTO Login (Email, Password, CustomerId) VALUES ('test@example.com', NULL, 1);

-- Insert sample products
INSERT INTO Products (SKU, Description, Category, Price, UoM, EachesToPallet, Image) VALUES
('PROD001', 'Premium Wooden Chair', 'Furniture', 299.99, 'Each', 24, 'https://via.placeholder.com/300x200?text=Chair'),
('PROD002', 'Office Desk', 'Furniture', 599.99, 'Each', 12, 'https://via.placeholder.com/300x200?text=Desk'),
('PROD003', 'LED Desk Lamp', 'Lighting', 89.99, 'Each', 48, 'https://via.placeholder.com/300x200?text=Lamp'),
('PROD004', 'Ergonomic Office Chair', 'Furniture', 449.99, 'Each', 18, 'https://via.placeholder.com/300x200?text=Office+Chair'),
('PROD005', 'Wireless Mouse', 'Electronics', 29.99, 'Each', 100, 'https://via.placeholder.com/300x200?text=Mouse'),
('PROD006', 'Mechanical Keyboard', 'Electronics', 129.99, 'Each', 50, 'https://via.placeholder.com/300x200?text=Keyboard'),
('PROD007', 'Monitor Stand', 'Electronics', 79.99, 'Each', 36, 'https://via.placeholder.com/300x200?text=Monitor+Stand'),
('PROD008', 'Desk Organizer', 'Accessories', 39.99, 'Each', 72, 'https://via.placeholder.com/300x200?text=Organizer'),
('PROD009', 'Cable Management Kit', 'Accessories', 19.99, 'Each', 120, 'https://via.placeholder.com/300x200?text=Cable+Kit'),
('PROD010', 'Anti-Fatigue Mat', 'Accessories', 49.99, 'Each', 60, 'https://via.placeholder.com/300x200?text=Mat');

-- Insert sample customer ship-to address
INSERT INTO CustomerShipTo (CustomerId, ShipToAddressId, Address1, Address2, City, State, Zip, Phone) VALUES
(1, 'ADDR001', '123 Main Street', 'Suite 100', 'Anytown', 'CA', '90210', '555-123-4567');

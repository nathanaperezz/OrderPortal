using Dapper;
using OrderPortal.Models;
using System.Data.SqlClient;

namespace OrderPortal.Data
{
    public class StoreRepository
    {
        private readonly string connectionString;

        public StoreRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StoreContext") ?? string.Empty;
        }

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

        public List<Product> GetProductsByCategory(string category)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT * FROM Products WHERE Category = @Category ORDER BY Description";
                    return con.Query<Product>(sql, new { Category = category }).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public Product? GetProductById(int productId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT * FROM Products WHERE ProductId = @ProductId";
                    return con.QueryFirstOrDefault<Product>(sql, new { ProductId = productId });
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CartItemView> GetCartItems(int cartId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT * FROM vwCartItems WHERE CartId = @CartId";
                    return con.Query<CartItemView>(sql, new { CartId = cartId }).ToList();
                }
            }
            catch (Exception)
            {
                return new List<CartItemView>();
            }
        }

        public Cart? GetCartByLogin(int loginPK)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT * FROM Cart WHERE LoginPK = @LoginPK AND Submitted = 0";
                    return con.QueryFirstOrDefault<Cart>(sql, new { LoginPK = loginPK });
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int CreateCart(int customerId, int loginPK, string? customerPO = null, DateTime? dueDate = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = @"INSERT INTO Cart (CustomerId, CustomerPO, DueDate, Submitted, LoginPK) 
                                  VALUES (@CustomerId, @CustomerPO, @DueDate, 0, @LoginPK);
                                  SELECT CAST(SCOPE_IDENTITY() as int)";
                    return con.QuerySingle<int>(sql, new { CustomerId = customerId, CustomerPO = customerPO, DueDate = dueDate, LoginPK = loginPK });
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool AddToCart(int cartId, int productId, int qty)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    
                    // Check if item already exists in cart
                    string checkSql = "SELECT CartItemId FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
                    var existingItem = con.QueryFirstOrDefault<int?>(checkSql, new { CartId = cartId, ProductId = productId });
                    
                    if (existingItem.HasValue)
                    {
                        // Update existing item
                        string updateSql = "UPDATE CartItem SET Qty = Qty + @Qty WHERE CartItemId = @CartItemId";
                        con.Execute(updateSql, new { Qty = qty, CartItemId = existingItem.Value });
                    }
                    else
                    {
                        // Add new item
                        string insertSql = "INSERT INTO CartItem (CartId, ProductId, Qty) VALUES (@CartId, @ProductId, @Qty)";
                        con.Execute(insertSql, new { CartId = cartId, ProductId = productId, Qty = qty });
                    }
                    
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCartItemQty(int cartItemId, int qty)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "UPDATE CartItem SET Qty = @Qty WHERE CartItemId = @CartItemId";
                    con.Execute(sql, new { Qty = qty, CartItemId = cartItemId });
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCartItem(int cartId, int productId, int qty)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    
                    // Check if item exists in cart
                    string checkSql = "SELECT CartItemId FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
                    var existingItem = con.QueryFirstOrDefault<int?>(checkSql, new { CartId = cartId, ProductId = productId });
                    
                    if (existingItem.HasValue)
                    {
                        // Update existing item
                        string updateSql = "UPDATE CartItem SET Qty = @Qty WHERE CartItemId = @CartItemId";
                        con.Execute(updateSql, new { Qty = qty, CartItemId = existingItem.Value });
                    }
                    else
                    {
                        // Add new item
                        string insertSql = "INSERT INTO CartItem (CartId, ProductId, Qty) VALUES (@CartId, @ProductId, @Qty)";
                        con.Execute(insertSql, new { CartId = cartId, ProductId = productId, Qty = qty });
                    }
                    
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveFromCart(int cartId, int productId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "DELETE FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
                    con.Execute(sql, new { CartId = cartId, ProductId = productId });
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SubmitCart(int cartId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "UPDATE Cart SET Submitted = 1 WHERE CartId = @CartId";
                    con.Execute(sql, new { CartId = cartId });
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Login? GetLoginByEmail(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT * FROM Login WHERE Email = @Email";
                    var login = con.QueryFirstOrDefault<Login>(sql, new { Email = email });
                    
                    // For demo purposes, if login doesn't exist, create it
                    if (login == null)
                    {
                        string insertSql = @"INSERT INTO Login (Email, Password, CustomerId) 
                                            VALUES (@Email, NULL, 1);
                                            SELECT CAST(SCOPE_IDENTITY() as int)";
                        int loginId = con.QuerySingle<int>(insertSql, new { Email = email });
                        
                        login = new Login
                        {
                            LoginPK = loginId,
                            Email = email,
                            Password = null,
                            CustomerId = 1
                        };
                    }
                    
                    return login;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetCategories()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "SELECT DISTINCT Category FROM Products WHERE Category IS NOT NULL ORDER BY Category";
                    return con.Query<string>(sql).ToList();
                }
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}

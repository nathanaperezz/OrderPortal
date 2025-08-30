using Microsoft.AspNetCore.Mvc;
using OrderPortal.Data;
using OrderPortal.Models;
using System.Text.Json;

namespace OrderPortal.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreRepository _repository;

        public StoreController(StoreRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var products = _repository.GetAllProducts();
            return View(products);
        }

        public IActionResult Product(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // Check if user is logged in and has this product in cart
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            int currentQuantity = 1; // Default quantity
            
            if (loginPK.HasValue)
            {
                var cart = _repository.GetCartByLogin(loginPK.Value);
                if (cart != null)
                {
                    var cartItems = _repository.GetCartItems(cart.CartId);
                    var existingItem = cartItems.FirstOrDefault(item => item.ProductId == id);
                    if (existingItem != null)
                    {
                        currentQuantity = existingItem.Qty;
                    }
                }
            }

            ViewBag.CurrentQuantity = currentQuantity;
            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int qty)
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            
            if (!loginPK.HasValue || !customerId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to add items to cart" });
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                // Create new cart
                int cartId = _repository.CreateCart(customerId.Value, loginPK.Value);
                if (cartId == 0)
                {
                    return Json(new { success = false, message = "Failed to create cart" });
                }
                cart = new Cart { CartId = cartId };
            }

            bool success = _repository.AddToCart(cart.CartId, productId, qty);
            return Json(new { success = success, message = success ? "Added to cart" : "Failed to add to cart" });
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int qty)
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            
            if (!loginPK.HasValue || !customerId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to update cart" });
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                // Create new cart
                int cartId = _repository.CreateCart(customerId.Value, loginPK.Value);
                if (cartId == 0)
                {
                    return Json(new { success = false, message = "Failed to create cart" });
                }
                cart = new Cart { CartId = cartId };
            }

            bool success = _repository.UpdateCartItem(cart.CartId, productId, qty);
            return Json(new { success = success, message = success ? "Updated cart" : "Failed to update cart" });
        }

        public IActionResult Cart()
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            
            if (!loginPK.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                return View(new List<CartItemView>());
            }

            var cartItems = _repository.GetCartItems(cart.CartId);
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult UpdateCartItem(int cartItemId, int qty)
        {
            bool success = _repository.UpdateCartItemQty(cartItemId, qty);
            return Json(new { success = success });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            
            if (!loginPK.HasValue)
            {
                return Json(new { success = false });
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                return Json(new { success = false });
            }

            bool success = _repository.RemoveFromCart(cart.CartId, productId);
            return Json(new { success = success });
        }

        [HttpPost]
        public IActionResult SubmitCart()
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            
            if (!loginPK.HasValue)
            {
                return Json(new { success = false, message = "Please log in to submit orders" });
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                return Json(new { success = false, message = "No active cart found" });
            }

            bool success = _repository.SubmitCart(cart.CartId);
            return Json(new { success = success, message = success ? "Order submitted successfully" : "Failed to submit order" });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            // Get user info from session
            var loginPK = HttpContext.Session.GetInt32("LoginPK");
            
            if (!loginPK.HasValue)
            {
                return Json(new { count = 0 });
            }

            var cart = _repository.GetCartByLogin(loginPK.Value);
            if (cart == null)
            {
                return Json(new { count = 0 });
            }

            var cartItems = _repository.GetCartItems(cart.CartId);
            int totalItems = cartItems.Sum(item => item.Qty);

            return Json(new { count = totalItems });
        }
    }
}

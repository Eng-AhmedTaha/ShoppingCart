// Program.cs




 // إضافة خدمات الـ
 // Session

using ShoppingCartApp.Models;
using System.Net.Http;

builder.Services.AddSession();
builder.Services.AddControllersWithViews();




// تذهب 
// Middleware
// لتفعيل
// الـ
// Session
app.UseSession();
app.UseStaticFiles();
app.MapDefaultControllerRoute();







//إنشاء موديل
//(Model)
//يمثل المنتج

namespace ShoppingCartApp.Models
{
    // نموذج المنتج
    public class Product
    {
        public int Id { get; set; } // معرّف المنتج
        public string Name { get; set; } // اسم المنتج
        public decimal Price { get; set; } // سعر المنتج
    }
}





// إنشاء الموديل الخاص بعربة التسوق

namespace ShoppingCartApp.Models
{
    // عنصر في عربة التسوق
    public class CartItem
    {
        public Product Product { get; set; } // المنتج نفسه
        public int Quantity { get; set; } // الكمية المطلوبة
    }
}








//  إنشاء Controller
//  لعربة التسوق
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApp.Models;
using System.Collections.Generic;

// تحميل
using Newtonsoft.Json; // لتحويل
                       // البيانات
                       // إلى
                       // JSON
                       // و
                       // استخدام
                       // Session

namespace ShoppingCartApp.Controllers
{
    public class CartController : Controller
    {
        // مفتاح الـ Session
        // لتخزين عربة التسوق
        private const string CartSessionKey = "Cart";

        // صفحة
        // لكي يتم
        // عرض عربة التسوق

        public IActionResult Index()
        {
            var cart = GetCartFromSession(); // استرجاع عربة
                                             // التسوق من
                                             // الـ Session
            return View(cart); // تمريرها
                               // إلى الـ View
        }

        // إضافة منتج إلى عربة التسوق
        public IActionResult AddToCart(int id, string name, decimal price)
        {
            // استرجاع عربة
            // التسوق من
            // الـ Session
            var cart = GetCartFromSession();

            // التحقق إذا كان المنتج موجودًا بالفعل
            var item = cart.Find(c => c.Product.Id == id);
            if (item != null)
            {
                item.Quantity++; // زيادة الكمية إذا كان المنتج موجودًا
            }
            else
            {
                // إضافة منتج جديد إذا لم يكن موجودًا
                cart.Add(new CartItem
                {
                    Product = new Product { Id = id, Name = name, Price = price },
                    Quantity = 1  // defaut add
                });
            }





            // تحديث
            // عربة
            // التسوق
            // في
            // الـ Session
            SaveCartToSession(cart);

            return RedirectToAction("Index"); // العودة إلى صفحة عربة التسوق
        }



        // تخزين عربة
        // التسوق في
        // الـ Session
        private void SaveCartToSession(List<CartItem> cart)
        {
            var sessionData = JsonConvert.SerializeObject(cart); // تحويل القائمة إلى
                                                                 // JSON

            HttpContext.Session.SetString(CartSessionKey, sessionData); // تخزينها



        }




        // حذف منتج من عربة التسوق
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCartFromSession(); // استرجاع عربة التسوق
            var item = cart.Find(c => c.Product.Id == id); // البحث عن المنتج
            if (item != null)
            {
                cart.Remove(item); // حذف المنتج إذا كان موجودًا
            }

            SaveCartToSession(cart); // تحديث الـ
                                     // Session
            return RedirectToAction("Index"); // العودة إلى صفحة عربة التسوق
        }





        // استرجاع عربة
        // التسوق
        // من الـ
        // Session
        private List<CartItem> GetCartFromSession()
        {
            // استرجاع البيانات كـ
            // JSON
            // وتحويلها
            // إلى
            // قائمة
            var sessionData = HttpContext.Session.GetString(CartSessionKey);
            return sessionData == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(sessionData);
        }

    
    }
}
// إنشاء
// الـ View


//------------------------------------------------

/*
 1- models
 2-   إضافة خدمات الـ
    Session
 3-  تذهب 
 Middleware
 لتفعيل
الـ
 Session
 
 4- Controller
 
 
 
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class UserController : Controller
    {
        BookStoreEntities db = new BookStoreEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer taikhoan)
        {
            if (ModelState.IsValid)
            {
                 var taikhoanAdmin = db.AdminAccount.FirstOrDefault(k => k.Email == taikhoan.Email && k.Password == taikhoan.Password);

                if (taikhoanAdmin != null)
                {
                    Session["TaiKhoan"] = taikhoanAdmin;
                    return RedirectToAction("Index", "Admin/OrderAdmin");
                }
               
                if (ModelState.IsValid)
                {
                    var account = db.Customer.FirstOrDefault(k => k.Email == taikhoan.Email && k.Password == taikhoan.Password);
                    if (account != null)
                    {
                        Session["TaiKhoan"] = account;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {

                        ViewBag.ThongBao = "Email hoặc mật khẩu không chính xác";
                    }
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session["TaiKhoan"] = null;
            Session["GioHang"] = null;

            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Customer cus,string rePass)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = db.Customer.FirstOrDefault(c => c.Email == cus.Email);
                if (checkEmail != null)
                {
                    ViewBag.ThongBaoEmail = "Đã có tài khoản đăng nhập bằng Email này";
                    return View();
                }
                if(cus.Password == rePass)
                {
                    db.Customer.Add(cus);
                    db.SaveChanges();
                    return RedirectToAction("Login","User");
                }
                else
                {
                    ViewBag.ThongBao = "Mật khẩu xác nhận không chính xác";
                    return View();
                }
            }
            return View();
        }

        
    }
}
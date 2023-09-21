using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class OrderAdminController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Admin/Order
        public ActionResult Index()
        {
            var Order = (from o in db.Order orderby o.IDOrder descending select o).ToList();
            return View(Order.ToList());
        }
            
        // GET: Admin/Order/Details/5
        public ActionResult Details(int id)
        {
            var detail = db.OrderDetail.Where(d => d.IDOrder == id).ToList();
            ViewBag.Detail = detail;

            decimal total = (decimal)detail.Sum(e=> e.UnitPrice);
            ViewBag.Total = total;
            var order = db.Order.FirstOrDefault(d => d.IDOrder == id);
            return View(order);
        }

        // GET: Admin/Order/Create
        public ActionResult Create()
        {
            ViewBag.IDCus = new SelectList(db.Customer, "ID", "NameCustomer");
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrder,Address,Status,DateOrder,IDCus")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.Customer, "ID", "NameCustomer", order.IDCus);
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCus = new SelectList(db.Customer, "ID", "NameCustomer", order.IDCus);
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrder,Address,Status,DateOrder,IDCus")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCus = new SelectList(db.Customer, "ID", "NameCustomer", order.IDCus);
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ConfirmOrder(int id)
        {
            var order = db.Order.FirstOrDefault(item => item.IDOrder == id);
            order.Status = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StatisticsByDay(DateTime date)
        {
            var orders = db.Order.Where(o => DbFunctions.TruncateTime(o.DateOrder) == date.Date).ToList();
            return View("Statistics", orders);
        }
        public ActionResult RevenueStatistics(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
              
                startDate = DateTime.Today.AddDays(-7);
                endDate = DateTime.Today;
            }

            // Filter orders based on the selected date range
            var orders = db.Order.Include(o => o.Customer).Include(o => o.OrderDetail)
                                 .Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate)
                                 .ToList();
            decimal totalRevenue = CalculateTotalRevenue(startDate.Value, endDate.Value);

            ViewBag.TotalRevenue = totalRevenue;

            return View(orders);
        }
        private decimal CalculateTotalRevenue(DateTime startDate, DateTime endDate)
        {
          
            var orders = db.Order.Include(o => o.OrderDetail)
                                 .Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate)
                                 .ToList();

            decimal totalRevenue = (decimal)orders.Sum(o => o.OrderDetail.Sum(od => od.UnitPrice));

            return totalRevenue;
        }


        public ActionResult TopSellingProducts()
        {
            var topProducts = db.OrderDetail
                                .GroupBy(od => od.ProductID)
                                .Select(group => new
                                {
                                    ProductID = group.Key,
                                    TotalQuantity = group.Sum(od => od.Quantity)
                                })
                                .OrderByDescending(result => result.TotalQuantity)
                                .Take(10)
                                .ToList();

            List<Product> products = new List<Product>();

            foreach (var item in topProducts)
            {
                Product product = db.Product.Find(item.ProductID);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return View(products);
        }


    }

}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zenwire.Domain.Commissions;
using Zenwire.Repositories;

namespace Zenwire.Controllers
{
    public class PayCodeController : Controller
    {
        private ZenwireContext db = new ZenwireContext();

        //
        // GET: /PayCode/

        public ActionResult Index()
        {
            return View(db.PayCodes.ToList());
        }

        //
        // GET: /PayCode/Details/5

        public ActionResult Details(int id = 0)
        {
            PayCode paycode = db.PayCodes.Find(id);
            if (paycode == null)
            {
                return HttpNotFound();
            }
            return View(paycode);
        }

        //
        // GET: /PayCode/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PayCode/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PayCode paycode)
        {
            if (ModelState.IsValid)
            {
                db.PayCodes.Add(paycode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paycode);
        }

        //
        // GET: /PayCode/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PayCode paycode = db.PayCodes.Find(id);
            if (paycode == null)
            {
                return HttpNotFound();
            }
            return View(paycode);
        }

        //
        // POST: /PayCode/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PayCode paycode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paycode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paycode);
        }

        //
        // GET: /PayCode/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PayCode paycode = db.PayCodes.Find(id);
            if (paycode == null)
            {
                return HttpNotFound();
            }
            return View(paycode);
        }

        //
        // POST: /PayCode/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PayCode paycode = db.PayCodes.Find(id);
            db.PayCodes.Remove(paycode);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
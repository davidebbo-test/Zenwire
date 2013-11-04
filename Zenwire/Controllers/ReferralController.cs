using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zenwire.Domain;
using Zenwire.Domain.Commissions;
using Zenwire.Models;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Zenwire.Controllers
{
    public class ReferralController : Controller
    {
        private readonly IReferralService _referralService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IProductService  _productService;

        public List<PayCode> PayCodeList; 

        public ReferralController(IReferralService referralService, IProductService productService, ICustomerService customerService, IEmployeeService employeeService)
        {
            _productService = productService;
            _customerService = customerService;
            _employeeService = employeeService;
            _referralService = referralService;

            PayCodeList = new List<PayCode>();
        }

        public ReferralController()
        {
            
        }

        // GET: /Referral/
        public ActionResult Index()
        {
            var referrals = _referralService.Get().ToList();
            return View(referrals);
        }

        // GET: /Referral/Details/5
        public ActionResult Details(int id = 0)
        {
            Referral referral = _referralService.Get(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // GET: /Referral/Create
        public ActionResult Create()
        {
            return View(new ReferralModel()
            {
                PayCodes = _employeeService.GetPayCodes(),
                Referral = new Referral()
            });
        }

        // POST: /Referral/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReferralModel model)
        {
            if (ModelState.IsValid)
            {
                _referralService.Add(model.Referral);
                return RedirectToAction("Index");
            }

            model.PayCodes = _employeeService.GetPayCodes();

            return View(model);
        }

        // GET: /Referral/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Referral referral = _referralService.Get(id);
            if (referral == null)
            {
                return HttpNotFound();
            }

            return View(referral);
        }

        // POST: /Referral/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Referral referral)
        {
            if (ModelState.IsValid)
            {
                _referralService.Update(referral);
                return RedirectToAction("Index");
            }

            return View(referral);
        }

        // GET: /Referral/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Referral referral = _referralService.Get(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // POST: /Referral/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _referralService.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult AddPayCode(ReferralModel model, string SelectedPayCode)
        {
            var payCode = _employeeService.GetPayCodeById(Convert.ToInt32(SelectedPayCode));

            model.PayCodesList = GetPayCodes();
            model.Add(payCode);
            Session["paycodes"] = model.PayCodesList;

            return PartialView("_PayCodesPartial", model);
        }

        private List<PayCode> GetPayCodes()
        {
            var payCodes = (List<PayCode>)Session["paycodes"];
            
            if (payCodes == null)
            {
                payCodes = new List<PayCode>();
                Session["paycodes"] = payCodes;
            }

            return payCodes;
        }
    }
}
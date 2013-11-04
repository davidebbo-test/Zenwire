using System.Web.Mvc;
using Zenwire.Domain;
using Zenwire.Services;

namespace Zenwire.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: /Customer/
        public ActionResult Index()
        {
            return View(_customerService.Get());
        }

        // GET: /Customer/Details/5
        public ActionResult Details(int id = 0)
        {
            var customer = _customerService.Get(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerService.Add(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Customer/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Customer customer = _customerService.Get(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                //_customerRepository.Update(customer);
                _customerService.Update(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Customer/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var customer = _customerService.Get(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _customerService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
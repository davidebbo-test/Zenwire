using System;
using System.Web.Mvc;
using Zenwire.Domain;
using Zenwire.Models;
using Zenwire.Repositories;

namespace Zenwire.Controllers
{
    public class ShiftController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Shift> _shiftRepository;

        public ShiftController(IRepository<Employee> employeeRepository, IRepository<Shift> shiftRepository)
        {
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
        }

        // GET: /Shift/
        public ActionResult Index()
        {
            return View(_shiftRepository.Get);
        }

        // GET: /Shift/Details/5
        public ActionResult Details(int id = 0)
        {
            Shift shift = _shiftRepository.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: /Shift/Create
        public ActionResult Create()
        {
            return View(new ShiftModel()
            {
                EmployeeList = _employeeRepository.Get,
                ShiftEntity = new Shift()
            });
        }

        // POST: /Shift/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShiftModel model, DateTime? date)
        {
            if (ModelState.IsValid)
            {
                _shiftRepository.Add(model.ShiftEntity);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Shift/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Shift shift = _shiftRepository.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }

            return View(new ShiftModel()
            {
                EmployeeList = _employeeRepository.Get,
                ShiftEntity = shift
            });
        }

        // POST: /Shift/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShiftModel shift)
        {
            if (ModelState.IsValid)
            {
                _shiftRepository.Update(shift.ShiftEntity);
                return RedirectToAction("Index");
            }
            return View(shift);
        }

        // GET: /Shift/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Shift shift = _shiftRepository.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: /Shift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shift shift = _shiftRepository.Find(id);
            _shiftRepository.Remove(shift);
            return RedirectToAction("Index");
        }
    }
}
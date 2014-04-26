using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VacationManager.Models;
using WebMatrix.WebData;

namespace VacationManager.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private MainContextDb db = new MainContextDb();

        //
        // GET: /Vacation/

        public ActionResult Index()
        {
            var vacations = db.Vacations.Include(v => v.UserProfile);
            return View(vacations.ToList());
        }

        //
        // GET: /Vacation/Details/5

        public ActionResult Details(int id = 0)
        {
            Vacation vacation = db.Vacations.Find(id);
            if (vacation == null)
            {
                return HttpNotFound();
            }
            return View(vacation);
        }

        //
        // GET: /Vacation/Create

        public ActionResult Create()
        {
            ViewBag.UserId = WebSecurity.CurrentUserId;
            ViewBag.IsApproved = false;
            return View();
        }

        //
        // POST: /Vacation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vacation vacation)
        {
            DateTime vacationDate = vacation.VacationDate;
            int userId = vacation.UserId;
            int vacationCount = db.Vacations.Where(c => c.UserId == userId && c.VacationDate.Year == vacationDate.Year).Count();
            String errorMsg = "More than 10 days is not allowed for the year " + vacationDate.Year;
            try
            {
                
                if (vacationCount >= 10)
                {
                    throw new ValidationException("More than 10 days is not allowed for the year " + vacationDate.Year);
                }
            }
            catch (ValidationException e)
            {
                errorMsg = e.Message;
            }
 
            if (ModelState.IsValid && vacationCount < 10)
            {
                db.Vacations.Add(vacation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", vacation.UserId);
            ViewBag.ErrorMsg = errorMsg;
            return View(vacation);
        }

        //
        // GET: /Vacation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vacation vacation = db.Vacations.Find(id);
            if (vacation == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = vacation.UserId;
            ViewBag.IsApproved = vacation.IsApproved;
            return View(vacation);
        }

        //
        // POST: /Vacation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vacation vacation)
        {
            if (ModelState.IsValid && (vacation.UserId == WebSecurity.CurrentUserId || User.IsInRole("Manager")))
            {
                db.Entry(vacation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMsg = "";
            if (vacation.UserId != WebSecurity.CurrentUserId || !User.IsInRole("Manager"))
            {
                ViewBag.ErrorMsg = "You may not modify someone else's vacation request";
            }
            return View(vacation);
        }

        //
        // GET: /Vacation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vacation vacation = db.Vacations.Find(id);
            if (vacation == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = WebSecurity.CurrentUserId;

            ViewBag.ErrorMsg = "You may not delete someone else's vacation request";
            
            return View(vacation);
        }

        //
        // POST: /Vacation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Vacation vacation = db.Vacations.Find(id);
            db.Vacations.Remove(vacation);
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
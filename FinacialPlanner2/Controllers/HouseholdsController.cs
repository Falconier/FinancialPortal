using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinacialPlanner2.Models;
using FinacialPlanner2.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace FinacialPlanner2.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        public ActionResult Join()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.EmailConfirmed)
            { 
                return View();
            }
            else
            {
                RedirectToAction("PlsConfirmEmail");
            }
        }

        #region Join Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(Invite invite)
        {
            if(ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                if(string.IsNullOrWhiteSpace(invite.Email))
                {
                    invite.Email = user.Email;
                }
                var dbInvite = db.Invites.FirstOrDefault(i => !i.HasBeenUsed && i.Email == invite.Email && i.HHToken == invite.HHToken); 

                if(dbInvite != null)
                {
                    dbInvite.HasBeenUsed = true;
                    user.HouseHoldId = dbInvite.HouseholdId;
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
                ModelState.AddModelError(string.Empty, "Invalid Invite Code. Please speak to the sender of the invite.");
                return View(invite);
            }

            return View(invite);
        }

        #endregion

        public ActionResult PlsConfirmEmail()
        {
            return View();
        }

        public ActionResult CreateInvite()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInvite([Bind(Include = "Id,Name")] Household household)
        {
            InviteToken tok = new InviteToken();
            string hht = tok.GenerateHHToken();
            Invite nvt = new Invite()
            {
                HasBeenUsed = false,
                HHToken = new Guid(hht),
                InviteDate = DateTimeOffset.Now,
                InvitedById = User.Identity.GetUserId(),
                //hiddenFor for the HouseHoldId
            };

            return View();
        }

        #region Editing
        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        #endregion

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
    }
}

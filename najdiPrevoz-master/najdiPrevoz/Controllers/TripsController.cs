using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using najdiPrevoz.Models;

namespace najdiPrevoz.Controllers
{
    public class TripsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Trips
        public ActionResult Index()
        {
            List<Trip> trips = db.Trips.ToList();
            foreach (Trip trip in trips)
            {
                trip.CanEdit = trip.Creator == User.Identity.Name;
            }
            return View(trips);
        }

        public ActionResult MyTrips()
        {
            List<Trip> trips = new List<Trip>();
            foreach (Trip trip in db.Trips.ToList())
            {
                if (trip.Creator == User.Identity.Name)
                {
                    trips.Add(trip);
                }
            }
            return View(trips);
        }

        // GET: Trips/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = GetTrip((long)id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        [Authorize]

        // GET: Trips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FromDestination,ToDestination,Time,Capacity,Description,Price")] Trip trip)
        { 
            
            if (ModelState.IsValid)
            {
                trip.Creator = User.Identity.Name;
                trip.FreeSeats = trip.Capacity;
                db.Trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trip);
        }

        // GET: Trips/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FromDestination,ToDestination,Time,Capacity,Description,Price")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trip);
        }

        // GET: Trips/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
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

        public ActionResult Reserve(long? id)
        {
            if (id != null)
            {
                return View(GetTrip((long)id));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Reserve")]
        public ActionResult MakeReservation(long? id)
        {
            if (id != null)
            {
                Trip trip = GetTrip((long)id);
                int noSeats = int.Parse(Request["NoSeats"]);

                if (noSeats <= trip.FreeSeats)
                {
                    // make reservation
                    Reservation reservation = new Reservation();
                    reservation.Date = DateTime.Now;
                    reservation.NoSeats = noSeats;
                    reservation.Traveler = User.Identity.Name;
                    reservation.TripId = (long)id;
                    reservation.Status = "WAITING";
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return View("SuccessfullReservation");
                }
                else
                {
                    // return error message: Not enough seats available
                    return View("FailedReservation");
                }
            }
            return View("Create");
        }

        public Trip GetTrip(long id)
        {
            Trip trip = db.Trips.Find(id);
            int totalTakenSeats = 0;

            // TODO: set trip canEdit property

            if(trip.Creator == User.Identity.Name)
            {
                trip.CanEdit = true;
            }
            else
            {
                trip.CanEdit = false;
            }


            foreach (Reservation reservation in db.Reservations)
            {
                if (trip.Id == reservation.TripId)
                {
                    if (reservation.Status.Equals("WAITING") || reservation.Status.Equals("APPROVED"))
                    {
                        {
                            totalTakenSeats += reservation.NoSeats;
                        }
                    }
                }
            }
                trip.FreeSeats = trip.Capacity - totalTakenSeats;
                return trip;
        }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace najdiPrevoz.Models
{
    public class Reservation
    {
        [Key] public long Id { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string Traveler { get; set; }

        public long TripId { get; set; }

        public int NoSeats { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace najdiPrevoz.Models
{
    public class Trip
    {
        [Key]
        public long Id { get; set; }

        public string Creator { get; set; }

        [Required]
        [Display(Name = "From Destination")]
        public string FromDestination { get; set; }

        [Required]
        [Display(Name = "To Destination")]
        public string ToDestination { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Time")]
        public int TimeHour { get; set; }

        [Required]
        public int TimeMinutes { get; set; }


        [Required]
        [Display(Name = "Number seats")]
        public int Capacity { get; set; }

        public int FreeSeats { get; set; }


        [Display(Name = "Add description for your trip")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Dodadi opis na vozilo, marka + godina na proizvodstvo")]
        public string AutoDescr { get; set; }

        [Display(Name = "Prikachete slika od voziloto")]
        public string Img { get; set; }


        [NotMapped]
        public bool CanEdit { get; set; }

        [Display(Name = "Price per traveler")]
        public int Price { get; set; }

    }
}
    public enum Gradovi
    {
        Skopje, Bitola, Prilep, Shtip, Gostivar, Kumanovo, Veles, Ohrid, Struga, Tetovo, Kichevo, Debar, Gevgelija
    }

    

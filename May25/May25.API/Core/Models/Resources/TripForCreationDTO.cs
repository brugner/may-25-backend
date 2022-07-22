using System;
using System.ComponentModel.DataAnnotations;

namespace May25.API.Core.Models.Resources
{
    public class TripForCreationDTO
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public PlaceForCreationDTO Origin { get; set; }

        [Required]
        public PlaceForCreationDTO Destination { get; set; }

        [Required]
        public DateTimeOffset Departure { get; set; }

        [Required]
        public byte MaxPassengers { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Distance { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int SuggestedCost { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public int CostPerPassenger { get; set; }
    }
}

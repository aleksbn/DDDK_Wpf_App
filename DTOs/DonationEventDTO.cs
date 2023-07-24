using System;
using System.ComponentModel.DataAnnotations;

namespace DDDK_Wpf.DTOs
{
    public class CreateDonationEventDTO
    {
        [Required]
        public DateTime eventDate { get; set; }
        [Required]
        public int locationId { get; set; }
        public string description { get; set; }
    }

    public class DonationEventDTO : CreateDonationEventDTO
    {
        public int id { get; set; }
        public string? locationName { get; set; }

        public override string ToString()
        {
            return locationName + " - " + eventDate.ToString("dd.MM.yyyy");
        }
    }

    public class UpdateDonationEventDTO : DonationEventDTO
    {

    }
}

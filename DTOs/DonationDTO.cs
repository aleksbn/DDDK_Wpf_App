using System.ComponentModel.DataAnnotations;

namespace DDDK_Wpf.DTOs
{
    public class CreateDonationDTO
    {
        [Required]
        public int donatorId { get; set; }
        [Required]
        public int donationEventId { get; set; }
    }

    public class DonationDTO: CreateDonationDTO
    {
        [Required]
        public int id { get; set; }
        public string? donatorFullName { get; set; }

        public override string ToString()
        {
            return donatorFullName;
        }
    }

    public class UpdateDonationDTO: DonationDTO
    {

    }

    public class DonationSearchDTO
    {
        public string donationDate { get; set; }
        public string location { get; set; }
    }
}

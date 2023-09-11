using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDDK_Wpf.DTOs
{
    public class CreateLocationDTO
    {
        [Required]
        public string name { get; set; }
        public string description { get; set; }
    }

    public class LocationDTO : CreateLocationDTO
    {
        public int id { get; set; }
        public virtual IList<DonationEventDTO> donationEvents { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class UpdateLocationDTO : CreateLocationDTO
    {

    }
}

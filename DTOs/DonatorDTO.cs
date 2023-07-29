using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDDK_Wpf.DTOs
{
    public class CreateDonatorDTO
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public DateTime birthDate { get; set; }
        [Required]
        public string address { get; set; }
        public string phoneNumber { get; set; }
        [Required]
        public int bloodTypeId { get; set; }
        public int previousDonations { get; set; } = 0;

        public IList<DonationDTO> donations { get; set; }

        public override string ToString()
        {
            return firstName + " " + lastName;
        }
    }

    public class DonatorDTO : CreateDonatorDTO
    {
        public int id { get; set; }
    }

    public class UpdateDonatorDTO : CreateDonatorDTO
    {

    }

    public class DonatorSearchDTO
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string bloodType { get; set; }
        public string lastDonationDate { get; set; }
        public int totalDonations { get; set; }
        public string fullName { get; set; }

        public override string ToString()
        {
            return id.ToString() + " - " + firstName + " " + lastName;
        }
    }

    public class DonatorFromEventDTO
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public bool didDonate { get; set; }
    }
}

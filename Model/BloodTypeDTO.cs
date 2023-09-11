namespace DDDK_Wpf.DTOs
{
    public class BloodTypeDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phFactor { get; set; }

        public override string ToString()
        {
            return name + phFactor;
        }
    }
}

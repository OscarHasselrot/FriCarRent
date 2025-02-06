using System.ComponentModel.DataAnnotations;

namespace FriCarRent.ViewModels
{
    public class BookingsViewModel
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public float TotalPrice { get; set; }
    }
}

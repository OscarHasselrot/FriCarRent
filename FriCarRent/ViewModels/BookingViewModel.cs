using System.ComponentModel.DataAnnotations;

namespace FriCarRent.ViewModels
{
    public class BookingViewModel
    {
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public float TotalPrice { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ATMSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string AccountNumber { get; set; } = "";

        [Required]
        public string Pin { get; set; } = "";

        public decimal Balance { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
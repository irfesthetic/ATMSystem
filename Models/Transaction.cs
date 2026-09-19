namespace ATMSystem.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int CustomerId { get; set; }

        public string TransactionType { get; set; } = "";

        public decimal Amount { get; set; }

        public decimal PreviousBalance { get; set; }

        public decimal NewBalance { get; set; }

        public string Description { get; set; } = "";

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Success";
    }
}
namespace Service.Model
{
    public class Invoice
    {
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string DateText { get; set; }
        public Expense ExpenseDetail { get; set; }
    }
}

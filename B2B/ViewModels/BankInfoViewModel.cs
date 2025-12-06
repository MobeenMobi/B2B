namespace B2B.ViewModels
{
    public class BankInfoViewModel
    {
        public string Bank { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public decimal AverageAnnualTrunover { get; set; }

        // New property: multiple remittance details
        public List<RemittanceDetailViewModel> RemittanceDetails { get; set; } = new List<RemittanceDetailViewModel>();
    }
}

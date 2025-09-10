namespace InvoiceSystem.ErrorMessages
{
    public static class SubscriptionErrors
    {
        public const string CustomerIdRequired = "Customer ID must be greater than 0.";
        public const string PlanIdRequired = "Plan ID must be greater than 0.";
        public const string StartDateRequired = "Start date is required.";
        public const string IsActiveRequired = "Subscription must have an active status.";
    }
}

namespace InvoiceSystem.ErrorMessages
{
    public class AllErrors
    {
        // Customer Errors
        public const string CustomerNameRequired = "Customer name is required.";
        public const string CustomerEmailRequired = "Customer email is required.";
        public const string CustomerEmailInvalid = "Customer email is invalid.";
        public const string CustomerNotFound = "Customer does not exist.";

        // Plan Errors
        public const string PlanDuplicateName = "A plan with this name already exists.";
        public const string PlanNotFound = "Plan does not exist.";
        public const string MaxUsersExceeded = "Max users must be greater than 0.";
        public const string InternalServerError = "An error occurred while processing the request.";

        // Subscription Errors (example future usage)
        public const string SubscriptionNotFound = "Subscription does not exist.";
        public const string SubscriptionAlreadyActive = "Subscription is already active.";
    }
}

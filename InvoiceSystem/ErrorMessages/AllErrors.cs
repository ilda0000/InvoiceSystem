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
        public const string PlanNotFound = "Plan does not exist.";
        public const string MaxUsersExceeded = "Max users must be greater than 0.";
        public const string InternalServerError = "An error occurred while processing the request.";

        // Subscription Errors 
        public const string SubscriptionNotFound = "Subscription does not exist.";
        public const string SubscriptionAlreadyActive = "Customer already has an active subscription for this plan.";
        public const string SubscriptionCreationFailed = "Subscription could not be created.";
        public const string SubscriptionPlanCapacityExceeded = "Plan capacity exceeded. Max users limit reached.";

        //Payment Errors
        public const string PaymentMethodInactive = "Selected payment method is not active.";
        public const string PaymentAmountInvalid = "Payment amount must match the invoice total amount.";
        public const string PaymentMethodRequired = "Payment method is required.";
        public const string PaymentAmountRequired = "Payment amount is required and must be greater than zero.";
        public const string PaymentInvoiceNotFound = "Invoice for payment not found.";
        public const string PaymentInvoiceAlreadyPaid = "Invoice is already paid.";
        public const string PaymentDateInvalid = "Payment date cannot be in the future.";


        public const string InvoiceNotFound = "Invoice does not exist.";
        public const string InvoiceAlreadyPaid = "Invoice is already paid.";
        public const string InvoiceAmountInvalid = "Invoice amount must be greater than zero.";
        public const string InvoiceCustomerMismatch = "Invoice does not belong to the specified customer.";
        public const string InvoiceDueDatePassed = "Invoice due date has already passed.";
        public const string InvoiceCreationFailed = "Invoice could not be created.";

    }
}

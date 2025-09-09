namespace InvoiceSystem.ErrorMessages
{
    public static class PlanError
    {
        public const string PlanDuplicateName = "A plan with this name already exists.";
        public const string PlanNotFound = "Plan does not exist.";
        public const string MaxUsersExceeded = "Max users must be greater than 0.";
        public const string InternalServerError = "An error occurred while processing the request.";
    }
}

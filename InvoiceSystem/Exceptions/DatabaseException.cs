﻿namespace InvoiceSystem.Exceptions
{
    public class DatabaseException :Exception
    {
        public DatabaseException(string message, Exception inner) : base(message, inner)
        {   
        }
    }
}

using System.Collections.Generic;
using System.Configuration;

namespace MoneyMinderWebAPI.Helpers
{
    public static class Constants
    {
        public static class PaymentFrequencies
        {
            public const int Daily = 1;
            public const int Weekly = 2;
            public const int Fortnightly = 3;
            public const int Monthly = 4;
            public const int Never = 5;

            public static Dictionary<string, int> FrequencyDictionary = new Dictionary<string, int>
            {
                {"Daily", Daily},
                {"Weekly", Weekly},
                {"Fortnightly", Fortnightly},
                {"Monthly", Monthly},
                {"Never", Never}
            };
        }

        public static class TransactionTypes
        {
            public const int AutomaticPayment = 1;
            public const int DD = 2;
        }
    }
}

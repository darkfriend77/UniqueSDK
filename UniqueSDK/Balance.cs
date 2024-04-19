using System.Numerics;

namespace UniqueSDK
{
    public class Balance
    {
        public Balance(BigInteger rawBalance, uint decimals, string unit)
        {
            Raw = rawBalance;
            Amount = (decimal)rawBalance / (decimal)Math.Pow(10, decimals);
            Formatted = ""; // Add formatted
            Decimals = decimals;
            Unit = unit;
        }

        public BigInteger Raw { get; set; }
        public decimal Amount { get; set; }
        public string Formatted { get; set; }
        public uint Decimals { get; set; }
        public string Unit { get; set; }
    }
}


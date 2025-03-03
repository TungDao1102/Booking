namespace Booking.Domain.Commons
{
    public sealed record Currency
    {
        public static readonly Currency Usd = new("USD");
        public static readonly Currency Eur = new("EUR");
        internal static readonly Currency None = new("");

        private Currency(string code) => Code = code;
        public string Code { get; init; }

        public static Currency FromCode(string code)
        {
            return All.FirstOrDefault(c => c.Code == code) ??
                throw new ApplicationException($"Currency code {code} is not supported.");
        }

        public static readonly IReadOnlyCollection<Currency> All = [Usd, Eur];
    }
}

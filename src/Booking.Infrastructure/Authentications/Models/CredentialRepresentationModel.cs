namespace Booking.Infrastructure.Authentications.Models
{
    internal class CredentialRepresentationModel
    {
        public string Algorithm { get; set; } = default!;

        public string Config { get; set; } = default!;

        public int Counter { get; set; }

        public long CreatedDate { get; set; }

        public string Device { get; set; } = default!;

        public int Digits { get; set; }

        public int HashIterations { get; set; }

        public string HashedSaltedValue { get; set; } = default!;

        public int Period { get; set; }

        public string Salt { get; set; } = default!;

        public bool Temporary { get; set; }

        public string Type { get; set; } = default!;

        public string Value { get; set; } = default!;
    }
}

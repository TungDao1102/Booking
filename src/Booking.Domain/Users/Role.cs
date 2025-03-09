﻿namespace Booking.Domain.Users
{
    public sealed class Role
    {
        public static readonly Role Registered = new(1, "Registered");

        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public ICollection<User> Users { get; init; } = [];

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

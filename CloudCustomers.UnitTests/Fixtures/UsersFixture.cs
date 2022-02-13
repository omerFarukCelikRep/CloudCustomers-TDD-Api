using CloudCustomers.API.Models;
using System.Collections.Generic;

namespace CloudCustomers.UnitTests.Fixtures;

public static class UsersFixture
{
    public static List<User> GetTestUsers() => new()
    {
        new User
        {
            Name = "Test User 1",
            Email = "testuser1@example.com",
            Address = new Address
            {
                Street = "123 Main St",
                City = "Madison",
                ZipCode = "53704"
            }
        },
        new User
        {
            Name = "Test User 2",
            Email = "testuser2@example.com",
            Address = new Address
            {
                Street = "123 Main St",
                City = "Madison",
                ZipCode = "53704"
            }
        },
        new User
        {
            Name = "Test User 3",
            Email = "testuser3@example.com",
            Address = new Address
            {
                Street = "123 Main St",
                City = "Madison",
                ZipCode = "53704"
            }
        }
    };
}

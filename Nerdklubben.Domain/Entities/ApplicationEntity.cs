using System;
using System.Collections.Generic;
using System.Text;

namespace Nerdklubben.Domain.Entities
{
    public class ApplicationEntity
    {

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        public string? Role { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";

    }
}

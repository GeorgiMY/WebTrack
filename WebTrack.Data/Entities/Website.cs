using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebTrack.Data.Entities
{
    public class Website
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(300)]
        public string BaseUrl { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [Required]
        public string OwnerUserId { get; set; } = null!;
        public User OwnerUser { get; set; } = null!;

        public ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();
    }
}

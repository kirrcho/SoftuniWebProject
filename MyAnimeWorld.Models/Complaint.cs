using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime DateCreatedAt { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(350)]
        public string Message { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Final.Models
{
    public class Borrower : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [MaxLength(20)]
        public string LibraryCardNumber { get; set; }
        public List<History>? histories { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        public byte Level { get; set; }

        [ForeignKey("Person")]
        public long PersonId { get; set; }

        public Person Person { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


}

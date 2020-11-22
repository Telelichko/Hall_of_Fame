using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Models
{
    public class Person
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "DisplayName can't exceed 100 characters")]
        public string DisplayName { get; set; }

        public ICollection<Skill> Skills { get; set; }

        public Person()
        {
            Skills = new List<Skill>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

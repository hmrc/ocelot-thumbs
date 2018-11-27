using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThumbsApi.Models
{
    public class Thumb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MinLength(7)]
        [MaxLength(7)]
        public string Pid { get; set; }  

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public bool Rating { get; set; }        

        [Required]
        [MaxLength(10)]
        public string Product { get; set; }  

        [Required]
        [MaxLength(100)]
        public string Group { get; set; }  //process

        [Required]
        [MaxLength(255)]
        public string EndPoint { get; set; }  //endpoint
    }
}

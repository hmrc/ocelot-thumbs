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

        [MaxLength(7)]
        public string Pid { get; set; }  //pid

        public DateTime Date { get; set; } = DateTime.Now;

        public bool Rating { get; set; }        

        public string Product { get; set; }  

        [MaxLength(255)]
        public string Group { get; set; }  //process

        public string EndPoint { get; set; }  //last15 or endpoint
    }
}

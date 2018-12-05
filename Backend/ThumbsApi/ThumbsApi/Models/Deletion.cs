using System;
using System.ComponentModel.DataAnnotations;

namespace ThumbsApi.Models
{
    public class Deletion : ThumbBase
    {
        /// <summary>
        /// Pid of staff who deleted thumb
        /// </summary>
        [MinLength(7)]
        [MaxLength(7)]
        [Required]
        public string DeletedBy { get; set; }

        /// <summary>
        /// Time Deleted
        /// </summary>
        [Required]
        public DateTime DeletedTime { get; private set; } = DateTime.Now;

        public Deletion()
        {

        }

        public Deletion(ThumbBase thumb)
        {
            this.Date = thumb.Date;
            this.EndPoint = thumb.EndPoint;
            this.Group = thumb.Group;
            this.Id = thumb.Id;
            this.Pid = thumb.Pid;
            this.Product = thumb.Product;
            this.Rating = thumb.Rating;
        }


    }
}

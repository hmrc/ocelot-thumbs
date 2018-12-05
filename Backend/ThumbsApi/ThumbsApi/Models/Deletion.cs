using System;
using System.ComponentModel.DataAnnotations;

namespace ThumbsApi.Models
{
    public class Deletion : Thumb
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

    }
}

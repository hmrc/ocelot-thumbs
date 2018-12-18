using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThumbsApi.Models
{
    public class ProductGroup
    {
        [Key]
        public Guid Id { get; set; }

        public string ProductReference { get; set; }

        public string ProductOwner { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        [JsonIgnore]
        public virtual ProductGroup Parent { get; set; }

        public virtual ICollection<ProductGroup> Children { get; set; }
    }
}
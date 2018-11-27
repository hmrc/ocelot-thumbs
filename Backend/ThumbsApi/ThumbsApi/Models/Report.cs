using System;
using System.Collections.Generic;
using System.Linq;

namespace ThumbsApi.Models
{
    public class Report
    {
        public string Product { get; private set; }

        public long Count
        {
            get
            {
                return Up + Down;
            }
            
        }

        private long? up;
        public long Up
        {
            get
            {
                if (up == null)
                {
                    up = myUp + Children.Sum(c => c.Up);
                }
                return up.Value;
            }
        }

        private long? down;
        public long Down
        {
            get
            {
                if (down == null)
                {
                    down = myDown + Children.Sum(c => c.Down);
                }
                return down.Value;
            }
        }

        public decimal Percentage
        {
            get
            {
                return Up / Count;
            }
        }

        public ICollection<Report> Children { get; set; } = new List<Report>();

        private readonly long myUp;
        private readonly long myDown;

        public Report(string product, long up, long down)
        {
            this.Product = product;
            this.myUp = up;
            this.myDown = down;
        }
    }
}

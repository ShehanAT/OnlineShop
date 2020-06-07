using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ToDoItem
    {
        public long id { get; set; }
        public string name { get; set; }
        public bool IsComplete { get; set; }

    }
}

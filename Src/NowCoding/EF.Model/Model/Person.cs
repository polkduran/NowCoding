using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Model.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}

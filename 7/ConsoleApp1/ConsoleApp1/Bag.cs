using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Bag
    {
        public Dictionary<Bag, int> contains;
        public List<Bag> containedin;
        public bool mark = false;
        public string name;

        public Bag(string n)
        {
            name = n;
            contains = new Dictionary<Bag, int>();
            containedin = new List<Bag>(); 
        }

        public void addContains(Bag b, int n)
        {
            if (!contains.ContainsKey(b))
            {
                contains.Add(b, n);
                b.containedin.Add(this);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class SPL
    {
        private String name;
        private String location;
        private int spl;
        private int id;

        public SPL (String Name, String Location, int SPL, int ID)
        {
            name = Name;
            location = Location;
            spl = SPL;
            id = ID;
        }

        public String Name { get { return name; } }
        public String Location { get { return location; } }
        public int SPLValue { get { return spl; } }
        public int ID { get { return id; } }
    }
}
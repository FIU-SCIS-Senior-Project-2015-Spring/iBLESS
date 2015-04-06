
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebApplication1
{
    public class Vibrations
    {
        public enum VibrationTypes { Low = 1, Medium = 2, FastAlert = 3 }
        public static String[] VibrationSettings = { "Low;1,1,1,1;6", "Medium;2,2,2,2;8", "Fast Alert;16,138,16,138,15,138,15,138;7" };

        public static List<Vibration> VibrationList
        {    
            get
            {
                String[] vibNames = Enum.GetNames(typeof(VibrationTypes));
                List<Vibration> vibrations = new List<Vibration>();

                foreach(var value in vibNames.Zip(VibrationSettings, (x, y) => new {Name = x, Setting = y}))
                {
                    vibrations.Add(new Vibration(value.Name, value.Setting));
                }

                return vibrations;
            }
        }

        public class Vibration
        {
            private String name;
            private String setting;
            private double low;
            private double high;
            private int id;

            public Vibration (String Name, String Setting)
            {
                this.name = Name;
                this.setting = Setting;
            }

            public Vibration (String Name, double Low, double High, int ID)
            {
                low = Low;
                high = High;
                name = Name;
                id = ID;
            }

            public String ToString ()
            {
                return "Low: " + low + " High: " + high;
            }

            public String Name { get { return name; } set { name = value; } }
            public String Setting { get { return setting; } set { setting = value; }}
            public double Low { get { return low; } set { low = value; } }
            public double High { get { return high; } set { high = value; } }
            public int ID { get { return id; } set { id = value; } }
        }
    }
}
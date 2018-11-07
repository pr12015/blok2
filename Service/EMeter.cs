using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class EMeter
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public double Reading { get; set; }

        public EMeter() { }

        public EMeter(int id, string fullName, double reading)
        {
            ID = id;
            FullName = fullName;
            Reading = reading;
        }

        public override string ToString()
        {
            return ID.ToString() + " " + FullName + " " + Reading.ToString();
        }

        public static EMeter FromString(string querryResult)
        {
            var arg = querryResult.Split(' ');
            return new EMeter(Int32.Parse(arg[0]), arg[1], Double.Parse(arg[2]));
        }
    }
}
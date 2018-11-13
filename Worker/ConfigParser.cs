using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Worker
{
    class ConfigParser
    {
        private static readonly string _path = @"C:\Users\stefan\Desktop\blok2\Blok2Project\Worker\config.xml";

        public static double GetPrice(double value)
        {
            var doc = new XmlDocument();
            double price;

            using(var reader = XmlReader.Create(_path))
            {
                doc.PreserveWhitespace = true;
                doc.Load(reader);
                
                /// Get 'zone' nodes.
                var nodes = doc.GetElementsByTagName("zone");
                foreach (XmlNode node in nodes)
                {
                    /// example node: 
                    /// <zone price="12.1">
                    ///     <min>0</min> 
                    ///     <max>350</min> 
                    /// </zone>
                    /// <max></max> can be ommited. Default max is 100000.
                    price = double.Parse(node.Attributes["price"].Value);

                    var children = node.ChildNodes;
                    int min = 0, max = 100000;

                    foreach (XmlNode child in children)
                    {
                        if (child.Name == "min")
                        {
                            min = int.Parse(child.InnerText);
                        }
                        if (child.Name == "max")
                        {
                            max = int.Parse(child.InnerText);
                        }
                    }

                    if (value > min && value < max)
                    {
                        return price;
                    }
                }
                throw new Exception("value out of bounds");
            }
        }
    }
}

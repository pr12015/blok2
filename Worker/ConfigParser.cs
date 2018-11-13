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
        private static readonly string _path = @"C:\Users\stefan\Desktop\blok2\Blok2Project\Service\rbac_config.xml";
        private static readonly string[]  zones = { "green", "blue", "red" };
        /*
        public static double GetPrice(double value)
        {
            using (var reader = XmlReader.Create(path))
            {
                double price;
                while (reader.Read())
                {
                    if(reader.NodeType == XmlNodeType.Element && zones.Contains(reader.Name))
                    {
                        reader.ReadAttributeValue()
                    }
                }
            }
        }
        */

        public static double GetPrice(double value)
        {
            var doc = new XmlDocument();

            using(var reader = XmlReader.Create(_path))
            {
                doc.PreserveWhitespace = true;
                doc.Load(reader);
                var nodes = doc.GetElementsByTagName("zone");
                //nodes[1].
                foreach(XmlNode node in nodes)
                {
                    var min = node.Attributes["min"].Value;
                    var max = node.Attributes["max"].Value;
                    //if(value)
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RBAC_Authorization
{
    static class RBACConfigParser
    {
        private static readonly string path = @"C:\Users\stefan\Desktop\blok2\Blok2Project\Service\rbac_config.xml";

        public static List<string> GetPermissions(string group)
        {
            var list = new List<string>();
            using (var reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == group)
                        break;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "permission")
                            list.Add(reader.ReadElementContentAsString());
                        else
                            break;
                    }
                }
            }

            return list;
        }
    }
}

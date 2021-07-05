using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace UnivIntel.Common
{
    public static class ConfigReader
    {
        public static Hashtable ReadConfig(string path)
        {
            Hashtable _ret = new Hashtable();
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader
                (
                    new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read)
                );
                XmlDocument doc = new XmlDocument();
                string xmlIn = reader.ReadToEnd();
                reader.Close();
                doc.LoadXml(xmlIn);
                foreach (XmlNode child in doc.ChildNodes)
                    if (child.Name.Equals("Settings"))
                        foreach (XmlNode node in child.ChildNodes)
                            if (node.Name.Equals("add"))
                                _ret.Add
                                (
                                    node.Attributes["key"].Value,
                                    node.Attributes["value"].Value
                                );
            }
            return (_ret);
        }

        public static string Get(string key, Hashtable systemConfig)
        {
            if (systemConfig.ContainsKey(key))
            {
                return systemConfig[key].ToString();
            }

            return null;
        }

        public static string Get(this Hashtable systemConfig, string key)
        {
            if (systemConfig.ContainsKey(key))
            {
                return systemConfig[key].ToString();
            }

            return null;
        }
    }

}

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    static class XmlFileIO
    {
        public static object xmlLoad(Type t,string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);
            object obj;

            using (FileStream fs = new FileStream(filename, FileMode.Open)) {
                using (StreamReader reader = new System.IO.StreamReader(fs))
                {
                    obj = serializer.Deserialize(reader);
                }
            }

            return obj;
        }

        public static void xmlSave(Type t,string filename,object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    serializer.Serialize(sw, obj);
                }
            }
        }
    }
}

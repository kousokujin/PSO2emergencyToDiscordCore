using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    static class XmlFileIO
    {
        static object xmlLoad(Type t,string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);
            System.IO.StreamReader sr = new System.IO.StreamReader(new FileStream(filename,FileMode.Open));
            Object obj = serializer.Deserialize(sr);

            return obj;
        }

        static void xmlSave(Type t,string filename,object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(new FileStream(filename,FileMode.Open));
            serializer.Serialize(sw, obj);
            //sw.close();
        }
    }
}

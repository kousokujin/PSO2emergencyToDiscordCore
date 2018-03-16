using System;
using System.IO;
//using System.Collections.Generic;
//using System.Text;

namespace PSO2emergencyToDiscordCore
{
    static class XmlFileIO
    {
        public static bool xmlLoad(Type t,string filename,out object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);

            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    using (StreamReader reader = new System.IO.StreamReader(fs))
                    {
                        try
                        {
                            obj = serializer.Deserialize(reader);
                        }
                        catch (System.InvalidOperationException)
                        {
                            obj = null;
                            logOutput.writeLog("設定ファイルの読み込みに失敗しました。");
                            return false;
                        }
                    }
                }
            }
            catch (IOException)
            {
                logOutput.writeLog("設定ファイルの読み込みに失敗しました。");
                obj = null;
                return false;
            }
            catch (System.Security.SecurityException)
            {
                logOutput.writeLog("設定ファイルへのアクセス権がありません。");
                obj = null;
                return false;
            }

            return true;
        }

        public static bool xmlSave(Type t,string filename,object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(t);

            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        serializer.Serialize(sw, obj);
                    }
                }
            }
            catch (IOException)
            {
                logOutput.writeLog("設定ファイルの書き込みに失敗しました。");
                return false;
            }
            catch (System.Security.SecurityException)
            {
                logOutput.writeLog("設定ファイルへのアクセス権がありません。");
                return false;
            }

            return true;
        }
    }
}

﻿//using System;
//using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;

namespace PSO2emergencyToDiscordCore
{
    class ControllerCore:IConfigFile
    {
        protected HttpClient hc;
        protected AbstractEventGetter emgGetter;
        protected AbstractChanpionGetter chGetter;
        protected EventAdmin admin;
        protected botController bot;
        protected AbstractService service;

        //string discordurl;
        string HttpGetUrl;
        string chanpionUrl;

        private string configFile;
        protected string chpFile;

        public ControllerCore()
        {
            chpFile = "";
            configure conf = configLoad();
            init(conf);
        }

        private configure configLoad(string filename = "config.xml")
        {
            this.configFile = filename;

            if(File.Exists(filename) == true)
            {
                logOutput.writeLog("設定ファイルが見つかりました。");

                object obj;
                bool result = loadConfig(out obj);

                if(result == true && obj is configure)
                {
                    configure conf = (configure)obj;
                    return conf;

                }
                else
                {
                    logOutput.writeLog("設定ファイルが不正です。初期設定を開始します。");
                    configure conf = setup();
                    return conf;
                }
            }
            else
            {
                logOutput.writeLog("設定ファイルが見つかりません、初期設定を開始します。");
                configure conf = setup();
                return conf;

            }
        }
        private void init(configure conf)
        {
            HttpGetUrl = "https://akakitune87.net/api/v4/pso2emergency";
            chanpionUrl = "https://xpow0wu0s5.execute-api.ap-northeast-1.amazonaws.com/v2";

            hc = new HttpClient();
            emgGetter = new aki_luaEventGetter(HttpGetUrl, hc);
            chGetter = new aki_luaChanpionGetter(chanpionUrl, hc);
            service = new DiscordService(conf.url, hc);
            admin = new EventAdmin(emgGetter,chGetter);
            bot = new botController(service, admin);

            bot.rodos = conf.rodos;
            bot.chp = conf.chp;

            this.chpFile = conf.xmlFile;

            if(chpFile != "")
            {
                admin.setChpTimeList(chpFile);
            }
        }

        private configure setup()
        {
            System.Console.WriteLine("DiscordのWebHooks URLを入力してください。");
            string url = System.Console.ReadLine();

            configure conf = new configure();
            conf.url = url;
            conf.rodos = true;

            return conf;
        }

        protected string convertCode(string str, Encoding encode, Encoding decode)
        {
            string convertStr = decode.GetString(
                System.Text.Encoding.Convert(
                    encode,
                    decode,
                    encode.GetBytes(str)));

            return convertStr;
        }

        //設定ファイル関連
        public string getFilename()
        {
            return configFile;
        }

        public bool saveConfig()    //設定ファイル保存
        {
            configure conf = new configure();
            conf.url = service.getUrl();
            conf.rodos = bot.rodos;
            conf.chp = bot.chp;
            conf.xmlFile = chpFile;
            bool result = XmlFileIO.xmlSave(conf.GetType(),getFilename(),conf);

            logOutput.writeLog("設定ファイルを保存しました。");

            return true;
        }

        public bool loadConfig(out object obj)  //設定ファイル読み込み
        {

            configure conf = new configure();
            bool result = XmlFileIO.xmlLoad(conf.GetType(), getFilename(),out obj);

            logOutput.writeLog("設定ファイルを読み込みました。");
            
            
            return result;
        }
        

    }

    public class configure
    {
        public string url;
        public bool rodos;
        public bool chp;
        public string xmlFile;
    }
}

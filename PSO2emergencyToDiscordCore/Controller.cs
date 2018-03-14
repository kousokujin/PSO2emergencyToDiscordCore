using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    class Controller:AbstractController
    {
        HttpClient hc;
        AbstractEventGetter emgGetter;
        EventAdmin admin;
        botController bot;
        AbstractService service;

        string discordurl;
        string HttpGetUrl;

        public Controller(string url)
        {
            discordurl = url;
            init();
            loop();
        }

        private void init()
        {
            HttpGetUrl = "https://akakitune87.net/api/v4/pso2emergency";

            hc = new HttpClient();
            emgGetter = new aki_luaEventGetter(HttpGetUrl, hc);
            service = new DiscordService(discordurl, hc);
            admin = new EventAdmin(emgGetter);
            bot = new botController(service, admin);

            initComandSet();
        }

        private void initComandSet()
        {
            addCommand("stop");
            addCommand("exit");
            addCommand("quit");
            addCommand("reload");
            addCommand("rodos");
            addCommand("post");
            addCommand("help");
            addCommand("url");
        }

        public override void commandProcess(string command, string[] args)
        {
            if(command == "stop" || command == "exit" || command == "quit")   //停止
            {
                end = true;
            }

            if(command == "reload") //再取得
            {
                admin.getEmgFromNet();
            }

            if(command == "post")
            {
                if (args.Length == 1)
                {
                    string postEnc = convertCode(args[0], Console.InputEncoding, Encoding.UTF8);
                    service.sendService(postEnc);
                }
            }

            if (command == "rodos") //バル・ロドス通知設定関連
            {
                if (args.Length == 1)
                {
                    if (args[0] == "true" || args[0] == "1" || args[0] == "yes" || args[0] == "y" || args[0] == "enable")
                    {
                        bot.rodos = true;
                        logOutput.writeLog("デイリーバル・ロドス討伐(VH)の通知は有効にしました。");
                    }

                    if (args[0] == "false" || args[0] == "0" || args[0] == "no" || args[0] == "n" || args[0] == "disable")
                    {
                        bot.rodos = false;
                        logOutput.writeLog("デイリーバル・ロドス討伐(VH)の通知は無効にしました。");
                    }
                }
                else
                {
                    if(bot.rodos == true)
                    {
                        System.Console.WriteLine("デイリーバル・ロドス討伐(VH)の通知は有効です。");
                    }
                    else
                    {
                        System.Console.WriteLine("デイリーバル・ロドス討伐(VH)の通知は無効です。");
                    }
                }
            }

            if(command == "url")
            {
                if(args.Length == 1)
                {
                    service.url = args[0];
                    logOutput.writeLog(string.Format("Discord WebHooks URLを{0}に変更しました。",args[0]));
                }
                else
                {
                    System.Console.WriteLine(service.url);
                }
            }

            if(command == "help")
            {
                outputHelp();
            }
        }

        private void outputHelp()
        {
            System.Console.WriteLine("使い方");
            System.Console.WriteLine("post [文字列] : Discordに[文字列]を投稿します。");
            System.Console.WriteLine("rodos : バル・ロドス討伐(VH)通知の状態を表示します。");
            System.Console.WriteLine("rodos [enable|disable]: バル・ロドス討伐(VH)の通知を有効または無効にします。");
            System.Console.WriteLine("reload : 緊急クエストの情報を再取得します。");
            System.Console.WriteLine("url : Discord WebHooks URLを表示します。");
            System.Console.WriteLine("url [WebHooks URL]: Discord WebHooks URLを[WebHooks URL]に設定します。");
            System.Console.WriteLine("stop : PSO2emergencyToDiscordCoreを終了します。");
        }

        private string convertCode(string str, Encoding encode, Encoding decode)
        {
            string convertStr = decode.GetString(
                System.Text.Encoding.Convert(
                    encode,
                    decode,
                    encode.GetBytes(str)));

            return convertStr;
        }
        

    }
}

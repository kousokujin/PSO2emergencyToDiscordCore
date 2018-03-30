using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    class ConsoleController : ControllerCore
    {

        protected List<string> commandList;
        protected bool end;   //終了フラグ

        public ConsoleController()
        {
            end = false;
            commandList = new List<string>();

            initComandSet();

            //デバッグ
            loop();

        }

        protected void addCommand(string command)
        {
            commandList.Add(command);
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
            addCommand("list");
            addCommand("rm");
            addCommand("delete");
            addCommand("del");
            addCommand("add");
            addCommand("hasha");
            addCommand("version");
            //addCommand("debug");
        }

        public bool checkCommand(string command)
        {
            foreach(string c in commandList)
            {
                if(c == command)
                {
                    return true;
                }
            }

            return false;
        }

        public string[] Separate(string input)
        {
            return input.Split(' ');
        }

        public void Process(string str)
        {
            string[] commandArr = Separate(str);
            bool check = checkCommand(commandArr[0]);

            if(check == false && str != "")
            {
                System.Console.WriteLine("コマンドが見つかりません。");
                return;
            }
            if(str == "")
            {
                return;
            }

            string[] args = new string[commandArr.Length-1];
            int i = 0;
            foreach(string st in commandArr)
            {
                if (i != 0)
                {
                    args[i - 1] = st;
                }

                i++;
            }
            commandProcess(commandArr[0], args);
            
        }

        public void commandProcess(string command, string[] args)
        {
            if (command == "stop" || command == "exit" || command == "quit")   //停止
            {
                saveConfig();
                end = true;
            }

            if (command == "reload") //再取得
            {
                admin.getEmgFromNet();
            }

            if (command == "post")
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
                    bool modify = false;

                    if (args[0] == "true" || args[0] == "1" || args[0] == "yes" || args[0] == "y" || args[0] == "enable")
                    {
                        bot.rodos = true;
                        logOutput.writeLog("デイリーバル・ロドス討伐(VH)の通知は有効にしました。");
                        modify = true;
                    }

                    if (args[0] == "false" || args[0] == "0" || args[0] == "no" || args[0] == "n" || args[0] == "disable")
                    {
                        bot.rodos = false;
                        logOutput.writeLog("デイリーバル・ロドス討伐(VH)の通知は無効にしました。");
                        modify = true;
                    }

                    if(modify == false)
                    {
                        System.Console.WriteLine("値が不正です。");
                    }
                }
                else
                {
                    if (bot.rodos == true)
                    {
                        System.Console.WriteLine("デイリーバル・ロドス討伐(VH)の通知は有効です。");
                    }
                    else
                    {
                        System.Console.WriteLine("デイリーバル・ロドス討伐(VH)の通知は無効です。");
                    }
                }
            }

            if (command == "url")
            {
                if (args.Length == 1)
                {
                    service.url = args[0];
                    logOutput.writeLog(string.Format("Discord WebHooks URLを{0}に変更しました。", args[0]));
                }
                else
                {
                    System.Console.WriteLine(service.url);
                }
            }

            if (command == "list") //緊急クエストの表示
            {
                string str = myFunction.EmgArrStrNumbered(admin.getPSO2Event());
                System.Console.WriteLine(str);
            }

            if(command == "rm" || command == "del" || command == "delete")
            {
                if(args.Length == 1)
                {
                    int number = 0;
                    bool enableDel = int.TryParse(args[0], out number);

                    if(enableDel == true)
                    {
                        admin.delPSO2Event(number - 1);
                    }                
                }
                else
                {
                    System.Console.WriteLine("引数が不正です。");
                }
            }

            if(command == "add")
            {
                if (args.Length != 5 && args.Length != 6)
                {
                    System.Console.WriteLine("引数の数が違います。");
                    return;
                }

                bool monthEnable = false;
                bool dayEnable = false;
                bool hourEnable = false;
                bool minEnable = false;

                int month = 0;
                int day = 0;
                int hour = 0;
                int min = 0;
                string name = "";

                monthEnable = int.TryParse(args[0], out month);
                dayEnable = int.TryParse(args[1], out day);
                hourEnable = int.TryParse(args[2], out hour);
                minEnable = int.TryParse(args[3], out min);
                name = args[4];

                if((monthEnable && dayEnable && hourEnable && minEnable) == false)
                {
                    System.Console.WriteLine("値が不正です。");
                    return;
                }

                DateTime time = new DateTime(DateTime.Now.Year, month, day, hour, min, 0);

                if ((time - DateTime.Now).TotalSeconds < 0) //過去の時間の場合は1年追加
                {
                    time += new TimeSpan(365, 0, 0, 0);
                }

                if (args.Length == 5)
                {
                    emgQuest em = new emgQuest(time, name);
                    admin.addEmg(em);
                }
                
                if (args.Length == 6)
                {
                    string live = args[5];
                    emgQuest em = new emgQuest(time, name, live);
                    admin.addEmg(em);

                }
            }

            if (command == "hasha")
            {
                bool ok = false;
                if (args.Length == 0)
                {
                    if (bot.chp == true)
                    {
                        logOutput.writeLog("覇者の紋章通知は有効です。");
                        ok = true;
                    }
                    else
                    {
                        logOutput.writeLog("覇者の紋章通知は無効です。");
                        ok = true;
                    }
                }
                if (args.Length == 1)
                {
                    if (args[0] == "true" || args[0] == "1" || args[0] == "yes" || args[0] == "y" || args[0] == "enable")
                    {
                        bot.chp = true;
                        logOutput.writeLog("覇者の紋章通知を有効にしました。");
                        ok = true;
                    }

                    if (args[0] == "false" || args[0] == "0" || args[0] == "no" || args[0] == "n" || args[0] == "disable")
                    {
                        bot.chp = false;
                        logOutput.writeLog("覇者の紋章通知を無効にしました。");
                        ok = true;
                    }

                    if(args[0] == "list")
                    {
                        logOutput.writeLog("覇者の紋章通知時間は以下の通りです。\n{0}", admin.getChpList());
                        ok = true;
                    }

                    if(args[0] == "reload")
                    {
                        logOutput.writeLog("覇者の紋章キャンペーンの情報を取得します。");
                        admin.getChanpionFromNet();
                        ok = true;
                    }
                }

                if(args.Length == 2)
                {
                    if(args[0] == "csvfile")
                    {
                        chpFile = args[1];
                        logOutput.writeLog("{0}から覇者の紋章通知時間を読み込みます。", args[1]);
                        admin.setChpTimeList(args[1]);

                        if(bot.chp == false)
                        {
                            System.Console.WriteLine("覇者の紋章通知は無効です。「hasha enable」コマンドで有効にしてください。");
                        }

                        ok = true;
                    }
                }

                if(ok == false)
                {
                    System.Console.WriteLine("値が不正です。");
                }
            }

            if (command == "help")
            {
                outputHelp();
            }

            if(command == "version")
            {
                outputVersion();
            }


            /*
            if(command == "debug")
            {
                admin.debug = true;
            }
            */
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

        private void outputVersion()
        {
            System.Console.WriteLine("PSO2emergencyToDiscordCore version {0}",myFunction.getAssemblyVersion());
            System.Console.WriteLine("Copyright (c) 2018 Kousokujin.");
            System.Console.WriteLine("Released under the MIT license.");
        }

        public void loop()
        {
            do
            {
                logOutput.outputPronpt();
                string commandInput = System.Console.ReadLine();
                Process(commandInput);

            } while (end == false);

            logOutput.writeLog("PSO2emergencyToDiscordCoreを終了します。");
        }
    }
}

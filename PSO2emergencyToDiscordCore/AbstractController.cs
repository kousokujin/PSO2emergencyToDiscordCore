using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractController
    {

        protected List<string> commandList;
        protected bool end;   //終了フラグ

        public AbstractController()
        {
            end = false;
            commandList = new List<string>();

        }

        protected void addCommand(string command)
        {
            commandList.Add(command);
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

        public abstract void commandProcess(string command, string[] args); //コマンドの処理
    }
}

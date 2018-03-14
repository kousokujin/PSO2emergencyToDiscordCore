using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractController
    {

        List<string> commandList;
        protected bool end;   //終了フラグ

        public AbstractController(List<string> cmdList)
        {
            end = false;
            commandList = cmdList;

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

            if(check == false)
            {
                System.Console.WriteLine("コマンドが見つかりません。");
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
                string commandInput = System.Console.ReadLine();
                Process(commandInput);

            } while (end == false);
        }

        public abstract void commandProcess(string command, string[] args); //コマンドの処理
    }
}

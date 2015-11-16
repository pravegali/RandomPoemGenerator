using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoemGenerator.Models
{
    public class PoemCore
    {
        Dictionary<String,List<List<String>>> cfg= new Dictionary<string, List<List<string>>>();
        String inputFilePathe = "";
        Random rnd = new Random();
        string[] FileLines;
        string poem;
        public Dictionary<String, List<List<String>>> CFG
        {
            get
            {
                return cfg;
            }
            set
            {
                cfg = value;
            }

        }


        public String InputFile
        {
            get
            { return inputFilePathe; }

            set
            {
                inputFilePathe = value;
            }

        }

        public PoemCore(string[] FileLines)
        {
            this.FileLines=FileLines;

            CFGDictionaryPrep(FileLines);
        }

        public void CFGDictionaryPrep(string[] FileLines)
        {
            string[] RuleValuePairs;
            string[] LineSecondPart;
            string[] RuleOrTerminals;
            
            List<string> values = new List<string>();
            foreach (string FileLine in FileLines)
            {
                List<List<string>> cfgValue = new List<List<string>>();
                RuleValuePairs = FileLine.Split(new string[] {": "}, StringSplitOptions.None);
                LineSecondPart = RuleValuePairs[1].Split(' ');
                
                //values = new List<string>(LineSecondPart);
                foreach (string Secondpart in LineSecondPart)
                {
                    RuleOrTerminals = Secondpart.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    values = new List<string>(RuleOrTerminals);
                    cfgValue.Add(values);
                }

                cfg.Add(RuleValuePairs[0], cfgValue);
            }
        }

        public string GetPoem()
        {
            
            int LineCount = 0;
            List<List<string>> CFGValue=new List<List<string>>();
           //Get the line count
            if (cfg.TryGetValue("POEM", out CFGValue))
            {
                LineCount=CFGValue.Count;

            }

            //form each line
            for (int i=1 ;i<= LineCount;i++ )
            {

                EvaluateComponent("LINE");

            }

            return this.poem;
        }

        public void EvaluateComponent(string word)
        {
            string keyword = "";
            List<List<string>> CFGValue = new List<List<string>>();
            if (cfg.TryGetValue(word, out CFGValue))
            {
                //choose random string from the first list of strings the CFGValue list. 
                foreach (List<string> value in CFGValue)
                {
                    int r = rnd.Next(value.Count);
                    //check the first char
                    //if it is $ then it is a command
                    char firstChar = value[r][0];
                    if (value[r][0] == '$')
                    {
                        //execute the command;
                        if (value[r].Split('$')[1] == "LINEBREAK")
                        {
                            poem += " "+"<br/>";

                             
                        }
                        if (value[r].Split('$')[1] == "END")
                        {

                            poem += " " + "<br/>";
                            return ;
                        }


                    }
                    else if(value[r][0]=='<')
                    {
                        string ruleOrWordOrCmd = value[r].Split(new char[] { '<', '>' })[1];
                        EvaluateComponent(ruleOrWordOrCmd);
                    }

                    else
                    {
                        poem +=" "+ value[r];
                    }


                }
                
            }
            return;

        }

        
    }
}
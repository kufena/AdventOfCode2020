using System;
using System.IO;
using System.Collections.Generic;

namespace _19
{

    /*
     * This little class implements the 42+ (n*42) (n*31) rule creation for an
     * input.  It does it by finding the longest input line, and then dividing it
     * by 5.  This gives us the maximum length of the new rules required for the
     * input.
     * It then generates rule for input lengths up to the longest one.  Hopefully.
     *
     * THIS DOENS'T QUITE WORK - THE RULES ARE OBVIOUSLY NOT EXHAUSTIVE.  NEED TO FIX.
     *
    */
    // ToDo To Do
    public class NewRules {

        public static void CalculateNewRules(StreamReader stream) {
            string line;
            while ((line = stream.ReadLine()).Trim() != "") {
                ; // read all the old rules.
            }

            int longestline = 0;
            while((line = stream.ReadLine()) != null) {
                if (line.Trim().Length > longestline)
                    longestline = line.Trim().Length;
            }

            longestline += 50; // jsut a bit of slack.

            int counter = 9000;
            List<int> newrules = new List<int>();
            int rules = (int) Math.Round((float)longestline / 5.0f);
            for(int j = 1; j < rules; j++) {
                int charc = j * 5;
                string prefix = "";
                for(int k = 0; k < j; k++) prefix += "42 ";
                for(int k = 1; k < rules; k++) {
                    string newrule = prefix;
                    for(int l = 0; l < k; l++) {
                        newrule += "42 ";
                        charc += 5;
                    }
                    for(int l = 0; l < k; l++) {
                        newrule += "31 ";
                        charc += 5;
                    }
                    if (charc <= longestline) {
                        Console.WriteLine($"{counter} : {newrule}");
                        newrules.Add(counter);
                        counter++;
                    }
                    newrule = "";
                }     
            }
            string finalchoice = "10000: ";
            foreach(var s in newrules) finalchoice += $"{s} | ";
            Console.WriteLine(finalchoice);
        }

    }
}
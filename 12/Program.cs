using System;
using System.IO;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's Go!");

            var file = File.OpenRead(args[0]);
            var sr = new StreamReader(file);
            Program.Step2(sr);
        }

        public static void Step1(StreamReader sr) {
            

            int direction = 0; // east
            int positionx = 0;
            int positiony = 0;

            string line;
            while ((line = sr.ReadLine() ) != null) {
                int val = Int32.Parse(line.Substring(1));
                Console.WriteLine(line + " quantity is " + val);
                switch (line[0]) {
                    case 'N':
                      positiony += Int32.Parse(line.Substring(1));
                      break;
                    case 'E':
                      positionx += Int32.Parse(line.Substring(1));
                      break;
                    case 'S':
                      positiony -= Int32.Parse(line.Substring(1));
                      break;
                    case 'W':
                      positionx -= Int32.Parse(line.Substring(1));
                      break;

                    case 'L':
                      int moves = (int) (val / 90);
                      for(int i = 0; i < moves; i++) {
                          direction -= 1;
                          if (direction < 0) direction = 3;
                      }
                      break;
                    case 'R':
                      int moves2 = (int) (val / 90);
                      for(int i = 0; i < moves2; i++) {
                          direction += 1;
                          if (direction > 3) direction = 0;
                      }
                      break;
                    case 'F':
                      switch(direction) {
                          case 0: // we're going East
                            positionx += Int32.Parse(line.Substring(1));
                            break;
                          case 1: // we're going South
                            positiony -= Int32.Parse(line.Substring(1));
                            break;
                          case 2: // we're going West
                            positionx -= Int32.Parse(line.Substring(1));
                            break;
                          case 3: // we're going north
                            positiony += Int32.Parse(line.Substring(1));
                            break;
                          default:
                            throw new Exception($"{direction} is not 0,1,2,or 3");
                      }
                      break;
                    default:
                      throw new Exception(line);
                }
            }

            Console.WriteLine($"Position = {positionx},{positiony}, direction is {direction}");
        }

        
        public static void Step2(StreamReader sr) {
            

            int positionx = 0;
            int positiony = 0;

            int wpositionx = 10;
            int wpositiony = 1;

            string line;
            while ((line = sr.ReadLine() ) != null) {
                int val = Int32.Parse(line.Substring(1));
                Console.WriteLine(line + " quantity is " + val);
                switch (line[0]) {
                    case 'N':
                      wpositiony += Int32.Parse(line.Substring(1));
                      break;
                    case 'E':
                      wpositionx += Int32.Parse(line.Substring(1));
                      break;
                    case 'S':
                      wpositiony -= Int32.Parse(line.Substring(1));
                      break;
                    case 'W':
                      wpositionx -= Int32.Parse(line.Substring(1));
                      break;

                    case 'L':
                      int moves = (int) (val / 90);
                      for(int i = 0; i < moves; i++) {
                          int spare = wpositionx;
                          wpositionx = -wpositiony;
                          wpositiony = spare;
                      }
                      break;
                    case 'R':
                      int moves2 = (int) (val / 90);
                      for(int i = 0; i < moves2; i++) {
                          int spare = wpositiony;
                          wpositiony = -wpositionx;
                          wpositionx = spare;
                      }
                      break;
                    case 'F':
                      positionx += Int32.Parse(line.Substring(1)) * wpositionx;
                      positiony += Int32.Parse(line.Substring(1)) * wpositiony;
                      break;
                    default:
                      throw new Exception(line);
                }
            }

            Console.WriteLine($"Position = ({positionx},{positiony}), waypoint is ({wpositionx},{wpositiony})");
        }
    }
}

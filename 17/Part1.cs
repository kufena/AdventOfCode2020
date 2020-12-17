using System;
using System.Collections.Generic;
using System.IO;

using static System.Console;

namespace _17
{

        public class Part1 {
        public static void Part1action(StreamReader stream) {

            List<string> lines = new List<string>();
            string line;
            while ((line = stream.ReadLine()) != null) {
                lines.Add(line);
            }
            int ylines = lines.Count;
            int xlines = lines[0].Length;

            int iterations = 16;

            // boundaries.
            int finaly = ylines + (2*iterations);
            int finalx = xlines + (2*iterations);
            int finalz = 1 + (2*iterations);

            // where does the top corner of the input go?
            int initz = iterations+1;
            int initx = iterations;
            int inity = iterations;
            /*
            if (xlines % 2 == 0)
                initx = (iterations+1) - ((int) (xlines/2));
            else
                initx = (iterations+1) - ((int) ((xlines-1) / 2));

            if (ylines % 2 == 0)
                inity = (iterations+1) - ((int) (ylines/2));
            else
                inity = (iterations+1) - ((int) ((ylines-1)/2));
            */

            // initialize space to empty
            int[][][] space = new int[finalx][][];
            for(int i = 0; i < finalx; i++) {
                space[i] = new int[finaly][];
                for(int j = 0; j < finaly; j++) {
                    space[i][j] = new int[finalz];
                    for(int k = 0; k < finalz; k++)
                        space[i][j][k] = 0;
                }
            }

            // read the lines and set up the plane.
            int zn = initz;
            int yn = inity;

            foreach(var pline in lines) {
                Console.WriteLine(pline);
                int xn = initx;
                for(int k = 0; k < pline.Length; k++) {
                    if (pline[k] == '#') {
                        space[xn+k][yn][zn] = 1;
                        Console.WriteLine($"setting ({xn+k},{yn},{zn}) to 1");
                    }
                }
                yn += 1;
            }

            for(int k = 0; k < finalz; k++)
                printplane(space, k, finalx, finaly, finalz);

            // compute 6 times.
            for(int iter = 0; iter < 6; iter++) {
                Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine($"ITERATION {iter}");
                int[][][] newspace = new int[finalx][][];
                for(int i = 0; i < finalx; i++) {
                    newspace[i] = new int[finaly][];
                    for(int j = 0; j < finaly; j++) {
                        newspace[i][j] = new int[finalz];
                        for(int k = 0; k < finalz; k++) {
                            int count = countAbout(i,j,k,space,finalx,finaly,finalz);
                            if (space[i][j][k] == 1 && (count == 2 || count == 3))
                            {
                                newspace[i][j][k] = 1;
                            }
                            else {
                                //Console.WriteLine($"setting ({i},{j},{k}) to 0 from 1");
                                newspace[i][j][k] = 0;
                            }

                            if (space[i][j][k] == 0 && count == 3) {
                                //Console.WriteLine($"setting ({i},{j},{k}) to 1 from 0");
                                newspace[i][j][k] = 1;
                            }
                        }
                    }
                }

                space = newspace;
                for(int k = 0; k < finalz; k++)
                    printplane(space, k, finalx, finaly, finalz);

            }

            // count
            int counter = 0;
            for(int i = 0; i < finalx; i++)
                for(int j = 0; j < finaly; j++)
                    for(int k = 0; k < finalz; k++)
                        counter += space[i][j][k];

        
        
            Console.WriteLine($"Final count is {counter}");
        }

        static int countAbout(int x, int y, int z, int[][][] space, int finalx, int finaly, int finalz) {
            int count = 0;
            if (x - 1 >= 0) {
                count += space[x-1][y][z];
                if (y-1 >= 0) {
                    count += space[x-1][y-1][z];
                    if (z-1 >= 0) count += space[x-1][y-1][z-1];
                    if (z+1 < finalz) count += space[x-1][y-1][z+1];
                }
                if (y+1 < finaly) {
                    count += space[x-1][y+1][z];
                    if (z-1 >= 0) count += space[x-1][y+1][z-1];
                    if (z+1 < finalz) count += space[x-1][y+1][z+1];
                }
                if (z-1 >= 0) count += space[x-1][y][z-1];
                if (z+1 < finalz) count += space[x-1][y][z+1];
            }
            if (x + 1 < finalx) {
                //Console.WriteLine($"{x+1} {y} {z} {finalx} {finaly} {finalz}");
                count += space[x+1][y][z];
                if (y-1 >= 0) {
                    count += space[x+1][y-1][z];
                    if (z-1 >= 0) count += space[x+1][y-1][z-1];
                    if (z+1 < finalz) count += space[x+1][y-1][z+1];
                }
                if (y+1 < finaly) {
                    count += space[x+1][y+1][z];
                    if (z-1 >= 0) count += space[x+1][y+1][z-1];
                    if (z+1 < finalz) count += space[x+1][y+1][z+1];
                }
                if (z-1 >= 0) count += space[x+1][y][z-1];
                if (z+1 < finalz) count += space[x+1][y][z+1];
            }
            
            if (y-1 >= 0) {
                count += space[x][y-1][z];
                if (z-1 >= 0) count += space[x][y-1][z-1];
                if (z+1 < finalz) count += space[x][y-1][z+1];
            }
            if (y+1 < finaly) {
                count += space[x][y+1][z];
                if (z-1 >= 0) count += space[x][y+1][z-1];
                if (z+1 < finalz) count += space[x][y+1][z+1];
            }
            if (z-1 >= 0) count += space[x][y][z-1];
            if (z+1 < finalz) count += space[x][y][z+1];

            return count;
        }

        static void printplane(int[][][] space, int z, int finalx, int finaly, int finalz) {
            string s = "";
            int c = 0;
            for(int j = 0; j<finalx; j++) {
                for(int i = 0; i < finaly; i++) {
                    s += space[i][j][z] == 1 ? "#" : ".";
                    c += space[i][j][z];
                }
                s += "\n";
            }
            if (c > 0) { 
                Console.WriteLine($"z={z}");
                Console.WriteLine(s);
            }
        }
    }
}

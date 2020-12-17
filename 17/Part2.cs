using System;
using System.Collections.Generic;
using System.IO;

using static System.Console;

namespace _17
{

        public class Part2 {
        public static void Part2action(StreamReader stream) {

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
            int finalw = 1 + (2*iterations);

            // where does the top corner of the input go?
            int initz = iterations+1;
            int initx = iterations;
            int inity = iterations;
            int initw = iterations+1;

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
            int[][][][] space = new int[finalx][][][];
            for(int i = 0; i < finalx; i++) {
                space[i] = new int[finaly][][];
                for(int j = 0; j < finaly; j++) {
                    space[i][j] = new int[finalz][];
                    for(int k = 0; k < finalz; k++) {
                        space[i][j][k] = new int[finalw];
                        for(int l = 0; l < finalw; l++)
                            space[i][j][k][l] = 0;
                    }
                }
            }

            // read the lines and set up the plane.
            int wn = initw;
            int zn = initz;
            int yn = inity;

            foreach(var pline in lines) {
                Console.WriteLine(pline);
                int xn = initx;
                for(int k = 0; k < pline.Length; k++) {
                    if (pline[k] == '#') {
                        space[xn+k][yn][zn][wn] = 1;
                        Console.WriteLine($"setting ({xn+k},{yn},{zn},{wn}) to 1");
                    }
                }
                yn += 1;
            }

            //for(int k = 0; k < finalz; k++)
            //    printplane(space, k, finalx, finaly, finalz);

            // compute 6 times.
            for(int iter = 0; iter < 6; iter++) {
                Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine($"ITERATION {iter}");
                int[][][][] newspace = new int[finalx][][][];
                for(int i = 0; i < finalx; i++) {
                    newspace[i] = new int[finaly][][];
                    for(int j = 0; j < finaly; j++) {
                        newspace[i][j] = new int[finalz][];
                        for(int k = 0; k < finalz; k++) {
                            newspace[i][j][k] = new int[finalw];
                            for(int l = 0; l < finalw; l++) {
                                int count = countAbout(i,j,k,l,space,finalx,finaly,finalz,finalw);
                                if (space[i][j][k][l] == 1 && (count == 2 || count == 3))
                                {
                                    newspace[i][j][k][l] = 1;
                                }
                                else {
                                    //Console.WriteLine($"setting ({i},{j},{k}) to 0 from 1");
                                    newspace[i][j][k][l] = 0;
                                }

                                if (space[i][j][k][l] == 0 && count == 3) {
                                    //Console.WriteLine($"setting ({i},{j},{k}) to 1 from 0");
                                    newspace[i][j][k][l] = 1;
                                }
                            }
                        }
                    }
                }

                space = newspace;
                //for(int k = 0; k < finalz; k++)
                //    printplane(space, k, finalx, finaly, finalz);

            }

            // count
            int counter = 0;
            for(int i = 0; i < finalx; i++)
                for(int j = 0; j < finaly; j++)
                    for(int k = 0; k < finalz; k++)
                        for(int l = 0; l < finalw; l++)
                            counter += space[i][j][k][l];

        
        
            Console.WriteLine($"Final count is {counter}");
        }

        static int countAbout(int x, int y, int z, int w, int[][][][] space, int finalx, int finaly, int finalz, int finalw) {
            int count = 0;
            int dx;
            int dy;
            int dz;
            int dw;

            for(int i = 0; i < 3; i++) {
                dx = i-1;
                for(int j = 0; j < 3; j++) {
                    dy = j-1;
                    for(int k = 0; k < 3; k++) {
                        dz = k-1;
                        for(int l = 0; l < 3; l++) {
                            dw = l-1;

                            if (dx==0&&dy==0&&dz==0&&dw==0)
                                continue;
                            else {
                                int nx = x + dx;
                                int ny = y + dy;
                                int nz = z + dz;
                                int nw = w + dw;
                                if (nx >= 0 && nx < finalx &&
                                    ny >= 0 && ny < finaly &&
                                    nz >= 0 && nz < finalz &&
                                    nw >= 0 && nw < finalw)
                                    count = count + space[nx][ny][nz][nw];
                            }
                        }
                    }
                }
            }
            return count;
        }

    }
}


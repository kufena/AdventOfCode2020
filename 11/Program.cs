using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

WriteLine("Hello All - let's go!");

var file = File.OpenRead(args[0]);
StreamReader sr = new StreamReader(file);

string line = sr.ReadLine();
List<int[]> rows = new List<int[]>();

do {
  int[] row = new int[line.Length];
  int counter = 0;
  foreach(char c in line) {
      switch (c) {
          case '.':
            row[counter] = -1;
            break;
          case 'L':
            row[counter] = 0;
            break;
          default:
            throw new Exception ($"Unexpected bagging in the seating area {c}");
      }
      counter++;
  }
  rows.Add(row);
  line = sr.ReadLine();
} while (line != null);

// assume it's square.
int rowcount = rows.Count;
int width = rows[0].Length;

WriteLine($"Found {rows.Count} rows of width {rows[0].Length}");

bool changed;


var ocounts = new List<int[]>();
for(int i = 0; i < rowcount; i++)
{
    int[] newrow = new int[width];
    ocounts.Add(newrow);
    for(int j = 0; j < width; j++)
        newrow[j] = 0;
}

while (true) {

    //M.printboard(rows, ocounts);
    //ReadLine();
    //WriteLine();
    //WriteLine();
    
    changed = false;

    var newboard = new List<int[]>();

    for(int i = 0; i < rowcount; i++) {
        int[] newrow = new int[width];
        for(int j = 0; j < width; j++) {
            if (rows[i][j] == -1) {
                newrow[j] = -1;
                ocounts[i][j] = 0;
            }
            if (rows[i][j] == 0) {
                int c = M.countoccupied(rows, j, i,rowcount, width);
                ocounts[i][j] = c;
                if (c == 0)
                    newrow[j] = 1;
                else
                    newrow[j] = 0;
            }
            if (rows[i][j] == 1) {
                int c = M.countoccupied(rows, j, i,rowcount, width);
                ocounts[i][j] = c;
                if (c >= 5)
                    newrow[j] = 0;
                else
                    newrow[j] = 1;
            }
            changed = changed || (rows[i][j] != newrow[j]);

        }
        newboard.Add(newrow);
    }

    if (!changed) {
        break;
    }
    WriteLine($"changed = {changed}");
    rows = newboard;
}

int occupied = 0;
for(int i = 0; i < rowcount; i++) {
    for(int j = 0; j < width; j++) {
        if (rows[i][j] == 1)
            occupied ++;
    }
}

WriteLine($"There are {occupied} occupied seats.");
WriteLine("All done!");

public static class M {
    public static List<(int,int)> positions(int x, int y, int rowcount, int width) {
        int x_1 = x-1;
        int x1 = x + 1;
        int y_1 = y-1;
        int y1 = y+1;

        List<(int,int)> results = new List<(int, int)>();
        if (y_1 >= 0) {
            if (x_1 >= 0) results.Add((x_1,y_1));
            results.Add((x,y_1));
            if (x1 < width) results.Add((x1,y_1));
        }
        if (x_1 >= 0) results.Add((x_1,y));
        if (x1 < width) results.Add((x1,y));

        if (y1 < rowcount) {
            results.Add((x,y1));
            if (x_1 >= 0) results.Add((x_1,y1));
            if (x1 < width) results.Add((x1, y1));
        }
        return results;
    }

    public static int countoccupied(List<int[]> board, int x, int y, int rowcount, int width) {
        return
            findoccupied(board, x, y, rowcount, width, -1, -1) +
            findoccupied(board, x, y, rowcount, width, -1, 0) +
            findoccupied(board, x, y, rowcount, width, -1, 1) +
            findoccupied(board, x, y, rowcount, width, 0, -1) +
            findoccupied(board, x, y, rowcount, width, 0, 1) +
            findoccupied(board, x, y, rowcount, width, 1, 0) +
            findoccupied(board, x, y, rowcount, width, 1, 1) +
            findoccupied(board, x, y, rowcount, width, 1, -1);
    }

    public static int findoccupied(List<int[]> board, int x, int y, int rowcount, int width, int dx, int dy) {
        int newx = x + dx;
        int newy = y + dy;
        if (newx >= 0 && newx < width && newy >= 0 && newy < rowcount) {
            if (board[newy][newx] == 1) return 1;
            if (board[newy][newx] == 0) return 0;
            return findoccupied(board, newx, newy, rowcount, width, dx, dy);
        }
        return 0;
    }

    public static void printboard(List<int[]> board, List<int[]> ocounts) {
        for(int i = 0; i < board.Count; i++) {
            for(int j = 0; j < board[i].Length; j++) {
                if (board[i][j] == -1) Write('.');
                if (board[i][j] == 0) Write('L');
                if (board[i][j] == 1) Write('#');
            }
            Write(" ");
            for(int j = 0; j < board[i].Length; j++) {
                Write(ocounts[i][j]);
            }
            WriteLine();
        }

    }

    public static void printpositions(List<(int,int)> positions) {
        foreach(var (x,y) in positions) {
            Write($"({x},{y}) ");
        }
        WriteLine();
    }
}

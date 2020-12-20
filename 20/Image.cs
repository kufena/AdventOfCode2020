using System;
using System.Collections.Generic;

namespace _20 {

    public class Image {
        static byte Lmask = 0x80;
        static byte Rmask = 0x40;
        static byte Tmask = 0x20;
        static byte Bmask = 0x10;
        static byte FLmask = 0x08;
        static byte FRmask = 0x04;
        static byte FTmask = 0x02;
        static byte FBmask = 0x01;
        
        string[] image;
        public int num { get; init; }

        List<Image> vertices;
        public int NumVertices { get => this.vertices.Count; }

        string top;
        string bottom;
        string left;
        string right;


        string reversetop;
        string reversebottom;
        string reverseleft;
        string reverseright;

        public Image(string[] image, int num) {
            vertices = new List<Image>();
            this.num = num;
            this.image = image;
            top = image[0];
            bottom = image[image.Length-1];
            left = findEdge(x => x[0]);
            right = findEdge(x => x[x.Length-1]);

            reversetop = reverseString(top);
            reversebottom = reverseString(bottom);
            reverseleft = reverseString(left);
            reverseright = reverseString(right);
        }

        public void AddVertex(Image image) {
            if (!vertices.Contains(image)) 
                vertices.Add(image);
        }

        private string findEdge(Func<string,char> fn) {
            string result = "";
            for(int i = 0; i < image.Length; i++) {
                result += $"{fn(image[i])}";
            }
            return result;
        }

        public byte matchImage(Image image) {
            byte total = 0;
            total |= matchEdge(image.top, Tmask);
            total |= matchEdge(image.bottom, Bmask);
            total |= matchEdge(image.left, Lmask);
            total |= matchEdge(image.right, Rmask);
            total |= matchEdge(image.reversetop, FTmask);
            total |= matchEdge(image.reversebottom, FBmask);
            total |= matchEdge(image.reverseleft, FLmask);
            total |= matchEdge(image.reverseright, FRmask);
            return total;
        }

        public byte matchEdge(string edge, byte score) {
            byte a = (top == edge ? score : ((byte)0));
            byte b = (bottom == edge ? score : ((byte)0));
            byte c = (left == edge ? score : ((byte)0));
            byte d = (right == edge ? score : ((byte)0));
            byte e = (byte) (a | b);
            byte f = (byte) (c | d);
            byte g = (byte) (e | f);
            return g;
        }

        public string matchImageCondition(Image image) {
            string total = "";
            total += matchEdgeCondition(image.top, "T");
            total += matchEdgeCondition(image.bottom, "B");
            total += matchEdgeCondition(image.left, "L");
            total += matchEdgeCondition(image.right, "R");
            total += matchEdgeCondition(image.reversetop, "FT");
            total += matchEdgeCondition(image.reversebottom, "FB");
            total += matchEdgeCondition(image.reverseleft, "FL");
            total += matchEdgeCondition(image.reverseright, "FR");
            return total;
        }

        public string matchEdgeCondition(string edge, string prefix) {
            string a = (top == edge) ? (prefix+"-T ") : "";
            string b = (bottom == edge) ? (prefix+"-B ") : "";
            string c = (left == edge) ? (prefix+"-L ") : "";
            string d = (right == edge) ? (prefix+"-R ") : "";
            string e = a+b;
            string f = c+d;
            string g = e+f;
            return g;
        }

        public string reverseString(string s) {
            string rev = "";
            for(int i = s.Length-1; i>=0; i--)
                rev += $"{s[i]}";
            return rev;
        }

        bool horizflip = false;
        bool vertiflip = false;

        List<Image> leftvert = null;
        List<Image> rightvert = null;
        List<Image> upvert = null;
        List<Image> downvert = null;
        List<Image> fleftvert = null;
        List<Image> frightvert = null;
        List<Image> fupvert = null;
        List<Image> fdownvert = null;
        
        public void Orientate() {
            leftvert = new List<Image>();
            rightvert = new List<Image>();
            upvert = new List<Image>();
            downvert = new List<Image>();
            fleftvert = new List<Image>();
            frightvert = new List<Image>();
            fupvert = new List<Image>();
            fdownvert = new List<Image>();

            Console.WriteLine($"Vertex {num} has {NumVertices} vertices.");
            Console.ReadLine();
            foreach(var im in vertices) {

                byte b = matchImage(im);
                string s = matchImageCondition(im);
                Console.WriteLine($"{im.num} :: {s}");
                if ((b & Tmask) > 0) {
                    upvert.Add(im);
                }
                if ((b & Bmask) > 0) {
                    downvert.Add(im);
                }
                if ((b & Lmask) > 0) {
                    rightvert.Add(im);
                }
                if ((b & Rmask) > 0) {
                    leftvert.Add(im);
                }
                if ((b & FBmask) > 0) {
                    fupvert.Add(im);
                }
                if ((b & FTmask) > 0) {
                    fdownvert.Add(im);
                }
                if ((b &FRmask) > 0) {
                    frightvert.Add(im);
                }
                if ((b &FLmask) > 0) {
                    fleftvert.Add(im);
                }
                
            }

            LinesAndLines(upvert,"up vertices");
            LinesAndLines(downvert,"down vertices");
            LinesAndLines(leftvert, "left vertices");
            LinesAndLines(rightvert, "right vertices");
            LinesAndLines(fupvert,"upf vertices");
            LinesAndLines(fdownvert,"downf vertices");
            LinesAndLines(fleftvert, "leftf vertices");
            LinesAndLines(frightvert, "rightf vertices");
        }

        private void LinesAndLines(List<Image> verts, string verttext) {
            if (verts.Count > 0) {
                Console.Write($"{num} has {verttext}");
                foreach(var im in verts) Console.Write($" {im.num}");
                Console.WriteLine();
            }
        }
    }
}
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

        public List<Image> vertices;
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

        public Image finalleft = null;
        public Image finalright = null;
        public Image finalup = null;
        public Image finaldown = null;
        public int x = 0;
        public int y = 0;

        internal bool TopLeft()
        {
            //work out if we're top left corner.
            if (NumVertices != 2) return false;
            Image one = vertices[0];
            Image two = vertices[1];

            if (RotateToFit(bottom, one, x => x.top) && RotateToFit(right, two, x => x.left)) {
                finaldown = one;
                finalright = two;
                one.finalup = this;
                two.finalleft = this;
                return true;
            }
            else if (RotateToFit(bottom, two, x => x.top) && RotateToFit(right, one, x => x.left)) {
                finaldown = two;
                finalright = one;
                two.finalup = this;
                one.finalleft = this;
                return true;
            }

            return false;
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

        public bool matchThese()
        {
            // prepare - we know we have left, maybe up.
            List<Image> remains = new List<Image>();
            foreach(var t in vertices)
            {
                if (finalleft == null)
                {
                    if (finalup == null)
                        remains.Add(t);
                    else
                    if (t.num != finalup.num)
                        remains.Add(t);
                }
                else if (t.num != finalleft.num)
                {
                    if (finalup == null)
                        remains.Add(t);
                    else 
                    if (t.num != finalup.num)
                        remains.Add(t);
                }
            }

            if (remains.Count == 0) // we're bottom right do nowt.
                return true;

            if (remains.Count == 1)
            {
                if (finalup == null && finalleft != null) // we're right - it's down.
                {
                    finalright = null;
                    finaldown = remains[0];
                    finaldown.finalup = this;
                    return true;
                }

                if (finalup != null && finalleft == null) // we're bottom left
                {
                    finalright = remains[0];
                    finalright.finalleft = this;
                    finaldown = null;
                    return true;
                }

                if (finalup != null && finalleft != null) // we're bottom middle or right middle
                {
                    // we don't rotate so we can just look at the matches.
                    Image one = remains[0];
                    if (RotateToFit(right, one, x => x.left))
                    {
                        finalright = one;
                        one.finalleft = this;
                        finaldown = null;
                    }
                    else if (RotateToFit(bottom, one, x => x.top))
                    {
                        finaldown = one;
                        one.finalup = this;
                        finalright = null;
                    }
                    return true;
                }

                return false;
            }

            // should have two left.
            if (RotateToFit(right, remains[0], x => x.left) && RotateToFit(bottom, remains[1], x=> x.top))
            {
                finalright = remains[0];
                finaldown = remains[1];
                return true;
            }
            else if (RotateToFit(right, remains[1], x => x.left) && RotateToFit(bottom, remains[0], x => x.top))
            {
                finalright = remains[1];
                finaldown = remains[0];
                return true;
            }
            return false;
        }

        private bool RotateToFit(string side, Image im, Func<Image,string> selector)
        {
            for(int flip = 0; flip < 4; flip++)
            {
                switch (flip)
                {
                    case 0: // no flip.
                        for(int i = 0; i < 4; i++)
                        {
                            im.Rotate();
                            if (side == selector(im))
                                return true;
                        }
                        break;
                    case 1: // vertflip
                        im.FlipVert();
                        for (int i = 0; i < 4; i++)
                        {
                            im.Rotate();
                            if (side == selector(im))
                                return true;
                        }
                        im.FlipVert(); // put it back.
                        break;

                    case 2: // horizontal flip
                        im.FlipHori();
                        for (int i = 0; i < 4; i++)
                        {
                            im.Rotate();
                            if (side == selector(im))
                                return true;
                        }
                        im.FlipHori(); // put it back.
                        break;
                    case 3: // horizontal flip
                        im.FlipHori();
                        im.FlipVert();
                        for (int i = 0; i < 4; i++)
                        {
                            im.Rotate();
                            if (side == selector(im))
                                return true;
                        }
                        im.FlipVert();
                        im.FlipHori(); // put it back.
                        break;
                }
            }
            return false;
        }

        public string reverseString(string s) {
            string rev = "";
            for(int i = s.Length-1; i>=0; i--)
                rev += $"{s[i]}";
            return rev;
        }

        bool horizflip = false;
        bool vertiflip = false;

        public List<Image> leftvert = null;
        public List<Image> rightvert = null;
        public List<Image> upvert = null;
        public List<Image> downvert = null;
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

            //Console.WriteLine($"Vertex {num} has {NumVertices} vertices.");
            //Console.ReadLine();
            foreach(var im in vertices) {

                byte b = matchImage(im);
                string s = matchImageCondition(im);
                //Console.WriteLine($"{im.num} :: {s}");
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

            /*
            LinesAndLines(upvert,"up vertices");
            LinesAndLines(downvert,"down vertices");
            LinesAndLines(leftvert, "left vertices");
            LinesAndLines(rightvert, "right vertices");
            LinesAndLines(fupvert,"upf vertices");
            LinesAndLines(fdownvert,"downf vertices");
            LinesAndLines(fleftvert, "leftf vertices");
            LinesAndLines(frightvert, "rightf vertices");
            */
        }

        private void LinesAndLines(List<Image> verts, string verttext) {
            if (verts.Count > 0) {
                Console.Write($"{num} has {verttext}");
                foreach(var im in verts) Console.Write($" {im.num}");
                Console.WriteLine();
            }
        }

        public void FlipVert()
        {
            int len = image[0].Length;
            for(int i = 0; i < len; i++)
            {
                image[i] = reverseString(image[i]);
            }
            CalculateSides(len);
        }

        public void FlipHori()
        {
            int len = image[0].Length;
            string[] newimage = new string[len];
            for (int i = 0; i < len; i++)
            {
                newimage[(len-i) - 1] = image[i];
            }
            image = newimage;
            CalculateSides(len);

        }

        public void Rotate()
        {
            int len = image[0].Length;
            string[] newimg = new string[len]; // assumes square.
            for (int i = 0; i < len; i++)
            {
                string line = "";
                for (int j = 0; j < len; j++)
                {
                    line += image[j][i];
                }
                newimg[i] = line;
            }

            CalculateSides(len);
        }

        private void CalculateSides(int len)
        {
            top = image[0];
            bottom = image[len - 1];
            left = findEdge(x => x[0]);
            right = findEdge(x => x[x.Length - 1]);

            reversetop = reverseString(top);
            reversebottom = reverseString(bottom);
            reverseleft = reverseString(left);
            reverseright = reverseString(right);
        }
    }
}
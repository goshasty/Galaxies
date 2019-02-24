using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Galaxies
{
    public partial class Form1 : Form
    {
        List<Circle> sh = new List<Circle>(); //List of galaxies
        List<List<double>> length = new List<List<double>>();
        List<List<bool>> belongMatrixAntAlgorithm = new List<List<bool>>();
        double[] D;
        int[] color;
        const double inf = double.MaxValue;
        string file, file0;
        int variant;
        ulong Objid_of_Divide;
        bool algorythmed = false;
        int ind_min = -1, ind_max = -1;
        double[] parameters = { 0.1, 0.001, 0.06, 0.08 };
        double Merge_parameter = 0.05;

        public Form1()
        {
            variant = -1;
            Circle.Radius = 5;
            Circle.Color_shape = Color.White;
            InitializeComponent();
            panel1.Visible = true;
            panel2.Visible = false;
            pictureBox1.Visible = false;
            trackBar1.Visible = false;
            vScrollBar1.Visible = false;
            hScrollBar1.Visible = false;
            button_next_algorithm.Visible = false;
            button_return_to_file.Visible = false;
            panel1.Left = 0;
            panel1.Top = 0;
            panel1.Height = 557;
            panel1.Width = 742;
            label2.Visible = true;
            label2.Top = 0;
            label2.Left = 0;
            label2.Text = "";
            label3.Visible = true;
            label3.Top = 0;
            label3.Left = 0;
            label3.Text = "";
            label1.Text = "";
        }

       
        void Change_coordinates() //From sphere to 3D 
        {
            System.IO.File.Delete("Coordinates.txt");
            StreamWriter sw = new StreamWriter("Coordinates.txt");
            string[] s = System.IO.File.ReadAllLines(openFileDialog1.FileName);
            for (int i = 1; i < s.Length; i++)
            {

                string[] values = s[i].Split('\t');
                string ra = values[0].Replace('.', ','); // f
                string dec = values[1].Replace('.', ','); // g
                string red_shift = values[2].Replace('.', ',');
                string objid = values[3].Replace('.', ',');
                string mas = values[4].Replace('.', ',');

                double r = 300000.0 / 66.93 * double.Parse(red_shift);

                const double rad_from_grad = Math.PI / 180.0;
                double x = r * Math.Cos(double.Parse(dec) * Math.PI / 180.0) * Math.Cos(double.Parse(ra) * rad_from_grad);
                double y = r * Math.Cos(double.Parse(dec) * Math.PI / 180.0) * Math.Sin(double.Parse(ra) * rad_from_grad);
                double z = r * Math.Sin(double.Parse(dec) * rad_from_grad);
                sw.WriteLine(x + " " + y + " " + z + " " + objid + " " + mas);
                sh.Add(new Circle(double.Parse(ra), double.Parse(dec), double.Parse(red_shift)));

                #region commented 
                /*
                int j = 0;
                while (s[i][j] == ' ')
                {
                    j++;
                }
                int k = j;
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string f = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string g = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string cr = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string objid = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (k < s[i].Length && s[i][k] != ' ')
                {
                    k++;
                }
                string mas = s[i].Substring(j, k - j);
                double r = 300000.0 / 66.93 * double.Parse(cr);
                double x = r * System.Math.Cos(double.Parse(g) * System.Math.PI / 180.0) * System.Math.Cos(double.Parse(f) * System.Math.PI / 180.0);
                double y = r * System.Math.Cos(double.Parse(g) * System.Math.PI / 180.0) * System.Math.Sin(double.Parse(f) * System.Math.PI / 180.0);
                double z = r * System.Math.Sin(double.Parse(g) * System.Math.PI / 180.0);

                double x, y, z;
                get_values_from_string(s[i], out x, out y, out z);
                sw.WriteLine(x + " " + y + " " + z + " " + objid + " " + mas);
                sh.Add(new Circle(double.Parse(f), double.Parse(g), double.Parse(cr)));
                */
                #endregion
            }
            sw.Close();
            openFileDialog1.FileName = "Coordinates.txt";
            file = "Coordinates.txt";
        }

        public class Cmp : IComparer<List<double>>
        {
            public int Compare(List<double> x, List<double> y)
            {
                return y[2].CompareTo(x[2]);
            }
        }

        void Cut_lines() //Cutting not-used lines 
        {
            if (variant != 4 && variant != 5)
            {
                List<List<double>> ways = new List<List<double>>();
                for (int i = 0; i < sh.Count; i++)
                {
                    ways.Add(new List<double>());
                    ways[i].Add(i);
                    ways[i].Add(sh[i].Next_index);
                    ways[i].Add(length[i][sh[i].Next_index]);
                }
                ways.Sort(new Cmp());
                for(int i = 0; i < ways.Count; i++)
                {
                    if(i < parameters[variant] * ways.Count)
                    {
                        sh[(int)ways[i][0]].Draw_Line = false;
                    }
                    else sh[(int)ways[i][0]].Draw_Line = true;
                }
            }
        }

        void FillDate(string t) //Getting coordinates from file 
        {
            string[] s = System.IO.File.ReadAllLines(t);
            for (int i = 0; i < s.Length; i++)
            {
                double x = 0, y = 0, z = 0;
                int j = 0;
                while (s[i][j] == ' ')
                {
                    j++;
                }
                int k = j;
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string f = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string g = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string cr = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (s[i][k] != ' ')
                {
                    k++;
                }
                string id = s[i].Substring(j, k - j);
                j = k;
                while (s[i][j] == ' ')
                {
                    j++;
                    k++;
                }
                while (k < s[i].Length && s[i][k] != ' ')
                {
                    k++;
                }
                string mas = s[i].Substring(j, k - j);
                x = double.Parse(f);
                y = double.Parse(g);
                z = double.Parse(cr);
                double mass = double.Parse(mas);
                sh[i].X = x;
                sh[i].Y = y;
                sh[i].Z = z;
                sh[i].Objid = id;
                sh[i].Mass = mass;
                sh[i].Brightness = 2.54 * Math.Pow(10, -6) * Math.Pow(10, 0.4 * (0 - sh[i].Mass));
            }
            for (int i = 0; i < sh.Count; i++)
            {
                length.Add(new List<double>());
                for (int j = 0; j < sh.Count; j++)
                {
                    length[i].Add(0);
                }
            }
            for (int i = 0; i < sh.Count; i++)
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    length[i][j] = Math.Sqrt(Math.Pow(sh[j].X - sh[i].X, 2) + Math.Pow(sh[j].Y - sh[i].Y, 2) + Math.Pow(sh[j].Z - sh[i].Z, 2));
                    length[j][i] = length[i][j];
                }
            }
            double z_min = 2, z_max = -1;
            for (int i = 0; i < sh.Count; i++)
            {
                if(sh[i].Redshift > z_max)
                {
                    z_max = sh[i].Redshift;
                    ind_max = i;
                }
                if(sh[i].Redshift < z_min)
                {
                    z_min = sh[i].Redshift;
                    ind_min = i;
                }
            }
            pictureBox1.Invalidate();
        }

        void Algorytm_of_neighbour()
        {
            int[,] way = new int[sh.Count, sh.Count];
            int index = 0;
            double length__way = double.MaxValue;
            for (int k = 0; k < sh.Count; k++)
            {
                bool b = true;
                double length_way = 0;
                for (int j = 0; j < sh.Count; j++)
                {
                    sh[j].Used = false;
                }
                int i = k;
                while (b)
                {
                    sh[i].Used = true;
                    int min_index, j = 0;
                    double min_length;
                    while (j < sh.Count && sh[j].Used)
                    {
                        j++;
                    }
                    if (j == sh.Count) break;
                    min_index = j;
                    min_length = length[i][j];
                    for (j = j; j < sh.Count; j++)
                    {
                        if (!sh[j].Used)
                        {
                            if (min_length > length[i][j])
                            {
                                min_length = length[i][j];
                                min_index = j;
                            }
                        }
                    }
                    length_way += min_length;
                    way[k, i] = min_index;
                    i = min_index;
                }
                way[k, i] = k;
                length_way += length[k][way[k, i]];
                if (length_way < length__way)
                {
                    index = k;
                    length__way = length_way;
                }
            }
            sh[index].Next_index = way[index, index];
            int f = way[index, index];
            while (f != index)
            {
                sh[f].Next_index = way[index, f];
                f = way[index, f];
            }
        }

        double Reduction(List<List<double>> a) //Reduction of table for method of branches and borders 
        {
            double k = 0;
            for (int i = 1; i < a.Count; i++)
            {
                double min = a[i][1];
                for (int j = 1; j < a.Count; j++)
                {
                    if (a[i][j] < min)
                    {
                        min = a[i][j];
                    }
                }
                for (int j = 1; j < a.Count; j++)
                {
                    a[i][j] -= min;
                }
                k += min;
            }
            for (int i = 1; i < a.Count; i++)
            {
                double min = a[1][i];
                for (int j = 1; j < a.Count; j++)
                {
                    if (a[j][i] < min)
                    {
                        min = a[j][i];
                    }
                }
                for (int j = 1; j < a.Count; j++)
                {
                    a[j][i] -= min;
                }
                k += min;
            }
            return k;
        }

        void Degrees(out int x, out int y, List<List<double>> a) //Counting "degrees" of zeros 
        {
            double h = 0;
            x = -1;
            y = -1;
            for (int i = 1; i < a.Count; i++)
            {
                for (int j = 1; j < a.Count; j++)
                {
                    if (a[i][j] == 0)
                    {
                        List<List<double>> b = new List<List<double>>();
                        for (int k = 0; k < a.Count; k++)
                        {
                            b.Add(new List<double>());
                            for (int s = 0; s < a.Count; s++)
                            {
                                b[k].Add(a[k][s]);
                            }
                        }
                        b[i][j] = inf;
                        double t = Reduction(b);
                        if (h == 0)
                        {
                            h = t;
                            x = i;
                            y = j;
                        }
                        else
                        {
                            if (t > h)
                            {
                                h = t;
                                x = i;
                                y = j;
                            }
                        }
                    }
                }
            }
        }

        double With(int x, int y, List<List<double>> a) //Using this way 
        {
            List<List<double>> b = new List<List<double>>();
            for (int k = 0; k < a.Count; k++)
            {
                b.Add(new List<double>());
                for (int s = 0; s < a.Count; s++)
                {
                    b[k].Add(a[k][s]);
                }
            }
            int xx = (int)a[x][0], yy = (int)a[0][y];
            b.RemoveAt(x);
            for (int i = 0; i < a.Count - 1; i++)
            {
                b[i].RemoveAt(y);
            }
            for (int i = 1; i < a.Count - 1; i++)
            {
                if (b[i][0] == yy)
                {
                    for (int j = 1; j < a.Count - 1; j++)
                    {
                        if (b[0][j] == xx)
                        {
                            b[i][j] = inf;
                        }
                    }
                }
            }
            return Reduction(b);
        }

        double Without(int x, int y, List<List<double>> a) //Not using this way 
        {
            List<List<double>> b = new List<List<double>>();
            for (int k = 0; k < a.Count; k++)
            {
                b.Add(new List<double>());
                for (int s = 0; s < a.Count; s++)
                {
                    b[k].Add(a[k][s]);
                }
            }
            b[x][y] = inf;
            return Reduction(b);
        }

        double iteration(double h0, List<List<double>> a, ref List<int[]> w) //One iteration from method of branches and borders 
        {
            int x, y;
            h0 += Reduction(a);
            Degrees(out x, out y, a);
            if (With(x, y, a) <= Without(x, y, a))
            {
                int[] ww = { (int)a[x][0], (int)a[0][y] };
                w.Add(ww);
                for (int i = 1; i < a.Count; i++)
                {
                    if (a[i][0] == a[0][y])
                    {
                        for (int j = 1; j < a.Count; j++)
                        {
                            if (a[0][j] == a[x][0])
                            {
                                a[i][j] = inf;
                            }
                        }
                    }
                }
                a.RemoveAt(x);
                for (int i = 0; i < a.Count; i++)
                {
                    a[i].RemoveAt(y);
                }
            }
            else
            {
                a[x][y] = inf;
            }
            return h0;
        }

        void Method_of_branches_and_borders()
        {
            List<int[]> w;
            List<List<double>> a = new List<List<double>>();
            for (int i = 0; i < sh.Count + 1; i++)
            {
                a.Add(new List<double>());
                for (int j = 0; j < sh.Count + 1; j++)
                    if (i == 0)
                    {
                        a[i].Add(j - 1);
                    }
                    else if (j == 0) a[i].Add(i - 1);
                    else if (i != j) a[i].Add(length[i - 1][j - 1]);
                    else a[i].Add(inf);
            }
            w = new List<int[]>();
            double h0 = Reduction(a);
            while (a.Count != 1)
            {
                iteration(h0, a, ref w);
            }
            for (int i = 0; i < w.Count; i++)
            {
                sh[w[i][0]].Next_index = w[i][1];
            }
        }

        void init(int v, ref int[] way) //Initialisation of arrays of Dejkstra-algorithm 
        {
            D = new double[sh.Count];
            color = new int[sh.Count];
            for (int i = 0; i < sh.Count; i++)
            {
                D[i] = double.MaxValue;
                color[i] = 0;
                way[i] = 0;
            }
            D[v] = 0;
            color[v] = 1;
        }

        void relax(int x, ref int[] way) //Changing length of ways (Dejkstra-algorithm) 
        {
            for (int i = 0; i < sh.Count; i++)
                if (D[i] > D[x] + length[x][i])
                {
                    D[i] = D[x] + length[x][i];
                    way[i] = x;
                    break;
                }
        }

        int findMin() //Finding next not-used point (Dejkstra-algorithm) 
        {
            int x = -1;
            double dist = double.MaxValue;
            for (int i = 0; i < sh.Count; i++)
            {
                if (D[i] < dist && color[i] == 0)
                {
                    x = i;
                    dist = D[i];
                }
            }
            return x;
        }

        void Algorythm_of_Dejkstra()
        {
            int v = 0;
            int[] way = new int[sh.Count];
            init(v, ref way);
            relax(v, ref way);
            int t = 0;
            for (int x = findMin(); x != -1; x = findMin())
            {
                t = x;
                color[x] = 1;
                relax(x, ref way);
            }
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Next_index = way[i];
            }
            int[] a = new int[sh.Count];
            for (int i = 0; i < sh.Count; i++)
            {
                a[way[i]] = -1;
            }
            for (int i = 0; i < sh.Count; i++)
            {
                if (a[i] != -1) sh[0].Next_index = i;
            }
        }

        double length_of_way(int[] way) //Counting length of way for method of local improvement 
        {
            double l = 0;
            for (int i = 1; i < sh.Count - 1; i++)
            {
                l += length[way[i]][way[i - 1]];
            }
            return l;
        }

        void Method_of_local_improvement()
        {
            for (int i = 0; i < sh.Count * 10; i++)
            {
                Method_of_local_improvement_iteration();
            }
        }

        void Method_of_local_improvement_iteration() //One iteration from method of local improvement 
        {
            int[] way00 = new int[sh.Count + 1];
            if (sh[sh.Count - 1].Used)
            {
                way00[0] = 0;
                for (int i = 1; i < sh.Count; i++)
                {
                    way00[i] = sh[way00[i - 1]].Next_index;
                }
                way00[sh.Count] = 0;
            }
            else way00[0] = -1;
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Used = false;
            }
            int[] way = new int[sh.Count + 1];
            int[] way1 = new int[sh.Count + 1];
            int[] way0 = new int[sh.Count + 1];
            Random rnd = new Random();
            for (int i = 0; i < sh.Count; i++)
            {
                int t = rnd.Next(sh.Count);
                while (sh[t].Used)
                {
                    t = rnd.Next(sh.Count);
                }
                way[i] = t;
                way1[i] = t;
                way0[i] = t;
                sh[t].Used = true;
            }
            way[sh.Count] = way[0];
            way1[sh.Count] = way1[0];
            way0[sh.Count] = way0[0];
            double l0 = length_of_way(way);
            bool b = true;
            while (b)
            {
                for (int i = 0; i <= sh.Count; i++)
                {
                    way[i] = way0[i];
                    way1[i] = way[i];
                }
                for (int i = 1; i < sh.Count - 1; i++)
                {
                    for (int j = 0; j < sh.Count + 1; j++)
                    {
                        way1[j] = way0[j];
                    }
                    int t = way1[i];
                    way1[i] = way1[i + 1];
                    way1[i + 1] = t;
                    if (length_of_way(way1) < l0)
                    {
                        l0 = length_of_way(way1);
                        for (int j = 0; j < sh.Count; j++)
                        {
                            way[j] = way1[j];
                        }
                    }
                }
                for (int j = 0; j < sh.Count; j++)
                {
                    way1[j] = way0[j];
                }
                int k = way1[sh.Count - 1];
                way1[sh.Count - 1] = way1[sh.Count];
                way1[sh.Count] = k;
                way1[0] = k;
                if (length_of_way(way) < l0)
                {
                    l0 = length_of_way(way);
                    for (int j = 0; j < sh.Count; j++)
                    {
                        way[j] = way1[j];
                    }
                }
                b = false;
                for (int i = 0; i < sh.Count; i++)
                {
                    if (way[i] != way0[i]) b = true;
                }
                if (b)
                    for (int j = 0; j < sh.Count; j++)
                    {
                        way0[j] = way[j];
                    }
            }
            if (way00[0] != -1)
            {
                if (length_of_way(way00) > length_of_way(way0))
                {
                    for (int i = 1; i < sh.Count + 1; i++)
                    {
                        sh[way0[i - 1]].Next_index = way0[i];
                    }
                }
            }
            else
            {
                for (int i = 1; i < sh.Count + 1; i++)
                {
                    sh[way0[i - 1]].Next_index = way0[i];
                }
            }
        }

        void Method_K_average()
        {
            Random rnd = new Random();
            const int Amount = 5;
            double[,] Points = new double[Amount, 3];
            double max_ra, min_ra, max_dec, min_dec, max_z, min_z;
            max_ra = min_ra = sh[0].Ra;
            max_dec = min_dec = sh[0].Dec;
            max_z = min_z = sh[0].Redshift;
            for (int i = 0; i < sh.Count; i++)
            {
                if (max_ra < sh[i].Ra) max_ra = sh[i].Ra;
                if (min_ra > sh[i].Ra) min_ra = sh[i].Ra;
                if (max_dec < sh[i].Dec) max_dec = sh[i].Dec;
                if (min_dec > sh[i].Dec) min_dec = sh[i].Dec;
                if (max_z < sh[i].Redshift) max_z = sh[i].Redshift;
                if (min_z > sh[i].Redshift) min_z = sh[i].Redshift;
            }
            for (int i = 0; i < Amount; i++)
            {
                Points[i, 0] = rnd.Next((int)min_ra, (int)max_ra) + rnd.NextDouble();
                Points[i, 1] = rnd.Next((int)min_dec, (int)max_dec) + rnd.NextDouble();
                Points[i, 2] = rnd.NextDouble() * (max_z - min_z) + min_z;
            }
            double[,] PointsXYZ = new double[Amount, 3];
            for (int i = 0; i < Amount; i++)
            {
                PointsXYZ[i, 0] = 300000.0 / 66.93 * Points[i, 2] * System.Math.Cos(Points[i, 0] * System.Math.PI / 180.0) * System.Math.Cos(Points[i, 1] * System.Math.PI / 180.0);
                PointsXYZ[i, 1] = 300000.0 / 66.93 * Points[i, 2] * System.Math.Cos(Points[i, 1] * System.Math.PI / 180.0) * System.Math.Sin(Points[i, 0] * System.Math.PI / 180.0);
                PointsXYZ[i, 2] = 300000.0 / 66.93 * Points[i, 2] * System.Math.Sin(Points[i, 1] * System.Math.PI / 180.0);
            }
            int[] Cluster_num = new int[sh.Count];
            double[,] Sums = new double[Amount, 4];
            for (int k = 0; k < Amount * 2; k++)
            {
                for (int i = 0; i < sh.Count; i++)
                {
                    double r = double.MaxValue;
                    int ind = -1;
                    for (int j = 0; j < Amount; j++)
                    {
                        Sums[j, 0] = 0;
                        Sums[j, 1] = 0;
                        Sums[j, 2] = 0;
                        Sums[j, 3] = 0;
                        if (r > Math.Sqrt(Math.Pow(sh[i].X - PointsXYZ[j, 0], 2) + Math.Pow(sh[i].Y - PointsXYZ[j, 1], 2) + Math.Pow(sh[i].Z - PointsXYZ[j, 2], 2)))
                        {
                            r = Math.Sqrt(Math.Pow(sh[i].X - PointsXYZ[j, 0], 2) + Math.Pow(sh[i].Y - PointsXYZ[j, 1], 2) + Math.Pow(sh[i].Z - PointsXYZ[j, 2], 2));
                            ind = j;
                        }
                    }
                    Cluster_num[i] = ind;
                }
                for (int i = 0; i < sh.Count; i++)
                {
                    Sums[Cluster_num[i], 0] += sh[i].X;
                    Sums[Cluster_num[i], 1] += sh[i].Y;
                    Sums[Cluster_num[i], 2] += sh[i].Z;
                    Sums[Cluster_num[i], 3] += 1;
                }
                for (int i = 0; i < Amount; i++)
                {
                    PointsXYZ[i, 0] = Sums[i, 0] / Sums[i, 3];
                    PointsXYZ[i, 1] = Sums[i, 1] / Sums[i, 3];
                    PointsXYZ[i, 2] = Sums[i, 2] / Sums[i, 3];
                }
            }
            int[] index_large = new int[3];
            int[] length = {0, 0, 0};
            for (int i = 0; i < Amount; i++)
            {
                if (length[0] < Sums[i, 3])
                {
                    index_large[2] = index_large[1];
                    length[2] = length[1];
                    index_large[1] = index_large[0];
                    length[1] = length[0];
                    index_large[0] = i;
                    length[0] = (int)Sums[i, 3];
                }
                else if(length[1] < Sums[i, 3])
                {
                    index_large[2] = index_large[1];
                    length[2] = length[1];
                    index_large[1] = i;
                    length[1] = (int)Sums[i, 3];
                }
                else if(length[2] < Sums[i,3])
                {
                    index_large[2] = i;
                    length[2] = (int)Sums[i, 3];
                }
            }
            int[] indexx = new int[3], index_0 = new int[3];
            //int indexx = -1, index_0 = -1;
            for (int i = 0; i < 3; i++)
            {
                indexx[i] = -1;
                index_0[i] = -1;
            }
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Draw_Line = false;
                if (Cluster_num[i] == index_large[0] || Cluster_num[i] == index_large[1] || Cluster_num[i] == index_large[2])
                {
                    int t = (index_large[0] == Cluster_num[i]) ? 0 : ((Cluster_num[i] == index_large[1]) ? 1 : 2);
                    if(indexx[t] == -1)
                    {
                        indexx[t] = i;
                        index_0[t] = i;
                        sh[i].Draw_Line = true;
                    }
                    else
                    {
                        sh[i].Next_index = indexx[t];
                        indexx[t] = i;
                        sh[i].Draw_Line = true;
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (indexx[i] != -1)
                {
                    sh[indexx[i]].Next_index = index_0[i];
                    sh[indexx[i]].Draw_Line = true;
                }
            }
            //sh[indexx].Next_index = index_0;
            //sh[indexx].Draw_Line = true;
        }

        private void Change_panel()
        {
            panel1.Visible = false;
            panel1.Enabled = false;
            panel2.Visible = true;
            button_visualisation.Visible = true;
        }

        private void button_openfile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            file = openFileDialog1.FileName;
            file0 = file;
            Change_panel();
            panel2.Height = Form1.ActiveForm.Height - 38;
            panel2.Width = Form1.ActiveForm.Width - 16;
            panel2.Left = 0;
            panel2.Top = 0;
            Change_coordinates();
        }

        List<List<bool>> Ant_algorithm()
        {
            for(int i = 0; i < sh.Count; i++)
            {
                sh[i].Draw_Line = false;
                sh[i].Next_index = 0;
            }
            AntAlghorithm a = new AntAlghorithm(length, sh);
            List<List<bool>> aa = a.Algorithm();
            for(int i = 0; i < aa.Count; i++)
            {
                for(int j = i; j < aa.Count; j++)
                {
                    if(aa[i][j])
                    {
                        sh[i].Next_index = j;
                        sh[i].Draw_Line = true;
                    }
                }
            }
            return aa;
        }

        private double[] FindCenterAndPeriod()
        {
            double max_ra, min_ra, max_dec, min_dec, max_z, min_z;
            max_ra = min_ra = sh[0].Ra;
            max_dec = min_dec = sh[0].Dec;
            max_z = min_z = sh[0].Redshift;
            for(int i = 0; i < sh.Count; i++)
            {
                if (max_ra < sh[i].Ra) max_ra = sh[i].Ra;
                if (min_ra > sh[i].Ra) min_ra = sh[i].Ra;
                if (max_dec < sh[i].Dec) max_dec = sh[i].Dec;
                if (min_dec > sh[i].Dec) min_dec = sh[i].Dec;
                if (max_z < sh[i].Redshift) max_z = sh[i].Redshift;
                if (min_z > sh[i].Redshift) min_z = sh[i].Redshift;
            }
            double[] a = {(max_ra + min_ra) / 2.0, (max_dec + min_dec) / 2.0, (max_z - min_z) / 10.0, min_z, max_z};
            return a;
        }

        private void button_visualisation_Click(object sender, EventArgs e)
        {
            FillDate(file);
            panel2.Visible = false;
            button_visualisation.Visible = false;
            pictureBox1.Visible = true;
            button_return_to_file.Visible = true;
            button_next_algorithm.Visible = true;
            vScrollBar1.Visible = true;
            hScrollBar1.Visible = true;
            pictureBox1.Enabled = true;
            pictureBox1.Visible = true;
            trackBar1.Visible = true;
           
            MinCut.Enabled = true;
            MinCut.Visible = true;
            MinCut.Location = new Point(30, 50);
            
            MaxCut.Enabled = true;
            MaxCut.Visible = true;
            MaxCut.Location = new Point(30, 75);
            MinText.Enabled = true;
            MinText.Visible = true;
            MinText.Location = new Point(50, 49);
            MaxText.Enabled = true;
            MaxText.Visible = true;
            MaxText.Location = new Point(50, 74);
            Cut.Enabled = true;
            Cut.Visible = true;
            Cut.Location = new Point(50, 94);
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Draw_Line = false;
            }
            algorythmed = true;
            pictureBox1.Visible = true;
            pictureBox1.Enabled = true;
            int sds = Form1.ActiveForm.Height;
            sds = button_next_algorithm.Height;
            sds = hScrollBar1.Height;
            pictureBox1.Height = this.Height - (button_next_algorithm.Height + hScrollBar1.Height + menuStrip1.Height) - 50;
            pictureBox1.Width = this.Width - vScrollBar1.Width;
            pictureBox1.Left = 0;
            pictureBox1.Top = menuStrip1.Height;
            menuStrip1.Width = this.Width;
            button_return_to_file.Visible = true;
            button_next_algorithm.Visible = true;
            trackBar1.Visible = true;
            vScrollBar1.Visible = true;
            hScrollBar1.Visible = true;
            pictureBox1.Refresh();
            Algorytm_of_neighbour();
            double[] cent_per = FindCenterAndPeriod();
            double z = cent_per[3];
            bool[,]matr = new bool[sh.Count, sh.Count];
            ulong[,] list = new ulong[11, 2];
            for (int i = 0; i < sh.Count; i++)
            {
                for(int j = 0; j < sh.Count; j++)
                {
                    matr[i, j] = false;
                }
            }
            for(int j = 0; j < 11; j++)
            {
                double min_r = double.MaxValue;
                int index = -1;
                for(int i = 0; i < sh.Count; i++)
                {
                    double r = Math.Pow((sh[i].Ra - cent_per[0]), 2) + Math.Pow((sh[i].Dec - cent_per[1]), 2) + Math.Pow((sh[i].Redshift * 10 - z * 10), 2);
                    if(r < min_r)
                    {
                        min_r = r;
                        index = i;
                    }
                    sh[i].Number_of_cluster = -1;
                    if (sh[i].Draw_Line)
                    {
                        matr[i, sh[i].Next_index] = true;
                        matr[sh[i].Next_index, i] = true;
                    }
                }
                dfs(0, index, matr);
                int sum = 0;
                for (int i = 0; i < sh.Count; i++)
                {
                    if(sh[i].Number_of_cluster == 0)
                    {
                        sum++;
                    }
                }
                list[j, 0] = ulong.Parse(sh[index].Objid);
                list[j, 1] = (ulong)sum;
                z += cent_per[2];
            }
            int sum_max = int.MinValue;
            int ind = -1;
            for(int i = 0; i < 11; i++)
            {
                if((int)list[i, 1] > sum_max)
                {
                    ind = i;
                    sum_max = (int)list[i, 1];
                }
            }
            Objid_of_Divide = list[ind, 0];
            int index_div = -1;
            for(int i = 0; i < sh.Count; i++)
            {
                if(ulong.Parse(sh[i].Objid) == Objid_of_Divide)
                {
                    index_div = i;
                }
                sh[i].Number_of_cluster = -1;
            }
            dfs(0, index_div, matr);
            double max_z = double.MinValue, min_z = double.MaxValue;
            for(int i = 0; i < sh.Count; i++)
            {
                if(sh[i].Number_of_cluster == 0)
                {
                    if (sh[i].Redshift < min_z) min_z = sh[i].Redshift;
                    if (sh[i].Redshift > max_z) max_z = sh[i].Redshift;
                }
            }
            for (int i = 0; i < sh.Count; i++)
            {
                if(sh[i].Redshift < min_z - 0.2 || sh[i].Redshift > max_z + 0.2)
                {
                  sh.RemoveAt(i);
                  length.RemoveAt(i);
                  for (int j = 0; j < length.Count; j++)
                  {
                    length[j].RemoveAt(i);
                  }
                  i--;
                }
            }
            variant = 0;
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Number_of_cluster = -1;
                sh[i].Used = false;
            }
            /*switch (variant)
            {
                case 0: Algorytm_of_neighbour(); break;
                case 1: Method_of_branches_and_borders(); break;
                case 2: Algorythm_of_Dejkstra(); break;
                case 3: Method_of_local_improvement(); break;
                case 4: Ant_algorithm(); break;
                case 5: Method_K_average(); break;
            }
            Cut_lines();
            Make_file();*/
                pictureBox1.Refresh();
        }

        Point3D Center() //Finding center of masses 
        {
            Point3D v;
            int x = 0, y = 0, z = 0;
            for (int j = 0; j < sh.Count; j++)
            {
                x += (int)sh[j].X;
                y += (int)sh[j].Y;
                z += (int)sh[j].Z;
            }
            v = new Point3D(x / sh.Count, y / sh.Count, z / sh.Count);
            return v;
        }

        void Draw(Graphics g, Point pp) //Drawing point 
        {
            if (pp.X < pictureBox1.Width && pp.Y < pictureBox1.Height)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(26, Color.White)), new Rectangle(new Point(pp.X - 5, pp.Y - 5), new Size(10, 10)));
                g.FillEllipse(new SolidBrush(Color.FromArgb(51, Color.White)), new Rectangle(new Point(pp.X - 4, pp.Y - 4), new Size(8, 8)));
                g.FillEllipse(new SolidBrush(Color.FromArgb(89, Color.White)), new Rectangle(new Point(pp.X - 3, pp.Y - 3), new Size(6, 6)));
                g.FillEllipse(new SolidBrush(Color.FromArgb(127, Color.White)), new Rectangle(new Point(pp.X - 2, pp.Y - 2), new Size(4, 4)));
                g.FillEllipse(new SolidBrush(Color.FromArgb(191, Color.White)), new Rectangle(new Point(pp.X - 1, pp.Y - 1), new Size(2, 2)));
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, Color.White)), new Rectangle(pp, new Size(1, 1)));
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Drawing on picturebox 
        {
            pictureBox1.Height = this.Height - (button_next_algorithm.Height + hScrollBar1.Height + menuStrip1.Height) - 50;
            pictureBox1.Width = this.Width - vScrollBar1.Width - 16;
            if (algorythmed)
            {
                if (sh.Count > 0)
                {
                    Point3D v = Center();
                    Point3D[] p1 = new Point3D[sh.Count];
                    for (int i = 0; i < sh.Count; i++)
                    {
                        p1[i] = new Point3D((int)(sh[i].X - v.x) * 2, (int)(sh[i].Y - v.y) * 2, (int)(sh[i].Z - v.z) * 2);
                    }
                    double max = p1[0].r, max1 = p1[0].r1;
                    for (int j = 0; j < p1.Length; j++)
                    {
                        if (p1[j].r > max)
                        {
                            max = p1[j].r;
                        }
                        if (p1[j].r1 > max1)
                        {
                            max1 = p1[j].r1;
                        }
                    }
                    double k = pictureBox1.Width / max / 2 * trackBar1.Value;
                    double k1 = pictureBox1.Height / max1 / 2 * trackBar1.Value;
                    Point[] p2 = new Point[sh.Count];
                    for (int j = 0; j < sh.Count; j++)
                    {
                        p2[j] = new Point((int)(p1[j].r * Math.Cos(p1[j].phi + hScrollBar1.Value / 10.0) * k + pictureBox1.Width / 2), (int)(p1[j].r1 * Math.Cos(p1[j].alpha + vScrollBar1.Value / 10.0) * k1 + pictureBox1.Height / 2));
                        Draw(e.Graphics, p2[j]);
                    }
                    for (int i = 0; i < sh.Count; i++)
                    {
                        if(variant == 5)
                            for (int j = i+1; j < sh.Count; j++)
                            {
                                if (belongMatrixAntAlgorithm[i][j])
                                    e.Graphics.DrawLine(new Pen(new SolidBrush(Color.AntiqueWhite), 1),
                                        new Point((int)p2[i].X, (int)p2[i].Y),
                                        new Point((int)p2[j].X, (int)p2[j].Y));
                            }


                         else
                            if (sh[i].Draw_Line)
                                e.Graphics.DrawLine(new Pen(new SolidBrush(Color.AntiqueWhite), 1), 
                                    new Point((int)p2[i].X, (int)p2[i].Y),
                                    new Point((int)p2[sh[i].Next_index].X, (int)p2[sh[i].Next_index].Y));
                    }
                    e.Graphics.FillEllipse(new SolidBrush(Color.White), new Rectangle(new Point(p2[ind_min].X, pictureBox1.Height - 55), new Size(10, 10)));
                    e.Graphics.FillEllipse(new SolidBrush(Color.White), new Rectangle(new Point(p2[ind_max].X, pictureBox1.Height - 55), new Size(10, 10)));
                    e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White)), new Point(p2[ind_min].X, pictureBox1.Height - 50), new Point(p2[ind_max].X, pictureBox1.Height - 50));
                    label3.Top = pictureBox1.Height - 21;
                    label3.Left = p2[ind_min].X - label3.Width + 10;
                    label3.Text = sh[ind_min].Redshift.ToString();
                    label2.Visible = true;
                    label3.Visible = true;
                    label2.Top = pictureBox1.Height - 31 - label2.Height;
                    label2.Left = p2[ind_max].X;
                    label2.Text = sh[ind_max].Redshift.ToString();
                }
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel1.Height = this.Height - 38;
            panel2.Height = this.Height - 38;
            panel1.Width = this.Width - 16;
            panel2.Width = this.Width - 116;
            pictureBox1.Height = this.Height - button_next_algorithm.Height - hScrollBar1.Height - 38;
            pictureBox1.Width = this.Width - vScrollBar1.Width - 16;
            pictureBox1.Invalidate();
        }

        private void button_return_to_algorithm_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            button_visualisation.Visible = true;
            pictureBox1.Visible = false;
            button_return_to_file.Visible = false;
            button_next_algorithm.Visible = false;
            vScrollBar1.Visible = false;
            hScrollBar1.Visible = false;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            sh.Clear();
            length.Clear();
            vScrollBar1.Value = 0;
            hScrollBar1.Value = 0;
            trackBar1.Visible = false;
            trackBar1.Value = 1;
            MinCut.Enabled = false;
            MinCut.Visible = false;
            MaxCut.Enabled = false;
            MaxCut.Visible = false;
            MinText.Enabled = false;
            MinText.Visible = false;
            MaxText.Enabled = false;
            MaxText.Visible = false;
            Cut.Enabled = false;
            Cut.Visible = false;
        }

        private void button_return_to_file_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.Enabled = true;
            button_openfile.Enabled = true;
            pictureBox1.Visible = false;
            button_return_to_file.Visible = false;
            button_next_algorithm.Visible = false;
            vScrollBar1.Visible = false;
            hScrollBar1.Visible = false;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            sh.Clear();
            length.Clear();
            vScrollBar1.Value = 0;
            hScrollBar1.Value = 0;
            trackBar1.Visible = false;
            trackBar1.Value = 1;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            MinCut.Enabled = false;
            MinCut.Visible = false;
            MaxCut.Enabled = false;
            MaxCut.Visible = false;
            MinText.Enabled = false;
            MinText.Visible = false;
            MaxText.Enabled = false;
            MaxText.Visible = false;
            Cut.Enabled = false;
            Cut.Visible = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        void dfs(int num, int v, bool[,]matr)
        {
            if (sh[v].Number_of_cluster != -1) return;
            else 
            {
                sh[v].Number_of_cluster = num;
                for(int i = 0; i < sh.Count; i++)
                {
                    if (matr[v, i]) dfs(num, i, matr);
                }
            }
        }

        private void Make_file()
        {
            System.IO.File.Delete(file0.Substring(0, file0.Length - 4) + "_" + "сluster_" + variant + ".txt");
            StreamWriter sw = new StreamWriter(file0.Substring(0, file0.Length - 4) + "_" + "сluster_" + variant + ".txt");
            string[] s = System.IO.File.ReadAllLines(file0);
            int num = 0;
            bool b = true;
            bool[,] matr = new bool[sh.Count, sh.Count];
            for (int i = 0; i < sh.Count; i++)
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    matr[i, j] = false;
                }
            }
            if (variant != 4)
                for (int i = 0; i < sh.Count; i++)
                {
                    if (sh[i].Draw_Line)
                    {
                       matr[i, sh[i].Next_index] = true;
                       matr[sh[i].Next_index, i] = true;
                    }
                }
            if (variant == 4)
                for (int i = 0; i < sh.Count; i++)
                    for (int j = 0; j< sh.Count; j++)
                        if (belongMatrixAntAlgorithm[i][j])
                        {
                            matr[i, j] = matr[j, i] = true;
                        }
            while (true)
            {
                int c = -1;
                for (int i = 0; i < sh.Count; i++)
                {
                    if (sh[i].Number_of_cluster == -1)
                    {
                        c = i;
                        break;
                    }
                }
                if (c == -1) break;
                else
                {
                    dfs(num, c, matr);
                    num++;
                }
            }
            int sum = 0;
            int sum_max = 0, ind = -1, ind1 = -1, sum_max1 = 0;
            for (int i = 0; i < num; i++)
            {
                sum = 0;
                for (int j = 0; j < sh.Count; j++)
                {
                    if (sh[j].Number_of_cluster == i)
                    {
                        sum++;
                    }
                }
                if (sum > sum_max)
                {
                    int t = sum_max, it = ind;
                    sum_max = sum;
                    ind = i;
                    if(sum_max1 < t)
                    {
                        sum_max1 = t;
                        ind1 = it;
                    }
                }
                else if (sum > sum_max1)
                {
                    sum_max1 = sum;
                    ind1 = i;
                }
            }
            for (int j = 0; j < sh.Count; j++)
            {
                if (sh[j].Number_of_cluster == ind)
                {
                    sum++;
                }
            }
            double center_z = 0, center_z_1 = 0, count = 0, count_1 = 0;
            for(int j = 0; j < sh.Count; j++)
            {
                if(sh[j].Number_of_cluster == ind)
                {
                    center_z += sh[j].Redshift * sh[j].Brightness;
                    count++;
                }
                if(sh[j].Number_of_cluster == ind1)
                {
                    center_z_1 += sh[j].Redshift * sh[j].Brightness;
                    count_1++;
                }
            }
            center_z = center_z / count;
            center_z_1 = center_z_1 / count_1;
            if (Math.Abs(center_z_1 - center_z) <= Merge_parameter)
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    if (sh[j].Number_of_cluster == ind || sh[j].Number_of_cluster == ind1)
                    {
                        sw.WriteLine(sh[j].Objid);
                    }
                }
            }
            else
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    if (sh[j].Number_of_cluster == ind)
                    {
                        sw.WriteLine(sh[j].Objid);
                    }
                }
            }
            sum = 0;
            sw.Close();
            Make_main_file(file0.Substring(0, file0.Length - 4) + "_" + "All_" + variant + ".txt", num);
            pictureBox1.Invalidate();
        }

        private void Make_main_file(string file_name, int num)
        {
            System.IO.File.Delete(file_name);
            StreamWriter sw = new StreamWriter(file_name);
            string[] s = System.IO.File.ReadAllLines(file0);
            bool b = true;
            bool[,] matr = new bool[sh.Count, sh.Count];
            for (int i = 0; i < sh.Count; i++)
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    matr[i, j] = false;
                }
            }
            for (int i = 0; i < sh.Count; i++)
            {
                if (sh[i].Draw_Line)
                {
                    matr[i, sh[i].Next_index] = true;
                    matr[sh[i].Next_index, i] = true;
                }
            }
            int sum = 0;
            double ra = 0, dec = 0, z = 0, sum_mass = 0;
            for (int i = 0; i < num; i++)
            {
                ra = 0;
                dec = 0;
                z = 0;
                sum_mass = 0;
                sw.WriteLine(i + ":");
                for (int j = 0; j < sh.Count; j++)
                {
                    if (sh[j].Number_of_cluster == i)
                    {
                        sw.WriteLine(sh[j].Objid + "   " + sh[j].Ra + "   " + sh[j].Dec + "   " + sh[j].Redshift + "   " + sh[j].Mass);
                        sum++;
                        ra += sh[j].Ra * sh[j].Brightness;
                        dec += sh[j].Dec * sh[j].Brightness;
                        z += sh[j].Redshift * sh[j].Brightness;
                        sum_mass += sh[j].Brightness;
                    }
                }
                ra /= sum_mass;
                dec /= sum_mass;
                z /= sum_mass;
                sw.WriteLine(sum);
                sw.WriteLine(ra + " " + dec + " " + z);
                sw.WriteLine();
                sum = 0;
            }
            sw.Close();
        }

        private void Make_final_file(string file_name)
        {
            System.IO.File.Delete(file_name);
            StreamWriter sw = new StreamWriter(file_name);
            int sum = 0;
            double ra = 0, dec = 0, z = 0, sum_mass = 0;
            for (int j = 0; j < sh.Count; j++)
            {
                if (sh[j].Draw_Line)
                {
                    if (sum == 0)
                    {
                        sw.WriteLine(sh[j].Objid + "   " + sh[j].Ra + "   " + sh[j].Dec + "   " + sh[j].Redshift + "   " + sh[j].Mass);
                        sum++;
                        ra += sh[j].Ra * sh[j].Brightness;
                        dec += sh[j].Dec * sh[j].Brightness;
                        z += sh[j].Redshift * sh[j].Brightness;
                        sum_mass += sh[j].Brightness;
                    }
                    sw.WriteLine(sh[sh[j].Next_index].Objid + "   " + sh[sh[j].Next_index].Ra + "   " + sh[sh[j].Next_index].Dec + "   " + sh[sh[j].Next_index].Redshift + "   " + sh[sh[j].Next_index].Mass);
                    sum++;
                    ra += sh[sh[j].Next_index].Ra * sh[sh[j].Next_index].Brightness;
                    dec += sh[sh[j].Next_index].Dec * sh[sh[j].Next_index].Brightness;
                    z += sh[sh[j].Next_index].Redshift * sh[sh[j].Next_index].Brightness;
                    sum_mass += sh[j].Brightness;
                }
            }
            ra /= sum_mass;
            dec /= sum_mass;
            z /= sum_mass;
            sw.WriteLine(sum);
            sw.WriteLine(ra + " " + dec + " " + z);
            sw.WriteLine();
            sum = 0;
            sw.Close();
        }

        private void button_next_algorithm_Click(object sender, EventArgs e)
        {
            if (variant <= 5)
            {
                for (int i = 0; i < sh.Count; i++)
                {
                    sh[i].Number_of_cluster = -1;
                    sh[i].Used = false;
                }

                //!
                //variant = 4;

                switch (variant)
                {
                    case 0: Algorytm_of_neighbour(); break;
                    case 1: Method_of_branches_and_borders(); break;
                    case 2: Algorythm_of_Dejkstra(); break;
                    case 3: Method_of_local_improvement(); break;
                    case 4:
                        {
                            for (int i = 0; i < sh.Count; i++)
                            {
                                sh[i].Next_index = -1;
                                sh[i].Draw_Line = false;
                            }
                            belongMatrixAntAlgorithm = Ant_algorithm(); 
                            break;
                        }
                    case 5: Method_K_average(); break;
                }
                Cut_lines();
                Make_file();
                pictureBox1.Invalidate();
                if (variant == 5)
                {
                  button_next_algorithm.Visible = false;
                  button_next_algorithm.Enabled = false;
                }
                variant++;
            }
        }

        private void JoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> s = File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_0.txt").ToList<string>();
            s.AddRange(File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_1.txt"));
            s.AddRange(File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_2.txt"));
            s.AddRange(File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_3.txt"));
            s.AddRange(File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_4.txt"));
            s.AddRange(File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster_5.txt"));
          s.Sort();
          for (int i = 0; i < s.Count; i++)
          {
              int j = 0;
              while (j < s[i].Length && s[i][j] != ' ') { j++; }
              s[i] = s[i].Substring(0, j);
          }
          StreamWriter sw = new StreamWriter(file0.Substring(0, file0.Length - 4) + "_сluster.txt");
          int sum = 0;
          for (int i = 1; i < s.Count; i++)
          {
            if (s[i] == s[i - 1])
            {
              sum++;
            }
            else
            {
              if (sum >= 2)
              {
                sw.WriteLine(s[i - 1]);
              }
              sum = 0;
            }
          }
          if (sum >= 2)
          {
            sw.WriteLine(s[s.Count - 1]);
          }
          s.Clear();
          sw.Close();
          string[] ss = File.ReadAllLines(file0.Substring(0, file0.Length - 4) + "_сluster.txt");
          for (int j = 0; j < sh.Count; j++)
          {
            sh[j].Draw_Line = false;
            sh[j].Next_index = -1;
          }
          int ind = -1;
          for (int i = 0; i < ss.Length; i++)
          {
            for (int j = 0; j < sh.Count; j++)
            {
              if (ss[i] == sh[j].Objid)
              {
                if (ind == -1) ind = j;
                else
                {
                  sh[ind].Next_index = j;
                  sh[ind].Draw_Line = true;
                  ind = j;
                }
                break;
              }
            }
          }
          Make_final_file(file0.Substring(0, file0.Length - 4) + "_Clusters.txt");
          pictureBox1.Invalidate();
        }

        private void NextAlgorithmToolStripMenuItem_Click(object sender, EventArgs e)
        {
          button_next_algorithm.PerformClick();
        }

        private void ReturnToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
          button_return_to_file.PerformClick();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            double min = double.Parse(MinText.Text), max = double.Parse(MaxText.Text);
            for(int i = 0; i < sh.Count(); i++)
            {
                if (sh[i].Redshift < min || sh[i].Redshift > max) sh.RemoveAt(i);
            }

        }

        private void MinCut_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MinText_TextChanged(object sender, EventArgs e)
        {

        }

        private void MaxText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

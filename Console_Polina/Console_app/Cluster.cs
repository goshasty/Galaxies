using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_app
{
    #region Galaxies

    #region Cluster

    public class Cluster
    {
        protected string File;
        public string file { get { return File; } set { File = value; } }
        protected List<Circle> Sh = new List<Circle>(); //List of galaxies
        public List<Circle> sh { get { return Sh; } set { Sh = value; } }
        protected List<List<double>> Length = new List<List<double>>();

        List<List<bool>> belongMatrixAntAlgorithm = new List<List<bool>>();

        public List<List<double>> length { get { return Length; } set { Length = value; } }
        protected double[] d;
        public double[] D { get { return d; } set { d = value; } }
        protected int[] Color;
        public int[] color { get { return Color; } set { Color = value; } }
        public const double inf = double.MaxValue;
        protected string File0;
        public string file0 { get { return File0; } set { File0 = value; } }
        protected int Variant;
        public int variant { get { return Variant; } set { Variant = value; } }
        protected ulong objid_of_Divide;
        public ulong Objid_of_Divide { get { return objid_of_Divide; } set { objid_of_Divide = value; } }
        protected bool Algorythmed = false;
        public bool algorythmed { get { return Algorythmed; } set { Algorythmed = value; } }
        public int ind_min = -1, ind_max = -1;
        public double[] parameters = { 0.35, 0.35, 0.35, 0.35 };
        double Merge_parameter = 0.05;

        public Cluster(string file)
        {
            this.file = file;
            variant = -1;
            file0 = file;
        }

        public void Finding_clusters()
        {
            Change_coordinates();
            FillDate(file);
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].Draw_Line = false;
            }
            algorythmed = true;
            Algorytm_of_neighbour();
            double[] cent_per = FindCenterAndPeriod();
            double z = cent_per[3];
            bool[,] matr = new bool[sh.Count, sh.Count];
            ulong[,] list = new ulong[11, 2];
            for (int i = 0; i < sh.Count; i++)
            {
                for (int j = 0; j < sh.Count; j++)
                {
                    matr[i, j] = false;
                }
            }
            for (int j = 0; j < 11; j++)
            {
                double min_r = double.MaxValue;
                int index = -1;
                for (int i = 0; i < sh.Count; i++)
                {
                    double r = Math.Pow((sh[i].Ra - cent_per[0]), 2) + Math.Pow((sh[i].Dec - cent_per[1]), 2) + Math.Pow((sh[i].Redshift * 10 - z * 10), 2);
                    if (r < min_r)
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
                    if (sh[i].Number_of_cluster == 0)
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
            for (int i = 0; i < 11; i++)
            {
                if ((int)list[i, 1] > sum_max)
                {
                    ind = i;
                    sum_max = (int)list[i, 1];
                }
            }
            Objid_of_Divide = list[ind, 0];
            int index_div = -1;
            for (int i = 0; i < sh.Count; i++)
            {
                if (ulong.Parse(sh[i].Objid) == Objid_of_Divide)
                {
                    index_div = i;
                }
                sh[i].Number_of_cluster = -1;
            }
            dfs(0, index_div, matr);
            double max_z = double.MinValue, min_z = double.MaxValue;
            for (int i = 0; i < sh.Count; i++)
            {
                if (sh[i].Number_of_cluster == 0)
                {
                    if (sh[i].Redshift < min_z) min_z = sh[i].Redshift;
                    if (sh[i].Redshift > max_z) max_z = sh[i].Redshift;
                }
            }
            for (int i = 0; i < sh.Count; i++)
            {
                if (sh[i].Redshift < min_z - 0.2 || sh[i].Redshift > max_z + 0.2)
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
            while (variant <= 5)
            {
                Next_algorithm();
            }
            Make_final_file(file0.Substring(0, file0.Length - 4) + "_Clusters.txt");
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
        }

        void Next_algorithm()
        {
            if (variant <= 5)
            {
                for (int i = 0; i < sh.Count; i++)
                {
                    sh[i].Number_of_cluster = -1;
                    sh[i].Used = false;
                }
                switch (variant)
                {
                    case 0: { Console.WriteLine("Алгоритм ближайшего соседа. \n Начало"); Algorytm_of_neighbour(); Console.WriteLine(" Конец"); } break;
                    case 1: { Console.WriteLine("Метод ветвей и границ. \n Начало"); Method_of_branches_and_borders(); Console.WriteLine(" Конец"); } break;
                    case 2: { Console.WriteLine("Алгорит Дейкстры. \n Начало"); Algorythm_of_Dejkstra(); Console.WriteLine(" Конец"); } break;
                    case 3: { Console.WriteLine("Метод локальных улучшений. \n Начало"); Method_of_local_improvement(); Console.WriteLine(" Конец"); } break;
                    case 4:
                        {
                            Console.WriteLine("Муравьиный алгоритм. \n Начало");

                            belongMatrixAntAlgorithm = Ant_algorithm();

                            Console.WriteLine(" Конец");

                        }
                        break;
                    case 5: { Console.WriteLine("Метод К средних. \n Начало"); Method_K_average(); Console.WriteLine(" Конец"); } break;
                }
                Cut_lines();
                Make_file();
                variant++;
            }
        }

        void Change_coordinates() //From sphere to 2D 
        {
            System.IO.File.Delete("Coordinates.txt");
            StreamWriter sw = new StreamWriter("Coordinates.txt");
            string[] s = System.IO.File.ReadAllLines(file);
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
                sw.WriteLine(x + " " + y + " " + z + " " + objid + " " + mas);
                sh.Add(new Circle(double.Parse(f), double.Parse(g), double.Parse(cr)));
            */
            #endregion
            }
            sw.Close();
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
                for (int i = 0; i < ways.Count; i++)
                {
                    if (i < parameters[variant] * ways.Count)
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
                if (sh[i].Redshift > z_max)
                {
                    z_max = sh[i].Redshift;
                    ind_max = i;
                }
                if (sh[i].Redshift < z_min)
                {
                    z_min = sh[i].Redshift;
                    ind_min = i;
                }
            }
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
            int[] length = { 0, 0, 0 };
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
                else if (length[1] < Sums[i, 3])
                {
                    index_large[2] = index_large[1];
                    length[2] = length[1];
                    index_large[1] = i;
                    length[1] = (int)Sums[i, 3];
                }
                else if (length[2] < Sums[i, 3])
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
                    if (indexx[t] == -1)
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

        List<List<bool>> Ant_algorithm()
        {
            AntAlghorithm a = new AntAlghorithm(length, sh);
            List<List<bool>> aa = a.Algorithm();
            for (int i = 0; i < aa.Count; i++)
            {
                for (int j = i; j < aa.Count; j++)
                {
                    if (aa[i][j])
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
            for (int i = 0; i < sh.Count; i++)
            {
                if (max_ra < sh[i].Ra) max_ra = sh[i].Ra;
                if (min_ra > sh[i].Ra) min_ra = sh[i].Ra;
                if (max_dec < sh[i].Dec) max_dec = sh[i].Dec;
                if (min_dec > sh[i].Dec) min_dec = sh[i].Dec;
                if (max_z < sh[i].Redshift) max_z = sh[i].Redshift;
                if (min_z > sh[i].Redshift) min_z = sh[i].Redshift;
            }
            double[] a = { (max_ra + min_ra) / 2.0, (max_dec + min_dec) / 2.0, (max_z - min_z) / 10.0, min_z, max_z };
            return a;
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

        void dfs(int num, int v, bool[,] matr)
        {
            if (sh[v].Number_of_cluster != -1) return;
            else
            {
                sh[v].Number_of_cluster = num;
                for (int i = 0; i < sh.Count; i++)
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
                    for (int j = 0; j < sh.Count; j++)
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
                    if (sum_max1 < t)
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
            for (int j = 0; j < sh.Count; j++)
            {
                if (sh[j].Number_of_cluster == ind)
                {
                    center_z += sh[j].Redshift * sh[j].Brightness;
                    count++;
                }
                if (sh[j].Number_of_cluster == ind1)
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
    }

    #endregion

    #region Ant_algorithm

    class AntAlghorithm
    {
        List<List<double>> length;
        List<Circle> sh;
        List<List<Pher>> ph;

        public AntAlghorithm(List<List<double>> length, List<Circle> sh)
        {
            this.length = length;
            this.sh = sh;
        }
        public List<List<bool>> Algorithm()
        {
            int i, j, step;
            double vision;
            int nextPoint, previousPoint;
            int countOfActive;
            double max, val;
            double closeness2;
            List<Ant> a;
            List<List<bool>> MatrixOfAdjacencies = new List<List<bool>>();
            #region Set of Coefficients
            //коэффициенты-параметры
            double pherFactor = 0.5, distanceFactor = 1; //коэффициенты приоритета
            double Q = 0; //откладываемый феромон
            for (i = 0; i < sh.Count; i++)
            {
                Q += length[0][i];
            }
            Q = Q / sh.Count;
            double startPher = Q / 10;
            const double p = 0.3; //коэффициент испарения
            int totalSteps; //раз исполняется алогритм
            totalSteps = sh.Count * 5;
            int inBan = (int)Math.Round(0.1 * sh.Count);

            //в функцию
            double maxLength = 0;
            double allLength = 0;
            double averageLength;
            for (i = 0; i < length.Count - 1; i++)
            {
                for (j = i + 1; j < length.Count; j++)
                {
                    allLength += length[i][j];
                    if (maxLength < length[i][j])
                        maxLength = length[i][j];
                }
            }

            averageLength = allLength / sh.Count / ((sh.Count - 1) / 2);
            closeness2 = sh.Count / Volume(averageLength);

            //

            #endregion

            #region Ant Algorithm
            ph = new List<List<Pher>>();
            a = new List<Ant>();
            for (i = 0; i < sh.Count; i++)
            {
                a.Add(new Ant(i));
                ph.Add(new List<Pher>());
                MatrixOfAdjacencies.Add(new List<bool>());
            }

            for (i = 0; i < ph.Count; i++)
                for (j = 0; j < a.Count; j++)
                {
                    MatrixOfAdjacencies[i].Add(false);
                    if (i == j)
                        ph[i].Add(new Pher(0));
                    else
                        ph[i].Add(new Pher(startPher)); //начальный феромон
                }
            for (step = 0; step < totalSteps; step++)//нужен выход!
            {
                for (i = 0; i < a.Count; i++)  //основной шаг для каждого муравья
                {
                    a[i].ban.Add(a[i].C); //в текущий идти нельзя
                    max = 0;
                    nextPoint = 0; //index of choosing point
                    for (j = 0; j < a.Count - 1; j++)
                    {
                        while (a[i].ban.Contains(j))
                            j++;
                        if (j >= a.Count)
                            break;
                        vision = 1 / length[i][j];
                        val = (Math.Pow((ph[i][j].V), pherFactor) + Math.Pow(vision, distanceFactor));
                        if (val > max)
                        {
                            max = val;
                            nextPoint = j;
                        } //выбрали лучший путь для муравья
                    }

                    previousPoint = a[i].C;
                    ph[nextPoint][previousPoint].numberOfUses = ph[previousPoint][nextPoint].numberOfUses += 1;
                    ph[nextPoint][previousPoint].I = ph[previousPoint][nextPoint].I += (Q / length[previousPoint][nextPoint]); //отложит ферамон
                    a[i].C = nextPoint;

                    if (step > inBan)
                        a[i].ban.RemoveAt(0);
                }

                for (i = 0; i < ph.Count; i++)
                    for (j = i + 1; j < a.Count - 1; j++)
                    {
                        if (i != j)  //изменения феромона на всех путях
                            ph[i][j].V = ph[j][i].V = ph[i][j].V * (1 - p) + ph[i][j].I;
                        ph[i][j].I = ph[j][i].I = 0;
                    }
            }

            //end of main algorithm
            //matrix of pheromon (ph) is ready to present result
            #endregion

            #region Choice Points

            int minNumberOfUsing;
            minNumberOfUsing = (int)(Math.Sqrt(totalSteps) * 1.25); //!!следить, где какой count указывать

            countOfActive = ChoiceEdge(minNumberOfUsing);
            // minNumberOfUsing = 50;
            // countOfActive = ChoiceEdge(minNumberOfUsing);

            for (i = 0; i < ph.Count - 1; i++)
            {
                for (j = i + 1; j < ph.Count; j++)
                {
                    MatrixOfAdjacencies[i][j] = ph[i][j].U;

                }
            }

            return MatrixOfAdjacencies;
            #endregion
        }
        private int ChoiceEdge(int minNumberOfUsing)
        {
            int res = 0;
            int i, j;
            for (i = 0; i < sh.Count; i++)
            {
                for (j = i + 1; j < sh.Count - 1; j++)
                {
                    if (ph[i][j].numberOfUses > minNumberOfUsing) //!!поправить, чтобы была согласованность в существительных
                    {
                        ph[i][j].U = ph[j][i].U = true;//!!определить, где работаем с верхнетреугольной, где со всей
                        res++;
                    }
                }
            }

            for (i = 0; i < sh.Count; i++)
            {
                res -= DeleteMinorPoint(i);
            }


            return res;
        }
        private int DeleteMinorPoint(int i)//неправильная процедура
        {
            int j;
            int count = 0;
            int totalDelete = 0;
            int minNumOfLinks = 4;
            if (sh.Count < 100)
                minNumOfLinks = 3;
            else
                minNumOfLinks = 4;

            for (j = 0; j < sh.Count; j++)
            {
                if (ph[i][j].U)
                    count++;
            }

            if (count < minNumOfLinks)
            {
                for (j = 0; j < sh.Count; j++)
                {
                    if (ph[i][j].U)
                    {
                        ph[i][j].U = ph[j][i].U = false;
                        totalDelete += DeleteMinorPoint(j);
                    }
                }
            }
            return totalDelete;
        }

        private double Volume(double averageLen)
        // relative volume
        {
            double vol = 0;
            double maxLen = 0;//distance to most remote point
            double currentLen = 0;
            double compactnessFactor; // averageLen / maxLen
            double x, y, z;//coordinates of center of mass
            x = y = z = 0;
            for (int i = 0; i < sh.Count; i++)
            {
                x += sh[i].X;
                y += sh[i].Y;
                z += sh[i].Z;
            }

            x = x / sh.Count;
            y = y / sh.Count;
            z = z / sh.Count;

            for (int i = 0; i < sh.Count; i++)
            {
                currentLen = Metrix(sh[i].X - x, sh[i].Y - y, sh[i].Z - z);
                if (currentLen > maxLen)
                    maxLen = currentLen;
            }
            compactnessFactor = averageLen / maxLen;
            vol = 4.0 / 3.0 * Math.PI * Math.Pow(compactnessFactor, 3);
            // volume is normorized because compactnessFactor < 1
            return vol;
        }

        private static double Metrix(double x, double y, double z)
        {
            double r;
            double kx = 2;
            double ky = 2;
            double kz = 2;
            r = Math.Sqrt(Math.Pow(x, kx) + Math.Pow(y, ky) + Math.Pow(y, kz));
            return r;
        }

    }
    #endregion

    #region Ant

    class Ant
    {
        int current; //где сейчас находится
        public List<int> ban;//память муравья - список запретов

        public int C
        {
            get { return current; }
            set { current = value; }
        }

        public Ant(int k)
        {
            current = k;
            ban = new List<int>();
        }

    }
    class Pher
    {
        public bool use; //рисуется или нет
        double value;
        double increment; //
        public double numberOfUses;
        public double V
        {
            get { return value; }
            set { this.value = value; }
        }
        public double I
        {
            get { return increment; }
            set { this.increment = value; }
        }

        public bool U { get { return use; } set { use = value; } }
        public Pher(double k)
        {
            value = k;
        }
    }

    #endregion

    #region Circle

    public class Circle
    {
        protected double x, y, z;
        static protected int radius;
        protected bool movement;
        protected int next_index;
        protected bool used;
        static protected int magnification;
        protected bool draw_line;
        protected string objid;
        public bool Draw_Line { get { return draw_line; } set { draw_line = value; } }
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double Z { get { return z; } set { z = value; } }
        public string Objid { get { return objid; } set { objid = value; } }
        static public int Magnification { get { return magnification; } set { magnification = value; } }
        static public int Radius { get { return radius; } set { radius = value; } }
        public bool Movement { get { return movement; } set { movement = value; } }
        public int Next_index { get { return next_index; } set { next_index = value; } }
        public bool Used { get { return used; } set { used = value; } }
        protected int number_of_cluster;
        public int Number_of_cluster { get { return number_of_cluster; } set { number_of_cluster = value; } }
        protected double mass;
        public double Mass { get { return mass; } set { mass = value; } }
        protected double brightness;
        public double Brightness { get { return brightness; } set { brightness = value; } }
        protected double ra, dec, redshift;
        public double Ra { get { return ra; } set { ra = value; } }
        public double Dec { get { return dec; } set { dec = value; } }
        public double Redshift { get { return redshift; } set { redshift = value; } }

        public Circle(double ra, double dec, double redshift)
        {
            movement = false;
            next_index = -1;
            used = false;
            number_of_cluster = -1;
            this.ra = ra;
            this.dec = dec;
            this.redshift = redshift;
        }
        public bool Pointing(int X_mouse, int Y_mouse)
        {
            return (Math.Pow(X_mouse - x * magnification, 2) + Math.Pow(Y_mouse - y * magnification, 2) <= Math.Pow(radius, 2));
        }
    }

    class Point3D
    {
        public int x, y, z;
        public double phi, r, r1, alpha;
        public Point3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = Math.Sqrt((double)x * x + y * y);
            this.r1 = Math.Sqrt((double)y * y + z * z);
            this.phi = Math.Atan2(y, x);
            this.alpha = Math.Atan2(y, z);
        }
    }

    #endregion

    #endregion
}

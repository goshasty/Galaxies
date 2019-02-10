using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxies
{
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

            averageLength = allLength / sh.Count / ((sh.Count - 1)/2);
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
            minNumberOfUsing = (int)(Math.Sqrt(totalSteps)*1.25); //!!следить, где какой count указывать

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
            double currentLen=0;
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
}

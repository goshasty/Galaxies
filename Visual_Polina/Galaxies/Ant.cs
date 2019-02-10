using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxies
{
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

}

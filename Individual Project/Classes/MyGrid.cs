using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Individual_Project.Classes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Threading;

namespace Individual_Project.Classes
{
    [Serializable]
   public class MyGrid
    {
       public Grid grid {get; set;}
        public List<List<Cell>> gridCells = new List<List<Cell>>();
        public int Neighbourhood {get; set;}

        public MyGrid()
        {
            grid = new Grid();
            gridCells = new List<List<Cell>>();
            Neighbourhood = 8;
        }
    }
}

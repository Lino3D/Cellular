using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project.Classes
{
    [Serializable]
  public class Cell
    {
       public int _state;
        public int _x;
        public int _y;
        public int _newstate;
        public  List<Cell> Neighbors = new List<Cell>();
       public List<List<Cell>> Neighs = new List<List<Cell>>();
             public int x
        {
            get { return _x; }
            set { _x = value; }
        }
      
        public int y
        {
            get { return _y; }
            set { _y = value; }
        }
      
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        public int newState
        {
            get { return _newstate; }
            set { _newstate = value; }
        }
      public Cell(int X, int Y)
        {
            x = X;
            y = Y;
        }
      public Cell()
      {
  
      }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project.Classes
{
    [Serializable]
   public  class Rule
    {
     public string Name {get; set;}
     public  List<List<Cell>> RuleCells = new List<List<Cell>>();
     public   int MainState {get; set;}
     public int NewState { get; set; }
     public int counter { get; set; }
     public int Neighbourhood { get; set; }
    public bool OnlyNumber  { get; set; }
    public int CounterWhite { get; set; }
    public int CounterBlack { get; set; }
    public int CounterBlackMax { get; set; }
    }
}

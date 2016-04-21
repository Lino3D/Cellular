using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project.Classes
{
    [Serializable]
  public  class ExportClass
    {
     public   List<Rule> Rules = new List<Rule>();
     public   List<List<Cell>> Cells = new List<List<Cell>>();
    }
}

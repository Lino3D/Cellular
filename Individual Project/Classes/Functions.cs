using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Individual_Project.Classes
{
  public static  class Functions
    {
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }


        }
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

      public static List<List<Cell>> PrepareExport(MyGrid g)
      {
            List<List<Cell>> list = new List<List<Cell>>();

          for (int i = 0; i < g.gridCells.Count; i++)
          {
              for (int j = 0; j < g.gridCells[i].Count; j++)
              {
                  g.gridCells[i][j].Neighs.Clear();
                  g.gridCells[i][j].Neighbors.Clear();
              }
          }
          list = g.gridCells;
          return list;
      }


    }
}

using Individual_Project.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Individual_Project
{
    /// <summary>
    /// Interaction logic for RulesetEditor.xaml
    /// </summary>
    public partial class RulesetEditor : Window
    {
        List<Rule> AllRules = new List<Rule>();
        List<Rule> ActiveRules = new List<Rule>();
        List<Rule> Socket = new List<Rule>();
        List<Rule> Socket2 = new List<Rule>();

    //    List<Rule> previousActive = new List<Rule>();

        TreeViewItem Items4;
        TreeViewItem Items8;
        TreeViewItem Items24;

        public RulesetEditor(List<Rule> all, List<Rule>active, TreeViewItem it4, TreeViewItem it8, TreeViewItem it24)
        {
            AllRules=all;
            InitializeComponent();

            foreach (Rule rule in AllRules)
            {
                AllRulesGrid.Items.Add(rule);
            }
            Items4 = it4;
            Items8 = it8;
            Items24 = it24;

            foreach (Rule rule in active)
            {
               ActiveRulesGrid.Items.Add(rule);
            }

           ActiveRules= active;

        }



        private void AllRules_SelectionChanged(object sender,
	    SelectionChangedEventArgs e)
	{
        Socket.Clear();
	    // ... Get SelectedItems from DataGrid.
	    var grid = sender as DataGrid;
	    var selected = grid.SelectedItems;

	    foreach (var item in selected)
	    {
		var Rule = item as Rule;
        Socket.Add(Rule);
	    }

	  
	}
    


        private void AllRules_CellEditEnding(object sender,
    DataGridCellEditEndingEventArgs e)
        {
            MessageBox.Show("EDIT HERE");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            foreach(Rule rule in Socket)
            {
              

                List<Rule> tmp = new List<Rule>();
                var items = ActiveRulesGrid.Items;
                foreach (var item in items)
                {
                    var Rule = item as Rule;
                    tmp.Add(Rule);
                }
                Rule result = tmp.Find(x => x.Name == rule.Name);
                if (result == null)
                {
                 //   ActiveRules.Add(rule);
                    ActiveRulesGrid.Items.Add(rule);
                }
            }
    
        }

        private void ActiveRulesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Socket2.Clear();
            var grid = sender as DataGrid;
            var selected = grid.SelectedItems;
            foreach (var item in selected)
            {
                var Rule = item as Rule;
                Socket2.Add(Rule);
            }
        
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            foreach(Rule r in Socket2.ToList())
            {
                ActiveRulesGrid.Items.Remove(r);
              Rule result = ActiveRules.Find(x => x.Name == r.Name);
               if(result!=null)
                {
              //     ActiveRules.Remove(result);
                }
            }
            int z = 0;
            z++;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if(Socket.Count==1)
            {
                Rule rule = Socket[0];
                string oName = rule.Name;
             //   Rule result = AllRules.Find(x => x.Name == rule.Name);
                AddRuleWindow AddWindow = new AddRuleWindow(rule, AllRules, Items4, Items8, Items24);
                AddWindow.ShowDialog();

              //  rule = ActiveRulesGrid.SelectedItem as Rule;
           int i=     ActiveRulesGrid.Items.IndexOf(rule);
           ActiveRulesGrid.Items.Refresh();
           AllRulesGrid.Items.Refresh();

            
            }

            else if(Socket.Count>1)
            {
                MessageBox.Show("Please Edit one rule at a time");
            }
            else if(Socket.Count==0)
            {
                MessageBox.Show("Please Select a single rule to edit");
            }

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            ActiveRules.Clear();
            var items = ActiveRulesGrid.Items;
              foreach (var item in items)
              {
                  var Rule = item as Rule;
                  ActiveRules.Add(Rule);
              }

            Items4.Items.Clear();
            Items8.Items.Clear();
            Items24.Items.Clear();

            foreach(Rule Rule in ActiveRules)
            {

                 TreeViewItem x = AddRuleWindow.GetTreeView(Rule.Name);
                if (Rule.Neighbourhood == 4)
                {
                    Items4.Items.Add(x);
                }
                else if (Rule.Neighbourhood == 8)
                    Items8.Items.Add(x);
                else if (Rule.Neighbourhood == 24)
                    Items24.Items.Add(x);
            }

    
                this.Close();
        } 

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportClass Export = new ExportClass();
            Export.Rules = AllRules;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                Functions.WriteToXmlFile<ExportClass>(saveFileDialog.FileName, Export);
       
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {

            ExportClass Imported;
            //List<Person> people = XmlSerialization.ReadFromXmlFile<List<Person>>("C:\people.txt");


               OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.Filter = "Text file (*.txt)|*.txt";
             if (openFileDialog.ShowDialog() == true) 
             {
                 try { 
                 Imported= Functions.ReadFromXmlFile<ExportClass>(openFileDialog.FileName);
          
                 foreach(Rule rule in Imported.Rules)
                 {
                     Rule result = AllRules.Find(x => x.Name == rule.Name);
                     if (result == null)
                     {
                       AllRules.Add(rule);
                        AllRulesGrid.Items.Add(rule);
                     }
                 }
                     }
                 catch(Exception a)
                 {
                     MessageBox.Show("This file does not contain proper rule data");
                 }
             }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


    }
}

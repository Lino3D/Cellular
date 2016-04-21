using Individual_Project.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddRuleWindow.xaml
    /// </summary>
    /// 
    
    public partial class AddRuleWindow : Window
    {
        int A=3;
        int B=3;
        int Z = 0;
        Rule Rule = new Rule();

        List<Rule> Rules = new List<Rule>();
        List<List<Cell>> RuleCells = new List<List<Cell>>();
        int Counter=0;
        int MainState;
        int NewState;
        TreeViewItem Items4;
        TreeViewItem Items8;
        TreeViewItem Items24;
        int neighbourhood =8;
        int BrushState;
        Grid RuleGrid;
        string OriginialName;

        bool OnlyNumber = false;
        int CounterWhite =0;
        int CounterBlack =0;
        int CounterMax = 0;




        public AddRuleWindow(List<Rule> rules, TreeViewItem it4, TreeViewItem it8, TreeViewItem it24)
        {

            InitializeComponent();
            Rules = rules; //przekazywanie parametru;
            Items4 = it4;
            Items8 = it8;
            Items24 = it24;
            NCombo.SelectedValue = "8 Point";
            MainStateCombo.SelectedValue = "Live";
            CreateNGrid(A,B);
        }
        public AddRuleWindow(Rule rule, List<Rule> rules, TreeViewItem it4, TreeViewItem it8, TreeViewItem it24)
        {

            InitializeComponent();
            Rules = rules;
            Rule = rule;

            LoadRule();
            Items4 = it4;
            Items8 = it8;
            Items24 = it24;

           

        }
        public AddRuleWindow(Rule rule, List<Rule> rules)
        {

            InitializeComponent();
            Rules = rules;
            Rule = rule;
            LoadRule();
        }

        public void LoadRule()
    {
      if(Rule.Neighbourhood==4)
      {
          Z=1;
          CreateNGrid(3, 3);
          NCombo.SelectedValue = "4 Point";
      }
      else if(Rule.Neighbourhood==8)
      {
          Z = 0;
          CreateNGrid(3, 3);
          NCombo.SelectedValue = "8 Point";
      }
      else if(Rule.Neighbourhood==24)
      {
          Z = 0;
          CreateNGrid(5, 5);
          NCombo.SelectedValue = "24 Point";
      }





      RuleCells = Rule.RuleCells;
      NameBox.Text = Rule.Name;
      MainState = Rule.MainState;
      NewState = Rule.NewState;
      Counter = Rule.counter;
      neighbourhood = Rule.Neighbourhood;
      OriginialName = Rule.Name;
      OnlyNumber = Rule.OnlyNumber;
      PaintGrid(RuleGrid);
      CounterBlack = Rule.CounterBlack;
      CounterMax = Rule.CounterBlackMax;
      Black.Text = CounterBlack.ToString();
      BlackMax.Text = CounterMax.ToString();


         if (OnlyNumber == false)
      {
          PositionTooBox.IsChecked = true;
          NumberOnlyBox.IsChecked = false;
      }
      else if (OnlyNumber == true)
      {
          PositionTooBox.IsChecked = false;
          NumberOnlyBox.IsChecked = true;
      }
            if(NewState==1)
            {
                MainStateCombo.SelectedValue = "Live";
            }
            else if(NewState==0)
            {
                MainStateCombo.SelectedValue = "Die";
            }



      //Rule.RuleCells = RuleCells;
      //Rule.Name = NameBox.Text;
      //Rule.MainState = MainState;
      //Rule.NewState = NewState;
      //Rule.counter = Counter;
      //Rule.Neighbourhood = neighbourhood;
    

    }
        private void PaintGrid(Grid m)
        {
            SolidColorBrush _black = new SolidColorBrush(Colors.Black);
            SolidColorBrush _white = new SolidColorBrush(Colors.White);
            SolidColorBrush _gray = new SolidColorBrush(Colors.Gray);
            foreach (Control ctrl in m.Children)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    //   ctrl.
                    int rowIndex = System.Windows.Controls.Grid.GetRow(ctrl);
                    int columnIndex = System.Windows.Controls.Grid.GetColumn(ctrl);
                    SolidColorBrush c = new SolidColorBrush(Colors.Black);
                    Label tb = ctrl as Label;


                    Cell cell =RuleCells[rowIndex][columnIndex];
                    if (cell.State == 1)
                        tb.Background = _black;
                    else if (cell.State == 0)
                        tb.Background = _gray;
                    else if (cell.State == 3)
                        tb.Background = _white;
                }

            }
        }


        public void CreateNGrid(int x, int y)
        {

            A = x;
            
            //repaint 
            int intTotalChildren = MainGrid.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter > 0; intCounter--)
            {
                if (MainGrid.Children[intCounter].GetType() == typeof(Grid))
                {
                 Grid ucCurrentChild = (Grid)MainGrid.Children[intCounter];
                    MainGrid.Children.Remove(ucCurrentChild);
                }
            }
            //Clear the List
            RuleCells.Clear();
            Grid NGrid = new Grid();
            NGrid.Width = Double.NaN;
            NGrid.Height = Double.NaN;
            NGrid.HorizontalAlignment = HorizontalAlignment.Left;
            NGrid.VerticalAlignment = VerticalAlignment.Top;
            NGrid.SetValue(Grid.RowProperty, 1);
            NGrid.SetValue(Grid.ColumnProperty, 1);
            NGrid.SetValue(Grid.RowSpanProperty, 2);

                 for (int i = 0; i < y; i++)
            {

                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = GridLength.Auto;
               NGrid.ColumnDefinitions.Add(gridCol);

            }

            for (int i = 0; i < x; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                NGrid.RowDefinitions.Add(rowdef);
            }
       
            for (int i = 0; i < x; i++)
            {
                List<Cell> sublist = new List<Cell>();
                for (int j = 0; j < y; j++)
                {
                    Label Box = new Label();

                    if (Z==1)
                    {
                        if (i == 0 && j == 0 || i == 0 && j == 2 || i == 2 && j == 0 || i == 2 && j == 2)
                        {
                            Box.Visibility = Visibility.Hidden;
                            Box.IsEnabled = false;
                        }
                    }
   

                    //Middle one is off the charts.
                if((i==(x*2)/5 && j==(y*2)/5))
                {
                    Box.IsEnabled = false;
                }


                    Box.Width = 120 / x;
                    Box.Height = 120 / y;
                    Box.Name = "Y" + i.ToString() + "Y" + j.ToString();

                    object o = MainGrid.FindName(Box.Name);
                    if (o != null)
                    {
                        MainGrid.UnregisterName(Box.Name);
                    }
                    MainGrid.RegisterName(Box.Name, Box);


                  //  if (neighbourhood != 4)
                    {
                        Cell cell = new Cell(i, j);
                        cell.State = 3;
                        sublist.Add(cell);
                    }
               
           
                   Box.MouseDown += new MouseButtonEventHandler(Cell_Click);
                    Box.BorderBrush = new SolidColorBrush(Colors.Gray);
                    Box.BorderThickness = new Thickness(1);
                    Box.FontSize = 14;
                    Box.FontWeight = FontWeights.Bold;
                    Box.Foreground = new SolidColorBrush(Colors.Black);
                    Box.VerticalAlignment = VerticalAlignment.Top;
                    Grid.SetRow(Box, i);
                    Grid.SetColumn(Box, j);
                    NGrid.Children.Add(Box);
                }
                RuleCells.Add(sublist);
            }


            RuleGrid = NGrid;
            MainGrid.Children.Add(NGrid);
  
    }
        private void Cell_Click(object sender, MouseButtonEventArgs e)
        {

            var label = sender as Label;


            string s = (label).Name;
            string[] words = s.Split('Y');
            int x = int.Parse(words[1]);
            int y = int.Parse(words[2]);
       
 //we paint black
            if (BrushState == 1)
            {
                if (RuleCells[x][y].State == 3) //if nothing was selected yet
                {
                  SolidColorBrush c = new SolidColorBrush(Colors.Black);
                    label.Background = c;
                    RuleCells[x][y].State = 1;
                    Counter++;
                    CounterBlack++;
                    CounterWhite--;
                }
                else if (RuleCells[x][y].State == 1)// if it was black
                {
                    SolidColorBrush c = new SolidColorBrush(Colors.White);
                    label.Background = c;
                    RuleCells[x][y].State = 3;
                    Counter--;
                    CounterBlack--;
                    CounterWhite++;
                }
                else if (RuleCells[x][y].State == 0) //if it was white
                {
                    SolidColorBrush c = new SolidColorBrush(Colors.Black);
                    label.Background = c;
                    RuleCells[x][y].State = 1;
                    CounterWhite--;
                    CounterBlack++;
                   // Counter++;
                }

            }
            else //paint white
            {
                if (RuleCells[x][y].State == 3)
                {
                    SolidColorBrush c = new SolidColorBrush(Colors.Gray);
                    label.Background = c;
                    RuleCells[x][y].State = 0;
                    Counter++;
               //     CounterWhite++;
                }
                else if (RuleCells[x][y].State == 1)
                {
                    SolidColorBrush c = new SolidColorBrush(Colors.Gray);
                    label.Background = c;
                    RuleCells[x][y].State = 0;
                   CounterWhite++;
                    CounterBlack--;
                   // Counter++;
                }
                else if(RuleCells[x][y].State==0)
                {
                    SolidColorBrush c = new SolidColorBrush(Colors.White);
                    label.Background = c;
                    RuleCells[x][y].State = 3;
                    Counter--;
               //     CounterWhite--;
                }

            }
            if (CounterMax < CounterBlack)
                CounterMax = CounterBlack;
            Black.Text = CounterBlack.ToString();
            BlackMax.Text = CounterMax.ToString();
            White.Text = CounterWhite.ToString();
          
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            int value;
            bool isNumeric = int.TryParse(BlackMax.Text, out value);
            if (isNumeric == true)
            {

                if (value > neighbourhood)
                {
                    MessageBox.Show("Maximum number of active neighbours cannot be larger than the neighbour space");
                    CounterMax = CounterBlack;
                    BlackMax.Text = CounterMax.ToString();
                }
                else if (value < CounterBlack)
                {
                    MessageBox.Show("Maximum number of active neighbours cannot be smaller than minimum number of active neighbours");
                    CounterMax = CounterBlack;
                    BlackMax.Text = CounterMax.ToString();

                }
                else if (value >= CounterBlack)
                {
                    CounterMax = value;
                    BlackMax.Text = CounterMax.ToString();

                    if (NameBox.Text.Length==0)
                        NameBox.Text = "Name";



                    //creating a rule.
                    Rule.RuleCells = RuleCells;
                    Rule.MainState = MainState;
                    Rule.NewState = NewState;
                    Rule.counter = Counter;
                    Rule.Neighbourhood = neighbourhood;
                    Rule.CounterBlack = CounterBlack;
                    Rule.CounterWhite = CounterWhite;
                    Rule.CounterBlackMax = CounterMax;
                    Rule.OnlyNumber = OnlyNumber;
                    Rule result = Rules.Find(z => z.Name == OriginialName);

                    Rule.Name = NameBox.Text.Replace(" ", "");

                    if (result == null)
                    {
                        result = Rules.Find(z => z.Name == Rule.Name);
                        if (result == null)
                        {
                            Rules.Add(Rule);
                            TreeViewItem x = GetTreeView(Rule.Name);
                            if (Rule.Neighbourhood == 4)
                            {
                                Items4.Items.Add(x);
                            }
                            else if (Rule.Neighbourhood == 8)
                                Items8.Items.Add(x);
                            else if (Rule.Neighbourhood == 24)
                                Items24.Items.Add(x);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Rule with name " + Rule.Name + " already exists.");
                        }

                    }
                    //changing existing.
                    else
                    {
                        result = Rule;
                       //  if (Rule.Neighbourhood == 4)
                        {
                            foreach (object obj in Items4.Items)
                            {

                                TreeViewItem TVI = Items4.ItemContainerGenerator.ContainerFromItem(obj) as TreeViewItem;
                                if (TVI != null)
                                    if (TVI.Header.ToString().ToLower() == OriginialName.ToLower())
                                    {

                                        if (Rule.Neighbourhood != 4)
                                        {
                                            Items4.Items.Remove(TVI);
                                            if (Rule.Neighbourhood == 24)
                                            {
                                                TVI.Header = Rule.Name;
                                                Items24.Items.Add(obj);
                                            }
                                            else
                                            {
                                                TVI.Header = Rule.Name;
                                                Items8.Items.Add(obj);
                                            }
                                            break;
                                        }
                                        else
                                            TVI.Header = Rule.Name;
                                    }
                            }
                        }
                //         else if (Rule.Neighbourhood == 8)
                        {

                            foreach (object obj in Items8.Items)
                            {
                                TreeViewItem TVI = Items8.ItemContainerGenerator.ContainerFromItem(obj) as TreeViewItem;
                                if (TVI != null)
                                    if (TVI.Header.ToString().ToLower() == OriginialName.ToLower())
                                    {
                                        if (Rule.Neighbourhood != 8)
                                        {
                                            Items8.Items.Remove(TVI);
                                            if (Rule.Neighbourhood == 24)
                                            {
                                                TVI.Header = Rule.Name;
                                                Items24.Items.Add(obj);
                                            }
                                            else
                                            {
                                                TVI.Header = Rule.Name;
                                                Items4.Items.Add(obj);
                                            }
                                            break;
                                        }
                                        else
                                            TVI.Header = Rule.Name;
                                    }

                            }
                        }
                       //   else if (Rule.Neighbourhood == 24)
                        {
                            foreach (object obj in Items24.Items)
                            {

                                TreeViewItem TVI = Items24.ItemContainerGenerator.ContainerFromItem(obj) as TreeViewItem;
                                if (TVI != null)
                                    if (TVI.Header.ToString().ToLower() == OriginialName.ToLower())
                                    {
                                        if (Rule.Neighbourhood != 24)
                                        {
                                            Items24.Items.Remove(TVI);
                                            if (Rule.Neighbourhood == 8)
                                            {
                                                TVI.Header = Rule.Name;
                                                Items8.Items.Add(obj);
                                            }
                                            else
                                            {
                                                TVI.Header = Rule.Name;
                                                Items4.Items.Add(obj);
                                            }
                                            break;
                                        }
                                        else

                                            TVI.Header = Rule.Name;
                                    }
                            }
                        }
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Maximum number of active neighbours has to be a number");
                CounterMax = CounterBlack;
                BlackMax.Text = CounterMax.ToString();
            }

        }


        private void NeighbourhoodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
    
            string selected = ((ComboBoxItem)NCombo.SelectedItem).Content.ToString();
            if (selected == "4 Point")
            {
               //CreateNGrid()
                neighbourhood = 4;
                Counter = 0;
                Z = 1;
                CreateNGrid(3, 3); //1 1
                CounterWhite = 4;
                CounterBlack = 0;
                CounterMax = 0;
                White.Text = CounterWhite.ToString();

               
            }
            if (selected=="8 Point")
            {
                neighbourhood = 8;
                Counter = 0;
                Z = 0;
                CreateNGrid(3, 3);
                CounterWhite = 8;
                White.Text = CounterWhite.ToString();
                CounterBlack = 0;
                CounterMax = 0;
           

            }
            if (selected == "24 Point") 
            {
                neighbourhood = 24;
                Counter = 0;
                Z = 0;
                CreateNGrid(5, 5); //2/2
                CounterWhite = 24;
                White.Text = CounterWhite.ToString();
                CounterBlack = 0;
                CounterMax = 0;
             
            }
            Black.Text = CounterBlack.ToString();
            BlackMax.Text = CounterMax.ToString();
        }

        private void MainStateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   string selected = ((ComboBoxItem)NCombo.SelectedItem).Content.ToString();
            int selected  = MainStateCombo.SelectedIndex;
            if (selected == 0) //Live
            {
                MainState = 0;
                NewState = 1;
                if(A==3)
                {
                 //   RuleCells[1][1].State = MainState;
                   // RuleCells[1][1].newState = NewState;
                }
                else
                {
               //     RuleCells[3][3].State = MainState;
                  //  RuleCells[3][3].newState = NewState;
                }

            }
            if (selected == 1) //Die
            {
               MainState= 1;
               NewState = 0;
               if (A == 3)
               {
                //   RuleCells[1][1].State = MainState;
                //   RuleCells[1][1].newState = NewState;
               }
               else
               {
                 //  RuleCells[3][3].State = MainState;
                 //  RuleCells[3][3].newState = NewState;
               }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       public static TreeViewItem GetTreeView(string name)
        {
            name = name.Replace(" ", "");
            TreeViewItem item = new TreeViewItem();
            item.Header = name;
            item.Name = name;
            item.ToolTip = "Rule";
            return item;
        }

        private void BrushCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = BrushCombo.SelectedIndex;
            if(selected==0)
            {
                BrushState = 1;
            }
            else
            {
                BrushState = 2;
            }
        }

        private void OnlyNumber_Checked(object sender, RoutedEventArgs e)
        {
            OnlyNumber = true;
           if(BlackMax!=null)
           {
               BlackMax.IsReadOnly = false; ;
           }
        }

        private void PositionAndNumber_Checked(object sender, RoutedEventArgs e)
        {
            OnlyNumber = false;
            if (BlackMax != null)
            {
                BlackMax.IsReadOnly = true;
                CounterMax = CounterBlack;
                BlackMax.Text = CounterMax.ToString();
            }

        }

        private void BlackMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int value;
            //bool isNumeric = int.TryParse(BlackMax.Text, out value);
            //if (isNumeric == true)
            //{

            //    if (value > neighbourhood)
            //    {
            //        MessageBox.Show("Maximum number of active neighbours cannot be larger than the neighbour space");
            //        CounterMax = CounterBlack;
            //        BlackMax.Text = CounterMax.ToString();
            //    }
            //    else if (value < CounterBlack)
            //    {
            //        MessageBox.Show("Maximum number of active neighbours cannot be smaller than minimum number of active neighbours");
            //        CounterMax = CounterBlack;
            //        BlackMax.Text = CounterMax.ToString();

            //    }
            //    else if (value >= CounterBlack)
            //    {
            //        CounterMax = value;
            //        BlackMax.Text = CounterMax.ToString();
            //    }

            //}
            //else
            //{
            //    MessageBox.Show("Maximum number of active neighbours has to be a number");
            //    CounterMax = CounterBlack;
            //    BlackMax.Text = CounterMax.ToString();
            //}
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.A) || (e.Key > Key.Z))
                e.Handled = true;
        }
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

    }
}

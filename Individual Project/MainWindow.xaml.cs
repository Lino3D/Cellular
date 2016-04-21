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
using System.Xml.Serialization;
using Microsoft.Win32;
using MahApps.Metro.Controls;


namespace Individual_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
      //  public int x =30;
      //  public int y = 30;
          //    List<List<Cell>> cells = new List<List<Cell>>();
        int kernel = 1;
        DispatcherTimer timer = new DispatcherTimer();
        int T = 250;
        bool started = false;
        int N = 2;
     
        int GridsN = 1;
        int CurrentGrid = 1;

        List<MyGrid> Grids = new List<MyGrid>();
        //List of Xs of Grids
        public List<int> Xs = new List<int>();
        public List<int> Ys = new List<int>();
   //     public List<Grid> Grids = new List<Grid>();

       public  List<Rule> AllRules = new List<Rule>();
       public List<Rule> ActiveRules = new List<Rule>();

       bool custom = true;

       int Height =300;
       int Width =600;


        public MainWindow()
        {
            InitializeComponent();
            Xs.Add(20);
            Ys.Add(20);
            CreateGrid(TabItem1, Xs[0], Ys[0], 0);
         //   NCombo.SelectedValue = "8 Point";
            timer.Interval = TimeSpan.FromMilliseconds(T);
            timer.Tick += new EventHandler(Timer_Tick);

        }
     



        //funkcja do tworzenia zakładki o nazwie name;
        TabItem CreateTab(String name)
        {
            TabItem tabitem = new TabItem();
            tabitem.Header = name;
            tabitem.Name = name;
            MyTab.Items.Add(tabitem);
            return tabitem;
        }





        private async void Timer_Tick(object sender, EventArgs e)
        {
            MyGrid mg = Grids[CurrentGrid];
          await  Step(Xs[CurrentGrid], Ys[CurrentGrid], mg);
            RepaintGrid(mg);
        }

        //   <Grid x:Name="MatrixGrid" Margin="10" RenderTransformOrigin="2.564,0.823" Grid.Column="1" Grid.Row="3" Grid.RowSpan="2"  />



        //dodaje grida do Tabu o nazwie name.
        public void CreateGrid(TabItem tab, int x, int y, int GridNumber)
        {

            Grid MatrixGrid = new Grid();
            MatrixGrid.Name=tab.Name;
            MatrixGrid.Width = Double.NaN;
            MatrixGrid.Height = Double.NaN;
            MatrixGrid.HorizontalAlignment = HorizontalAlignment.Left;
            MatrixGrid.VerticalAlignment = VerticalAlignment.Top;
            MatrixGrid.SetValue(Grid.RowProperty, 3);
            MatrixGrid.SetValue(Grid.ColumnProperty, 1);
            MatrixGrid.SetValue(Grid.RowSpanProperty, 2);
            //  MatrixGrid.Margin = new Thickness(30, 30, 30, 30);

            List<List<Cell>> cells = new List<List<Cell>>();
            for (int i = 0; i < y; i++)
            {

                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = GridLength.Auto;
                MatrixGrid.ColumnDefinitions.Add(gridCol);

            }

            for (int i = 0; i < x; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                MatrixGrid.RowDefinitions.Add(rowdef);
            }


            for (int i = 0; i < x; i++)
            {
                List<Cell> sublist = new List<Cell>();
                for (int j = 0; j < y; j++)
                {
                    Label Box = new Label();
                    Box.Name = "Y" + i.ToString() + "Y" + j.ToString();

                    if (x == 0)
                        x = 1;
                    if (y==0)
                        y=1;

                 //   if (x <= 30)
                        Box.Height = Height / x;
                //    if (y <= 35)
                        Box.Width = Width / y;

                    object o = MainGrid.FindName(Box.Name);
                    if (o != null)
                    {
                        MainGrid.UnregisterName(Box.Name);
                    }
                    MainGrid.RegisterName(Box.Name, Box);
                    Cell cell = new Cell(i, j);
                    cell.State = 0;
                    sublist.Add(cell);
                    SolidColorBrush w = new SolidColorBrush(Colors.White);
                    Box.Background = w;
                    Box.MouseDown += new MouseButtonEventHandler(Cell_Click);
                    Box.BorderBrush = new SolidColorBrush(Colors.Gray);
                    Box.BorderThickness = new Thickness(1);
                    Box.FontSize = 14;
                    Box.FontWeight = FontWeights.Bold;
                    Box.Foreground = new SolidColorBrush(Colors.Black);
                    Box.VerticalAlignment = VerticalAlignment.Top;
                     Grid.SetRow(Box, i);
                      Grid.SetColumn(Box, j);
                    MatrixGrid.Children.Add(Box);
                }
                cells.Add(sublist);
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Cell cell = cells[i][j];
                    for (int filterX = 0 - kernel; filterX <= 3 - 1 - kernel; filterX++)
                    {
                        List<Cell> sublist = new List<Cell>();
                        for (int filterY = 0 - kernel; filterY <= 3 - 1 - kernel; filterY++)
                        {
                            int a = i + filterX;
                            int b = j + filterY;
                           
                       
                            if (a >= 0 && a < x && b >= 0 && b < y)
                            {
                               // if (a != i || b != j)
                                {
                                    cell.Neighbors.Add(cells[a][b]);
                                 //   c.State = cells[a][b].State;
                                    sublist.Add(cells[a][b]);
                                }
                               
                            }
                        }
                        cell.Neighs.Add(sublist);
                    }

                }

            }

           {
               MyGrid mg = new MyGrid();
               mg.grid = MatrixGrid;
               mg.gridCells=cells;
               mg.Neighbourhood = 8;
               //   MainGrid.Children.Add(MatrixGrid);
               Grids.Add(mg);
               ScrollViewer scroller = new ScrollViewer();
               scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
               scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
               scroller.Content = MatrixGrid;
               tab.Content = scroller;
               

               //  name.Content = scroller;
                  }

        }

        private void Neighbourhood_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selected = ((ComboBoxItem)NCombo.SelectedItem).Content.ToString();
            if (selected == "4 Point")
            {
                //CreateNGrid()
                kernel = 1;
                ChangeNeighbourhood( Xs[CurrentGrid], Ys[CurrentGrid],Grids[CurrentGrid]);
                Grids[CurrentGrid].Neighbourhood = 4;
            }
            if (selected == "8 Point")
            {
                kernel = 1;
                ChangeNeighbourhood(Xs[CurrentGrid], Ys[CurrentGrid],Grids[CurrentGrid]);
                Grids[CurrentGrid].Neighbourhood = 8;

            }
            if (selected == "24 Point")
            {
                kernel = 2;
                ChangeNeighbourhood(Xs[CurrentGrid], Ys[CurrentGrid],Grids[CurrentGrid]);
                Grids[CurrentGrid].Neighbourhood = 24;
            }
        }




        void ChangeNeighbourhood(int x, int y, MyGrid mg)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Cell cell = mg.gridCells[i][j];
                    cell.Neighs.Clear();
                    cell.Neighbors.Clear();
                }
            }
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        Cell cell = mg.gridCells[i][j];
                        for (int filterX = 0 - kernel; filterX <= 0+kernel; filterX++)
                        {
                            List<Cell> sublist = new List<Cell>();
                            for (int filterY = 0 - kernel; filterY <= kernel; filterY++)
                            {
                                int a = i + filterX;
                                int b = j + filterY;


                                if (a >= 0 && a < x && b >= 0 && b < y)
                                {
                         
                                    {
                                        cell.Neighbors.Add(mg.gridCells[a][b]);
                                        sublist.Add(mg.gridCells[a][b]);
                                    }

                                }
                            }
                            cell.Neighs.Add(sublist);
                        }

                    }
                }
                  }
        }



        private void Cell_Click(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            SolidColorBrush c = new SolidColorBrush(Colors.Black);
            SolidColorBrush w = new SolidColorBrush(Colors.White);

             string s = (label).Name;
                string[] words = s.Split('Y');
                int x = int.Parse(words[1]);
                int y = int.Parse(words[2]);
             //   MessageBox.Show(w.ToString());
          if (  Grids[CurrentGrid].gridCells[x][y].State == 0)
            {
                label.Background = c;
                Grids[CurrentGrid].gridCells[x][y].State = 1;
                Grids[CurrentGrid].gridCells[x][y].newState = 1;
            }
          else
            {
            label.Background = w;
              Grids[CurrentGrid].gridCells[x][y].State = 0;
             Grids[CurrentGrid].gridCells[x][y].newState = 0;
            }
        }

        Label getLabelByCords(int x, int y)
        {
            Label lab = new Label();
            string s = "Y" + x.ToString() + 'Y' + y.ToString();
            lab = (Label)this.FindName(s);
            return lab;
        }
        private void RepaintGrid(MyGrid mg)
        {

            SolidColorBrush _black = new SolidColorBrush(Colors.Black);
            SolidColorBrush _white = new SolidColorBrush(Colors.White);
            foreach (Control ctrl in mg.grid.Children)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    //   ctrl.
                    int rowIndex = System.Windows.Controls.Grid.GetRow(ctrl);
                    int columnIndex = System.Windows.Controls.Grid.GetColumn(ctrl);
                    SolidColorBrush c = new SolidColorBrush(Colors.Black);
                    Label tb = ctrl as Label;


                    Cell cell = mg.gridCells[rowIndex][columnIndex];
                    if (cell.State == 1)
                        tb.Background = _black;
                    if (cell.State == 0)
                        tb.Background = _white;
                }

            }
        }





        public void ChangeStates(int x, int y, MyGrid mg)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Cell cell = mg.gridCells[i][j];
                    cell.State = cell.newState;
                }
            }
        }
        public async Task Step(int x, int y, MyGrid mg)
        {
            await Task.Run(() => {
            if(custom==false)
            {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Cell cell = mg.gridCells[i][j];

                    int counter = 0;
                    foreach (Cell neighbor in cell.Neighbors)
                    {
                        if (neighbor.State == 1 && neighbor!=cell)
                        {
                            counter++;
                        }
                    }
                   // 1. If an alive cell has less than 2 alive neighbours, it dies in the next step.
                    if (counter < 2 && cell.State == 1)
                    {
                        cell.newState = 0;
                    }
                    //2. If an alive cell has 2 or 3 alive neighbours, it survives the step.
                    if (counter == 3 || counter == 2)
                    {
                        if(cell.State==1)
                        cell.newState = 1;
                    }
                    //3. If an alive cells has more than 3 alive neighbours, it dies in the next step.
                    if (cell.State == 1 && counter > 3)
                    {
                        cell.newState = 0;
                    }
                    //4. If a dead cell has 3 alive neighbours, it becomes alive in the next step.

                    if (cell.State == 0 && counter == 3)
                    {
                        cell.newState = 1;
                    }
                }
                }
        
            ChangeStates(Xs[CurrentGrid], Ys[CurrentGrid], mg);
              }
            else
            {
                  for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                       
                        Cell cell = mg.gridCells[i][j];
                    
                        foreach(Rule rule in ActiveRules)
                        {
                            if (rule.OnlyNumber == false)
                            {
                                int checker = 0;
                                for (int a = 0; a < cell.Neighs.Count; a++)
                                {
                                    for (int b = 0; b < cell.Neighs[a].Count; b++)
                                    {
                                        if (mg.Neighbourhood != 4)
                                        {
                                            if (rule.RuleCells[a][b].State == cell.Neighs[a][b].State && rule.Neighbourhood == mg.Neighbourhood)
                                            {

                                                checker++;
                                            }
                                        }
                                        else if (mg.Neighbourhood == 4)
                                        {
                                            if (a != 0 || b != 0)
                                            {
                                                if (a != 2 || b != 0)
                                                    if (a != 0 || b != 2)
                                                        if (a != 2 || b != 2)
                                                        {
                                                            // int z = 0;
                                                            //  z++;
                                                            if (rule.RuleCells[a][b].State == cell.Neighs[a][b].State && rule.Neighbourhood == mg.Neighbourhood)
                                                            {

                                                                checker++;
                                                            }
                                                        }
                                            }
                                        }

                                    }
                                }
                                if (checker == rule.counter && checker!=0 && rule.Neighbourhood == mg.Neighbourhood)
                                    cell.newState = rule.NewState;
                            }
                                //liczymy tylko ilosc
                            else if(rule.OnlyNumber==true)
                            {
                                int checkerWhite = 0;
                                int checkerBlack = 0;
                                for (int a = 0; a < cell.Neighs.Count; a++)
                                {
                                    for (int b = 0; b < cell.Neighs[a].Count; b++)
                                    {
                                        if (mg.Neighbourhood != 4)
                                        {

                                            if(mg.Neighbourhood==8)
                                            { 

                                                if (cell.Neighs[a][b].State ==1 && rule.Neighbourhood == mg.Neighbourhood )
                                                {
                                                    if(a!=1 || b!=1)
                                                    { 
                                                    checkerBlack++;
                                                    }
                                                }
                                          
                                            }
                                            if (cell.Neighs[a][b].State == 0 && rule.Neighbourhood == mg.Neighbourhood)
                                            {
                                                if (a != 1 || b != 1)
                                                    checkerWhite++;
                                            }

                                            else if(mg.Neighbourhood==24)
                                            {

                                                if (cell.Neighs[a][b].State == 1 && rule.Neighbourhood == mg.Neighbourhood)
                                                {
                                                    if (a != 2 || b != 2)
                                                    { 
                                                    checkerBlack++;
                                                    }

                                                }
                                                if (cell.Neighs[a][b].State == 0 && rule.Neighbourhood == mg.Neighbourhood)
                                                {
                                                    if (a != 1 || b != 1)
                                                        checkerWhite++;
                                                }
                                            }
                                        }
                                        else if (mg.Neighbourhood == 4)
                                        {
                                            if (a != 0 || b != 0)
                                            {
                                                if (a != 2 || b != 0)
                                                    if (a != 0 || b != 2)
                                                        if (a != 2 || b != 2)
                                                        {
                                                            if (cell.Neighs[a][b].State == 1 && rule.Neighbourhood == mg.Neighbourhood)
                                                            {
                                                                if (a != 1 || b != 1)
                                                                checkerBlack++;
                                                            }
                                                            if (cell.Neighs[a][b].State == 0 && rule.Neighbourhood == mg.Neighbourhood)
                                                            {
                                                                if (a != 1 || b != 1)
                                                                    checkerWhite++;
                                                            }


                                                        }
                                            }
                                        }

                                    }
                                }
                                if (checkerBlack >= rule.CounterBlack && checkerBlack <= rule.CounterBlackMax && rule.Neighbourhood == mg.Neighbourhood)
                                cell.newState = rule.NewState;
                            }
                        }

                    }
                    
                }

            }
            ChangeStates(Xs[CurrentGrid], Ys[CurrentGrid], mg);
            });
        }



        private async void OneStep_Click(object sender, RoutedEventArgs e)
        {
            MyGrid mg=Grids[CurrentGrid];
            await Step(Xs[CurrentGrid], Ys[CurrentGrid], mg);
            RepaintGrid( mg);

        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {


            int value;
            bool isNumeric = int.TryParse(tBox.Text, out value);
            if (isNumeric == true)
            {

                if (started == false)
                {
                    if (value <= 0)
                    {
                        value = 50;
                        tBox.Text = value.ToString();
                    }
                    T = value;
                    timer.Interval = TimeSpan.FromMilliseconds(T);
                    timer.Start();
                    started = true;
                    PlayButton.Content = "Stop";
                }
                else
                {
                    if (value <= 0)
                    {
                        value = 50;
                        tBox.Text = value.ToString();
                    }
                    T = value;
                    timer.Interval = TimeSpan.FromMilliseconds(T);
                    timer.Stop();
                    started = false;
                    PlayButton.Content = "Run";
                }
            }
            else
                MessageBox.Show("T must be an integer");
        }


        private async void N_Steps_Click(object sender, RoutedEventArgs e)
        {

            MyGrid mg = Grids[CurrentGrid];
            int value;
            bool isNumeric = int.TryParse(nBox.Text, out value);
            if (isNumeric == true)
            {
                N = value;
                for (int i = 0; i < N; i++)
                {
                 await Step(Xs[CurrentGrid], Ys[CurrentGrid], mg);
                }
                RepaintGrid(mg);
            }
            else
            {
                MessageBox.Show("N must be an integer");

            }



        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implementation of cellular Automaton project by Jakub Kaminski\n The user can import old or export new progress through file menu.\n" +
            "By default, Automata works as 8 point neighborhood Conway game of life.\n Once user changes to 'User Rules', he can see current Active rules.\n"+
           "In edit, ruleset editor, the user can choose which rules to activate. \n Each rule can be modified" );
        }
        //przycisk tworzący nowego grida (znaczy się nową zakladkę);
        private void AddGrid_Click(object sender, RoutedEventArgs e)
        {
            GridsN++;
         
        //     MessageBox.Show(name);

            int i = GridsN-1;

            Xs.Add(20);
            Ys.Add(20);

            ChangeGridSizeWindow GW = new ChangeGridSizeWindow(Xs[i], Ys[i]);

            GW.ShowDialog();
           if(GW.Check.Text!="False")
           {
               String name = "Grid" + GridsN.ToString();
               TabItem item = CreateTab(name);

            Xs[i] = int.Parse(GW.NewXBox.Text);
            Ys[i] = int.Parse(GW.NewYBox.Text);

            CreateGrid(item, Xs[i], Ys[i], i);
           }
           else
           {
               Xs.RemoveAt(i);
               Ys.RemoveAt(i);
               GridsN--;
           }


        }


        private void ChangeGrid_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";


            int i=CurrentGrid;
            ChangeGridSizeWindow GW = new ChangeGridSizeWindow(Xs[i], Ys[i]);
        
            GW.ShowDialog();
           Xs[i] =int.Parse(GW.NewXBox.Text);
            Ys[i] = int.Parse(GW.NewYBox.Text);
            String name = "Grid" + CurrentGrid.ToString();
            ChangeGrid(name, Xs[i], Ys[i], CurrentGrid);
        

        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

            int x = AllRules.Count;
            AddRuleWindow AddWindow = new AddRuleWindow(AllRules, PointRules4, PointRules8, PointRules24);
            AddWindow.ShowDialog();
            if(x<AllRules.Count)
            { 
            ActiveRules.Add(AllRules[AllRules.Count-1]);
            AddTooltips();
            }
           }

        public void ChangeGrid(String name, int x, int y, int GridNumber)
        {
            Grids[GridNumber].gridCells.Clear();
            Grid MatrixGrid = new Grid();
            MatrixGrid.Name = name;
            MatrixGrid.Width = Double.NaN;
            MatrixGrid.Height = Double.NaN;
            MatrixGrid.HorizontalAlignment = HorizontalAlignment.Left;
            MatrixGrid.VerticalAlignment = VerticalAlignment.Top;
            MatrixGrid.SetValue(Grid.RowProperty, 3);
            MatrixGrid.SetValue(Grid.ColumnProperty, 1);
            MatrixGrid.SetValue(Grid.RowSpanProperty, 2);
            SolidColorBrush w = new SolidColorBrush(Colors.White);

            List<List<Cell>> cells = new List<List<Cell>>();


            for (int i = 0; i < y; i++)
            {

                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = GridLength.Auto;
                MatrixGrid.ColumnDefinitions.Add(gridCol);

            }

            for (int i = 0; i < x; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                MatrixGrid.RowDefinitions.Add(rowdef);
            }


            for (int i = 0; i < x; i++)
            {
                List<Cell> sublist = new List<Cell>();
                for (int j = 0; j < y; j++)
                {
                    Label Box = new Label();
                    Box.Name = "Y" + i.ToString() + "Y" + j.ToString();

                    object o = MainGrid.FindName(Box.Name);
                    if (o != null)
                    {
                        MainGrid.UnregisterName(Box.Name);
                    }

                    if (x == 0)
                        x = 1;
                    if (y == 0)
                        y = 1;

            //        if (x <= 30)
                        Box.Height = Height / x;
              //      if (y <= 35)
                        Box.Width = Width / y;



                    MainGrid.RegisterName(Box.Name, Box);
                    Cell cell = new Cell(i, j);
                    cell.State = 0;
                    sublist.Add(cell);
                    Box.Background = w;
                    Box.MouseDown += new MouseButtonEventHandler(Cell_Click);
                    Box.BorderBrush = new SolidColorBrush(Colors.Gray);
                    Box.BorderThickness = new Thickness(1);
                    Box.FontSize = 14;
                    Box.FontWeight = FontWeights.Bold;
                    Box.Foreground = new SolidColorBrush(Colors.Black);
                    Box.VerticalAlignment = VerticalAlignment.Top;
                    Grid.SetRow(Box, i);
                    Grid.SetColumn(Box, j);
                    MatrixGrid.Children.Add(Box);
                }
                cells.Add(sublist);
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Cell cell = cells[i][j];
                    for (int filterX = 0 - kernel; filterX <= 3 - 1 - kernel; filterX++)
                    {
                        List<Cell> sublist = new List<Cell>();
                        for (int filterY = 0 - kernel; filterY <= 3 - 1 - kernel; filterY++)
                        {
                            int a = i + filterX;
                            int b = j + filterY;


                            if (a >= 0 && a < x && b >= 0 && b < y)
                            {
                                // if (a != i || b != j)
                                {
                                    cell.Neighbors.Add(cells[a][b]);
                                    //   c.State = cells[a][b].State;
                                    sublist.Add(cells[a][b]);
                                }

                            }
                        }
                        cell.Neighs.Add(sublist);
                    }

                }
            }
            MyGrid mg = new MyGrid();
            mg.grid = MatrixGrid;
            mg.gridCells = cells;
            Grids[GridNumber] = mg;
            TabItem result = MyTab.SelectedItem as TabItem;
            if (result != null)
            {
                ScrollViewer scroller = new ScrollViewer();
                scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scroller.Content = MatrixGrid;
                result.Content = scroller;

            }
            else
                MessageBox.Show("Error while changing");
         

        }

        private void MyTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
            if (Grids.Count > 0)
            {
                CurrentGrid = MyTab.SelectedIndex;
                if (Grids[CurrentGrid].Neighbourhood == 4)
                {
       
                    NCombo.SelectedValue = "4 Point";
                }
                else if (Grids[CurrentGrid].Neighbourhood == 8)
                {
      
                    NCombo.SelectedValue = "8 Point";
                }
                else if (Grids[CurrentGrid].Neighbourhood == 24)
                {
          
                    NCombo.SelectedValue = "24 Point";
                }
            }

        }

        private void ChangeRule_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
          //  TabItem result = MyTab.SelectedItem as TabItem;
            TreeViewItem item = RuleTree.SelectedItem as TreeViewItem;

            if (item != null)
            {
                string oName=item.Name;
                Rule result = AllRules.Find(x => x.Name == item.Name);
               // MessageBox.Show(result.Name);
                if (result != null)
                {
                    AddRuleWindow AddWindow = new AddRuleWindow(result, AllRules, PointRules4, PointRules8, PointRules24);
                    AddWindow.ShowDialog();
                    Rule newrule = ActiveRules.Find(x => x.Name == oName);
                    newrule = result;
                    item.Name = result.Name;
                }
                else
                    MessageBox.Show("Please select a rule to edit");
            }

        }

        private void Pattern_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = PatternTree.SelectedItem as TreeViewItem;
            if (item != null)
            {
                string name = item.Name;
                if(name=="Chess")
                {
                    for (int i = 0; i < Xs[CurrentGrid]; i++)
                    {
                        for (int j = 0; j < Ys[CurrentGrid]; j++)
                        {
                            Cell cell = Grids[CurrentGrid].gridCells[i][j];

                            if(i%2==0 && j%2==0)
                            {
                                cell.State = 1;
                                cell.newState = 1;
                            }
                        }

                    }
                    RepaintGrid(Grids[CurrentGrid]);
                }
                else if(name=="Cross")
                {
                    for (int i = 0; i < Xs[CurrentGrid]; i++)
                    {
                        for (int j = 0; j < Ys[CurrentGrid]; j++)
                        {
                            Cell cell = Grids[CurrentGrid].gridCells[i][j];

                            if (i == (Xs[CurrentGrid]/2) || j ==(Ys[CurrentGrid]/2))
                            {
                                cell.State = 1;
                                cell.newState = 1;
                            }
                        }
                    }
                    RepaintGrid(Grids[CurrentGrid]);
                }
                else if (name == "Blank")
                {
                    for (int i = 0; i < Xs[CurrentGrid]; i++)
                    {
                        for (int j = 0; j < Ys[CurrentGrid]; j++)
                        {
                            Cell cell = Grids[CurrentGrid].gridCells[i][j];

                                cell.State = 0;
                                cell.newState = 0;
                         
                        }
                    }
                    RepaintGrid(Grids[CurrentGrid]);
                }
                else if (name == "Random")
                {
                    Random rnd = new Random();
                    for (int i = 0; i < Xs[CurrentGrid]; i++)
                    {
                        for (int j = 0; j < Ys[CurrentGrid]; j++)
                        {
                            Cell cell = Grids[CurrentGrid].gridCells[i][j];
                          
                            int dice = rnd.Next(1, 10);
                            if (i % dice == 0 || j%dice ==0)
                            {
                                cell.State = 1;
                                cell.newState = 1;
                            }
                        }
                    }
                    RepaintGrid(Grids[CurrentGrid]);
                }


            }
        }



        private void RulesetEditor_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
            RulesetEditor RE = new RulesetEditor(AllRules, ActiveRules, PointRules4, PointRules8, PointRules24);
            RE.ShowDialog();
      
        }

        private void RulesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
            string selected = ((ComboBoxItem)RulesCombo.SelectedItem).Content.ToString();

            if (selected == "Conway")
            {
                custom = false;
                RuleTree.Visibility = Visibility.Hidden;
                NCombo.IsEnabled = false;
            }
            else
            {
                custom = true;
                RuleTree.Visibility = Visibility.Visible;
                NCombo.IsEnabled = true;
            }
        }

        private void ExportGrid_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
        
            ExportClass Export = new ExportClass();

            MyGrid g = Grids[CurrentGrid];
            Export.Cells = Functions.PrepareExport(g);
      //   Export.Rules = AllRules;


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
               // Functions.WriteToXmlFile<MyGrid>(saveFileDialog.FileName, g);
                Functions.WriteToXmlFile<ExportClass>(saveFileDialog.FileName, Export);

            ChangeNeighbourhood(Xs[CurrentGrid], Ys[CurrentGrid], Grids[CurrentGrid]);
            RepaintGrid(Grids[CurrentGrid]);
        }

        private void ImportGrid_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                ExportClass Imported = new ExportClass();

                try
                {
                    Imported = Functions.ReadFromXmlFile<ExportClass>(openFileDialog.FileName);
                    List<List<Cell>> Importedcells = Imported.Cells;
        
                    if (Importedcells.Count != 0)
                    {
                        GridsN++;
                        String name = "Grid" + GridsN.ToString();
                        TabItem item = CreateTab(name);
                        int z = GridsN - 1;

                        //      List<Rule> Importedrulles = Imported.Rules;


                        CreateGrid(item, Importedcells.Count, Importedcells[0].Count, z);

                        Xs.Add(Importedcells.Count);
                        Ys.Add(Importedcells[0].Count);
                        Grids[z].gridCells = Importedcells;
                        ChangeNeighbourhood(Importedcells.Count, Importedcells[0].Count, Grids[z]);
                        RepaintGrid(Grids[z]);
                    }
                    else
                    {
                        MessageBox.Show("This file does not contain Proper Grid information");
                    }

                }
                catch (Exception a)
                {
                    MessageBox.Show("This file does not contain Proper Grid information");
                }
            }
        }

        private void ActivateAll_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

            foreach (Rule rule in AllRules)
            {
                Rule result = ActiveRules.Find(x => x.Name == rule.Name);
                if (result == null)
                {
                    ActiveRules.Add(rule);
                    PointRules4.Items.Clear();
                    PointRules4.Items.Clear();
                    PointRules8.Items.Clear();
                    PointRules24.Items.Clear();

                    foreach (Rule Rule in ActiveRules)
                    {
                        TreeViewItem x = AddRuleWindow.GetTreeView(Rule.Name);
                        if (Rule.Neighbourhood == 4)
                        {
                            PointRules4.Items.Add(x);
                        }
                        else if (Rule.Neighbourhood == 8)
                            PointRules8.Items.Add(x);
                        else if (Rule.Neighbourhood == 24)
                            PointRules24.Items.Add(x);
                    }
                }
            }
        }

        private void _ExportAutomata_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

            //List<Rule> Imported = new List<Rule>();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Text file (*.txt)|*.txt";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    try
            //    {
            //        Imported = Functions.ReadFromXmlFile<List<Rule>>(openFileDialog.FileName);

            //        foreach (Rule rule in Imported)
            //        {
            //            Rule result = AllRules.Find(x => x.Name == rule.Name);
            //            if (result == null)
            //            {
            //                AllRules.Add(rule);
            //               }
            //        }
            //    }
            //    catch (Exception a)
            //    {
            //        MessageBox.Show("This file does not contain proper rule data");
            //    }
            //}
            ExportClass Export = new ExportClass();
      
             MyGrid g = Grids[CurrentGrid];


              Export.Cells = Functions.PrepareExport(g);
              Export.Rules = AllRules;

              SaveFileDialog saveFileDialog = new SaveFileDialog();
              saveFileDialog.Filter = "Text file (*.txt)|*.txt";
              if (saveFileDialog.ShowDialog() == true)
              {
                  // Functions.WriteToXmlFile<MyGrid>(saveFileDialog.FileName, g);
                  Functions.WriteToXmlFile<ExportClass>(saveFileDialog.FileName, Export);

                
              }
              ChangeNeighbourhood(Xs[CurrentGrid], Ys[CurrentGrid], Grids[CurrentGrid]);
              RepaintGrid(Grids[CurrentGrid]);

        }

        private void _SaveRules_Click(object sender, RoutedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
            ExportClass Export = new ExportClass();
            Export.Rules = AllRules;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                Functions.WriteToXmlFile<ExportClass>(saveFileDialog.FileName, Export);
        }

        private void AddTooltips()
        {
            foreach (object obj in PointRules4.Items)
                    {

                        TreeViewItem TVI = obj as TreeViewItem;
                         Rule rule = AllRules.Find(x => x.Name == TVI.Header.ToString());
                        if (TVI != null && rule!=null)
                        {

                            TVI.MouseDoubleClick += new MouseButtonEventHandler(Rule_DoubleClick);
                         //   TVI.ToolTip = "Hello There";
                         }
                        }
            foreach (object obj in PointRules8.Items)
            {

                TreeViewItem TVI = obj as TreeViewItem;
                Rule rule = AllRules.Find(x => x.Name == TVI.Header.ToString());
                if (TVI != null && rule != null)
                {

                  TVI.MouseDoubleClick += new MouseButtonEventHandler(Rule_DoubleClick);
               //     TVI.ToolTip = "Hello There";
                }
            }
            foreach (object obj in PointRules24.Items)
            {

                TreeViewItem TVI = obj as TreeViewItem;
                Rule rule = AllRules.Find(x => x.Name == TVI.Header.ToString());
                if (TVI != null && rule != null)
                {

                    TVI.MouseDoubleClick += new MouseButtonEventHandler(Rule_DoubleClick);
                  //  TVI.ToolTip = "Hello there";
                }
            }



             }

        private void Rule_DoubleClick(object sender, MouseEventArgs e)
        {
        //   MessageBox.Show(sender.ToString());

            TreeViewItem item = RuleTree.SelectedItem as TreeViewItem;

            if (item != null)
            {
                string oName = item.Name;
                Rule result = AllRules.Find(x => x.Name == item.Name);
                // MessageBox.Show(result.Name);
                if (result != null)
                {
                    AddRuleWindow AddWindow = new AddRuleWindow(result, AllRules, PointRules4, PointRules8, PointRules24);
                    AddWindow.ShowDialog();

                    Rule newrule = ActiveRules.Find(x => x.Name == oName);
                    newrule = result;
                    item.Name = result.Name;
                }
                else
                    MessageBox.Show("Please select a rule to edit");
            }
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

           for(int i=0; i<Grids.Count; i++)
            {
                MyGrid mg = Grids[i];
                int x = Xs[i];
                int y = Ys[i];
                foreach (Control ctrl in mg.grid.Children)
                {
                    if (ctrl.GetType() == typeof(Label))
                    {
                        //   ctrl.
                        int rowIndex = System.Windows.Controls.Grid.GetRow(ctrl);
                        int columnIndex = System.Windows.Controls.Grid.GetColumn(ctrl);
                        SolidColorBrush c = new SolidColorBrush(Colors.Black);
                        Label tb = ctrl as Label;

                    // if (x <= 30)
                           tb.Height = ((int)Row1.ActualHeight+(int)Row2.ActualHeight) / x;
                        if(tb.Height>50)
                          Height = (int)tb.Height;
                   //   if (y <= 35)
                            tb.Width = (Col.ActualWidth-10) / y;
                        if(tb.Width>100)
                            Width = (int)tb.Width;
                  }

                }
            }
        }

        private void _ImportAutomata_Click(object sender, RoutedEventArgs e)
        {

            started = false;
            PlayButton.Content = "Run";
            timer.Stop();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                ExportClass Imported = new ExportClass();
             
                try
                {
                    Imported = Functions.ReadFromXmlFile<ExportClass>(openFileDialog.FileName);
                  

            
                    List<List<Cell>> Importedcells = Imported.Cells;
                    List<Rule> Importedrulles = Imported.Rules;
                    if (Importedcells.Count!=0)
                    {
                        GridsN++;
                        String name = "Grid" + GridsN.ToString();
                        TabItem item = CreateTab(name);
                        int z = GridsN - 1;



                        CreateGrid(item, Importedcells.Count, Importedcells[0].Count, z);

                        Xs.Add(Importedcells.Count);
                        Ys.Add(Importedcells[0].Count);
                        Grids[z].gridCells = Importedcells;
                        ChangeNeighbourhood(Importedcells.Count, Importedcells[0].Count, Grids[z]);
                        RepaintGrid(Grids[z]);
                    }
                    foreach(Rule rule in Importedrulles)
                    {
                        AllRules.Add(rule);
                    }
                    if(GridsN<=2)
                    {
                        MessageBox.Show("Please remember to activate rules before starting the computation\n It is possible in 'Edit' menu");
                    }

                }
                catch (Exception a)
                {
                    MessageBox.Show("This file does not contain Proper Automata information");
                }
            }


        }

        private void _ImportRules_Click(object sender, RoutedEventArgs e)
        {
            ExportClass Imported;
            //List<Person> people = XmlSerialization.ReadFromXmlFile<List<Person>>("C:\people.txt");


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Imported = Functions.ReadFromXmlFile<ExportClass>(openFileDialog.FileName);

                    foreach (Rule rule in Imported.Rules)
                    {
                        Rule result = AllRules.Find(x => x.Name == rule.Name);
                        if (result == null)
                        {
                            AllRules.Add(rule);
                            }
                    }
                }
                catch (Exception a)
                {
                    MessageBox.Show("This file does not contain proper rule data");
                }
            }
        }

        private void RemoveGrid_Click(object sender, RoutedEventArgs e)
        {

            if (Grids.Count - 1 > 0 && CurrentGrid != 0)
            {
                GridsN--;
                int GridToDelete = MyTab.SelectedIndex;

                MyTab.SelectedIndex = 0;
                MyTab.Items.RemoveAt(GridToDelete);
                Grids.RemoveAt(GridToDelete);
                Xs.RemoveAt(GridToDelete);
                Ys.RemoveAt(GridToDelete);
            }

            else
            {
                MessageBox.Show("The main cannot be deleted");
            }
        }


     
      
    }

   // Mouse.MouseEnter="TreeViewItem_MouseEnter"

}
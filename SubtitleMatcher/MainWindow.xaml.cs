using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
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

namespace SubtitleMatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeBinding();
            InitializeEvent();
            DataContext = this;
        }
        private void InitializeBinding()
        {
            LeftList.ItemsSource = Data.leftList;
            RightList.ItemsSource = Data.rightList;
        }
        private void InitializeEvent()
        {
            LeftList.Drop += List_Drop;
            RightList.Drop += List_Drop;
            LeftDel.Click += Del_Button_Click;
            RightDel.Click += Del_Button_Click;
            LeftUp.Click += Up_Button_Click;
            RightUp.Click += Up_Button_Click;
            LeftDown.Click += Down_Button_Click;
            RightDown.Click += Down_Button_Click;
            LeftSort.Click += Sort_Button_Click;
            RightSort.Click += Sort_Button_Click;
            LeftClr.Click += Clear_Button_Click;
            RightClr.Click += Clear_Button_Click;
        }
        private void List_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                switch (((ListView)sender).Name) {
                    case "LeftList":
                        foreach (string file in files)
                        {
                            Data.leftList.Add(new Data.ListItem(file));
                        }
                        break;
                    case "RightList":
                        foreach (string file in files)
                        {
                            Data.rightList.Add(new Data.ListItem(file));
                        }
                        break;
                    default: break;
                }
            }
        }
        private void Del_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftDel":
                    Data.RemoveListItemWithIndex(true, LeftList.SelectedIndex);
                    break;
                case "RightDel":
                    Data.RemoveListItemWithIndex(false, RightList.SelectedIndex);
                    break;
                default: break;
            }
        }
        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftUp":
                    int leftIndex = LeftList.SelectedIndex;
                    Data.MoveUpListItemWithIndex(true, LeftList.SelectedIndex);
                    LeftList.SelectedIndex = Math.Max(-1, leftIndex - 1);
                    break;
                case "RightUp":
                    int rightIndex = RightList.SelectedIndex;
                    Data.MoveUpListItemWithIndex(false, RightList.SelectedIndex);
                    RightList.SelectedIndex = Math.Max(-1, rightIndex - 1);
                    break;
                default: break;
            }
        }
        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftDown":
                    int leftIndex = LeftList.SelectedIndex;
                    Data.MoveDownListItemWithIndex(true, LeftList.SelectedIndex);
                    LeftList.SelectedIndex = leftIndex + 1;
                    break;
                case "RightDown":
                    int rightIndex = RightList.SelectedIndex;
                    Data.MoveDownListItemWithIndex(false, RightList.SelectedIndex);
                    RightList.SelectedIndex = rightIndex + 1;
                    break;
                default: break;
            }
        }
        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftSort":
                    Data.SortList(true);
                    LeftList.SelectedIndex = -1;
                    break;
                case "RightSort":
                    Data.SortList(false);
                    RightList.SelectedIndex = -1;
                    break;
                default: break;
            }
        }
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftClr":
                    Data.ClearList(true);
                    LeftList.SelectedIndex = -1;
                    break;
                case "RightClr":
                    Data.ClearList(false);
                    RightList.SelectedIndex = -1;
                    break;
                default: break;
            }
        }
        private void ExecButton_Click(object sender, RoutedEventArgs e)
        {
            if (Data.leftList.Count != Data.rightList.Count)
            {
                MessageBox.Show("左右项目数量不一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Data.ApplyFileNames(Addon.Text);
        }
    }
}

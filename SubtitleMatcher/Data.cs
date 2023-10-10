using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleMatcher
{
    class Data
    {
        public static ObservableCollection<ListItem> leftList = new();
        public static ObservableCollection<ListItem> rightList = new();
        public class ListItem
        {
            string path;
            public ListItem(string path) { this.path = path; }
            public string GetPath() { return path; }
            public override string ToString()
            {
                return Path.GetFileName(path);
            }
        }
        public static void RemoveListItemWithIndex(bool isLeft, int index)
        {
            if (index == -1) return;
            if (isLeft)
            {
                leftList.RemoveAt(index);
            }
            else
            {
                rightList.RemoveAt(index);
            }
        }
        public static void MoveUpListItemWithIndex(bool isLeft, int index)
        {
            if (index == -1) return;
            if (index == 0) return;
            if (isLeft)
            {
                ListItem listItem = leftList[index];
                leftList[index] = leftList[index - 1];
                leftList[index - 1] = listItem;
            }
            else
            {
                ListItem listItem = rightList[index];
                rightList[index] = rightList[index - 1];
                rightList[index - 1] = listItem;
            }
        }
        public static void MoveDownListItemWithIndex(bool isLeft, int index)
        {
            if (index == -1) return;
            if (isLeft)
            {
                if (index == leftList.Count - 1) return;
                ListItem listItem = leftList[index];
                leftList[index] = leftList[index + 1];
                leftList[index + 1] = listItem;
            }
            else
            {
                if (index == rightList.Count - 1) return;
                ListItem listItem = rightList[index];
                rightList[index] = rightList[index + 1];
                rightList[index + 1] = listItem;
            }
        }
        public static void SortList(bool isLeft)
        {
            if (isLeft)
            {
                List<ListItem> sortedList = leftList.OrderBy(item => item.ToString()).ToList();
                leftList.Clear();
                foreach (ListItem item in sortedList)
                {
                    leftList.Add(item);
                }
            }
            else
            {
                List<ListItem> sortedList = rightList.OrderBy(item => item.ToString()).ToList();
                rightList.Clear();
                foreach (ListItem item in sortedList)
                {
                    rightList.Add(item);
                }
            }
        }
        public static void ApplyFileNames(string addon)
        {
            List<string> paths = leftList.Select(item => item.GetPath()).ToList();
            List<string> newNames = rightList.Select(item => item.ToString()).ToList();

            for (int i = 0; i < paths.Count; i++)
            {
                try
                {
                    string sourcePath = paths[i];
                    string newName = newNames[i];
                    int addonIndex = newName.LastIndexOf('.');
                    if (addonIndex == -1) return;
                    string extension = Path.GetExtension(sourcePath);
                    newName = newName.Remove(addonIndex + 1);
                    newName = newName.Insert(addonIndex + 1, addon + extension);
                    string newFilePath = Path.Combine(Path.GetDirectoryName(sourcePath), newName);
                    File.Move(sourcePath, newFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public static void ClearList(bool isLeft)
        {
            if (isLeft)
            {
                leftList.Clear();
            }
            else
            {
                rightList.Clear();
            }
        }
    }
}

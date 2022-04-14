using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia.Media.Imaging;

namespace lab9.Models
{
    public class FileTreeItem : ReactiveObject
    {
        public string Name
        {
            get; set;
        }

        public bool Direct;

        ObservableCollection<FileTreeItem>? items;
        public ObservableCollection<FileTreeItem>? Items
        {
            get
            {
                if(items==null && Direct)
                {
                    items = new ObservableCollection<FileTreeItem>();
                    
                    string[] str;
                    try
                    {
                        str = Directory.GetDirectories(path);
                        foreach (string file in str)
                        {
                            items.Add(new FileTreeItem(file));
                        }
                    } catch(UnauthorizedAccessException)
                    {

                    }
                    try
                    {
                        str = Directory.GetFiles(path);
                        foreach (string file in str)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            string extension = fileInfo.Extension;
                            if (extension == ".png" || extension == ".jpg")
                            {
                                items.Add(new FileTreeItem(file));
                            }
                        }
                    } catch(UnauthorizedAccessException) { }
                }
                return items;
            }
        }
        public string path
        {
            get; private set;
        }
        public FileTreeItem(string path)
        {
            items = null;
            this.path = path;
            var file = new FileInfo(path);
            if (!file.Exists && !file.Attributes.HasFlag(FileAttributes.Directory))
            {
                throw new NullReferenceException();
            }
            Direct = file.Attributes.HasFlag(FileAttributes.Directory);
            if (file.Name.Length==0) Name = path;
            else Name = file.Name;
        }
    }
}

using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.IO;

namespace MyLittleTools3
{

    class MyFileTool
    {
        private class RenameRule
        {
            public String path;
            public String oldname;
            public String newname;
        }
        private List<RenameRule> renameList;

        public List<String> fileList = new List<String>();
        public String fileSource = "";
        public String fileTarget = "";
  
        public String BatchRename()
        {
            if (this.fileSource == "")
            {
                return "参数有误";
            }

            renameList = new List<RenameRule>();

            int length = this.fileList.Count;
            for (int i = 0; i < length; i++)
            {
                RenameRule rule = new RenameRule
                {
                    path = Path.GetDirectoryName(fileList[i]),
                    oldname = Path.GetFileName(fileList[i])
                };
                if (fileTarget == null)
                {
                    // 模板方式
                    rule.newname = fileSource.Replace("*", (i+1).ToString().PadLeft(length.ToString().Length, '0'));
                }
                else　
                {
                    //字符替换
                    rule.newname = Path.GetFileNameWithoutExtension(fileList[i]).Replace(fileSource, fileTarget);
                }
                rule.newname = rule.newname + Path.GetExtension(fileList[i]);
                renameList.Add(rule);
            }
            return RenameView();
        }

        private String RenameView()
        {
            String text = "程序错误";
            if (this.renameList.Count > 0)
            {
                text = "";
                foreach (RenameRule rule in renameList)
                {
                    text += rule.oldname + "\t" + "→" + "\t" + rule.newname + Environment.NewLine;
                }
            }
            return text;
        }

        public void DoRename()
        {
            int length = this.renameList.Count;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    RenameRule rule = this.renameList[i];
                    try
                    {
                        File.Move(Path.Combine(rule.path, rule.oldname), Path.Combine(rule.path, rule.newname));
                    }
                    catch (Exception)
                    {
                        String newname = "ErrFile" + i.ToString() + "-" + rule.newname;
                        rule.newname = newname;
                        try
                        {
                            File.Move(Path.Combine(rule.path, rule.oldname), Path.Combine(rule.path, rule.newname));
                        }
                        catch (Exception)
                        {
                            rule.newname = rule.oldname;
                        }
                        finally
                        {
                            this.renameList[i] = rule;
                        }
                    }
                    finally
                    {
                        this.fileList[i] = Path.Combine(rule.path, rule.newname);
                    }
                }
            }
        }
    }

    class MyFileAttr : INotifyPropertyChanged
    {
        private String filePath;
        private FileAttributes fileAttr;

        public MyFileAttr(String FilePath)
        {
            if (File.Exists(FilePath))
            {
                this.filePath = FilePath;
                this.fileAttr = File.GetAttributes(this.filePath);
            }
            else
            {
                this.filePath = "";
                this.fileAttr = 0;
            }
            NotifyChanged();
        }

        public Boolean ReadOnly
        {
            get
            {
                return (this.fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                this.ChangeAttr(FileAttributes.ReadOnly, value);
            }
        }

        public Boolean Hidden
        {
            get
            {
                return (this.fileAttr & FileAttributes.Hidden) == FileAttributes.Hidden;
            }
            set
            {
                this.ChangeAttr(FileAttributes.Hidden, value);
            }
        }

        public Boolean Archive
        {
            get
            {
                return (this.fileAttr & FileAttributes.Archive) == FileAttributes.Archive;
            }
            set
            {
                this.ChangeAttr(FileAttributes.Archive, value);
            }
        }

        public Boolean System
        {
            get
            {
                return (this.fileAttr & FileAttributes.System) == FileAttributes.System;
            }
            set
            {
                this.ChangeAttr(FileAttributes.System, value);
            }
        }

        public Boolean Normal
        {
            get
            {
                return (this.fileAttr & FileAttributes.Normal) == FileAttributes.Normal;
            }
            set
            {
                this.ChangeAttr(FileAttributes.Normal, value);
            }
        }

        public Boolean NotContentIndexed
        {
            get
            {
                return (this.fileAttr & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed;
            }
            set
            {
                this.ChangeAttr(FileAttributes.NotContentIndexed, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ReadOnly"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Hidden"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Archive"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NotContentIndexed"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("System"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Normal"));
            }
        }

        public void ChangeAttr(FileAttributes attr, Boolean value)
        {
            if (this.filePath != "")
            {
                if (attr == FileAttributes.Normal)
                {
                    if (value)
                    {
                        this.fileAttr = FileAttributes.Normal;
                    }
                }
                else
                {
                    if (value)
                    {
                        this.fileAttr = fileAttr | attr;
                        this.fileAttr = fileAttr & ~FileAttributes.Normal;
                    }
                    else
                    {
                        this.fileAttr = fileAttr & ~attr;
                    }
                }
                File.SetAttributes(this.filePath, this.fileAttr);
            }
            NotifyChanged();
        }

    }

}

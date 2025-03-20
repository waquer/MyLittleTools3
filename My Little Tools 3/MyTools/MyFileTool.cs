using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace MyLittleTools3.MyTools
{
    internal class MyFileTool
    {
        private class RenameRule
        {
            public string Path;
            public string OldName;
            public string NewName;
        }
        private List<RenameRule> _renameList;

        public readonly List<string> FileList = new List<string>();
        public string FileSource = "";
        public string FileTarget = "";

        public string BatchRename()
        {
            if (this.FileSource == "") {
                return "参数有误";
            }

            _renameList = new List<RenameRule>();

            var length = this.FileList.Count;
            for (var i = 0; i < length; i++) {
                var rule = new RenameRule {
                    Path = Path.GetDirectoryName(FileList[i]),
                    OldName = Path.GetFileName(FileList[i]),
                    // 模板方式
                    NewName = FileTarget == null ? FileSource.Replace("*", (i + 1).ToString().PadLeft(length.ToString().Length, '0')) :
                        //字符替换
                        Path.GetFileNameWithoutExtension(FileList[i])?.Replace(FileSource, FileTarget)
                };
                rule.NewName += Path.GetExtension(FileList[i]);
                _renameList.Add(rule);
            }
            return RenameView();
        }

        private string RenameView()
        {
            const string text = "程序错误";
            return _renameList.Count <= 0 ? text : _renameList.Aggregate("", (current, rule) => current + (rule.OldName + "\t" + "→" + "\t" + rule.NewName + Environment.NewLine));
        }

        public void DoRename()
        {
            var length = _renameList.Count;
            if (length <= 0) return;
            for (var i = 0; i < length; i++) {
                var rule = _renameList[i];
                try {
                    File.Move(Path.Combine(rule.Path, rule.OldName), Path.Combine(rule.Path, rule.NewName));
                } catch (Exception) {
                    var newName = "ErrFile" + i + "-" + rule.NewName;
                    rule.NewName = newName;
                    try {
                        File.Move(Path.Combine(rule.Path, rule.OldName), Path.Combine(rule.Path, rule.NewName));
                    } catch (Exception) {
                        rule.NewName = rule.OldName;
                    } finally {
                       _renameList[i] = rule;
                    }
                } finally {
                    FileList[i] = Path.Combine(rule.Path, rule.NewName);
                }
            }
        }

        public void DoUpdate(string FilePath)
        {
            if (MessageBox.Show("此操作将覆盖原程序，确定吗？", "程序更新", MessageBoxButton.OKCancel) !=
                MessageBoxResult.OK) return;
            var fileNew = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            if (fileNew != null) File.Copy(fileNew, FilePath, true);
        }

    }

    internal class MyFileAttr : INotifyPropertyChanged
    {
        private readonly string _filePath;
        private FileAttributes _fileAttr;

        public MyFileAttr(string FilePath)
        {
            if (File.Exists(FilePath)) {
                _filePath = FilePath;
                _fileAttr = File.GetAttributes(_filePath);
            } else {
                _filePath = "";
                _fileAttr = 0;
            }
            NotifyChanged();
        }

        public bool ReadOnly {
            get => (_fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            set => ChangeAttr(FileAttributes.ReadOnly, value);
        }

        public bool Hidden {
            get => (_fileAttr & FileAttributes.Hidden) == FileAttributes.Hidden;
            set => ChangeAttr(FileAttributes.Hidden, value);
        }

        public bool Archive {
            get => (_fileAttr & FileAttributes.Archive) == FileAttributes.Archive;
            set => ChangeAttr(FileAttributes.Archive, value);
        }

        public bool System {
            get => (_fileAttr & FileAttributes.System) == FileAttributes.System;
            set => ChangeAttr(FileAttributes.System, value);
        }

        public bool Normal {
            get => (_fileAttr & FileAttributes.Normal) == FileAttributes.Normal;
            set => ChangeAttr(FileAttributes.Normal, value);
        }

        public bool NotContentIndexed {
            get => (_fileAttr & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed;
            set => ChangeAttr(FileAttributes.NotContentIndexed, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChanged()
        {
            if (PropertyChanged == null) return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ReadOnly"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Hidden"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Archive"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NotContentIndexed"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("System"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Normal"));
        }

        private void ChangeAttr(FileAttributes attr, bool value)
        {
            if (_filePath != "") {
                if (attr == FileAttributes.Normal) {
                    if (value) {
                        _fileAttr = FileAttributes.Normal;
                    }
                } else {
                    if (value) {
                        _fileAttr |= attr;
                        _fileAttr &= ~FileAttributes.Normal;
                    } else {
                        _fileAttr &= ~attr;
                    }
                }
                File.SetAttributes(_filePath, _fileAttr);
            }
            NotifyChanged();
        }

    }

}

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Shell;

namespace MyLittleTools3.MyTools
{
    internal class MyJumpList
    {
        public readonly ObservableCollection<JumpTask> JtData = new ObservableCollection<JumpTask>();
        private JumpList _jumpList = JumpList.GetJumpList(Application.Current);

        public MyJumpList()
        {
            LoadIni();
        }

        public void Add(string path, string name = "", string desc = "")
        {
            if (name == "")
            {
                name = Path.GetFileNameWithoutExtension(path);
            }

            if (desc == "")
            {
                desc = name;
            }

            var jumpTask = new JumpTask
            {
                ApplicationPath = path,
                IconResourcePath = path,
                Title = name,
                Description = desc
            };

            JtData.Add(jumpTask);
        }

        public void ClearAll()
        {
            JtData.Clear();
            //setJumpList();
        }

        public void RemoveAt(int idx)
        {
            if (idx > -1 && idx < JtData.Count)
            {
                JtData.RemoveAt(idx);
            }
        }

        public void MoveUp(int idx)
        {
            if (idx <= 0) return;
            var jt = JtData[idx];
            JtData.RemoveAt(idx);
            JtData.Insert(idx - 1, jt);
        }

        public void MoveDown(int idx)
        {
            if (idx >= JtData.Count - 1) return;
            var jt = JtData[idx];
            JtData.RemoveAt(idx);
            JtData.Insert(idx + 1, jt);
        }

        public void SetJumpList()
        {
            if (_jumpList != null)
            {
                _jumpList.JumpItems.Clear();
            }
            else
            {
                _jumpList = new JumpList();
            }

            if (JtData.Count > 0)
            {
                foreach (var jt in JtData)
                {
                    _jumpList.JumpItems.Add(jt);
                }

                _jumpList.JumpItems.Add(new JumpTask());
            }

            AppendDefault();
            _jumpList.Apply();
            SaveIni();
        }

        private void AppendDefault()
        {
            var appPath = System.Reflection.Assembly.GetEntryAssembly()?.Location;

            string[,] argArray =
            {
                { "修改Hosts", "-edithosts" }
            };

            for (var i = 0; i < argArray.GetLength(0); i++)
            {
                var jumpTask = new JumpTask
                {
                    ApplicationPath = appPath,
                    IconResourcePath = appPath,
                    Title = argArray[i, 0],
                    Description = argArray[i, 0],
                    Arguments = argArray[i, 1]
                };
                _jumpList.JumpItems.Add(jumpTask);
            }
        }

        private void LoadIni()
        {
            var keyList = new StringCollection();
            MyIniTool.ReadSection("JumpList", keyList);
            foreach (var key in keyList)
            {
                var path = MyIniTool.ReadString("JumpList", key, "");
                var jumpTask = new JumpTask
                {
                    Title = key,
                    Description = key,
                    ApplicationPath = path,
                    IconResourcePath = path
                };
                JtData.Add(jumpTask);
            }
        }

        private void SaveIni()
        {
            MyIniTool.EraseSection("JumpList");
            foreach (var jumpTask in JtData)
            {
                MyIniTool.WriteString("JumpList", jumpTask.Title, jumpTask.ApplicationPath);
            }
        }
    }
}
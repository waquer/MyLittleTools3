using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Shell;

namespace MyLittleTools3
{
    class MyJumpList
    {

        private ObservableCollection<JumpTask> JTData = new ObservableCollection<JumpTask>();
        private JumpList jumpList = JumpList.GetJumpList(App.Current);

        public MyJumpList()
        {
            this.LoadINI();
        }

        public void Add(String path, String name="", String desc="")
        {
            if (name=="")
            {
                name = Path.GetFileNameWithoutExtension(path);
            }

            if (desc == "")
            {
                desc = name;
            }

            JumpTask jumpTask = new JumpTask
            {
                ApplicationPath = path,
                IconResourcePath = path,
                Title = name,
                Description = desc
            };

            this.JTData.Add(jumpTask);
        }

        public void ClearAll()
        {
            this.JTData.Clear();
            //this.setJumpList();
        }

        public void RemoveAt(int idx)
        {
            if (idx > -1 && idx < JTData.Count)
            {
                this.JTData.RemoveAt(idx);
            }
        }

        public void MoveUp(int idx)
        {
            if (idx > 0)
            {
                JumpTask jt = JTData[idx];
                JTData.RemoveAt(idx);
                JTData.Insert(idx-1, jt);
            }
        }

        public void MoveDown(int idx)
        {
            if (idx < JTData.Count - 1)
            {
                JumpTask jt = JTData[idx];
                JTData.RemoveAt(idx);
                JTData.Insert(idx + 1, jt);
            }
        }

        public void SetJumpList()
        {
            if (jumpList != null)
            {
                this.jumpList.JumpItems.Clear();
            }
            else
            {
                this.jumpList = new JumpList();
            }

            if (JTData.Count > 0)
            {
                foreach (JumpTask jt in JTData)
                {
                    this.jumpList.JumpItems.Add(jt);
                }
                this.jumpList.JumpItems.Add(new JumpTask());
            }
            this.AppendDefault();
            this.jumpList.Apply();
            this.SaveINI();
        }

        private void AppendDefault()
        {
            String appPath = System.Reflection.Assembly.GetEntryAssembly().Location;

            String[,] argArray =
            {
                {"修改Hosts","-edithosts"}
            };

            for (int i = 0; i < argArray.GetLength(0); i++)
            {
                JumpTask jumpTask = new JumpTask
                {
                    ApplicationPath = appPath,
                    IconResourcePath = appPath,
                    Title = argArray[i, 0],
                    Description = argArray[i, 0],
                    Arguments = argArray[i, 1]
                };
                this.jumpList.JumpItems.Add(jumpTask);
            }
        }
        
        private void LoadINI()
        {
            MyINI ini = new MyINI();

            StringCollection KeyList = new StringCollection();
            ini.ReadSection("JumpList", KeyList);
            foreach (string key in KeyList)
            {
                JumpTask jumpTask = new JumpTask
                {
                    ApplicationPath = ini.ReadString("JumpList", key, ""),
                    IconResourcePath = ini.ReadString("JumpList", key, ""),
                    Title = key,
                    Description = key
                };
                this.JTData.Add(jumpTask);
            }
        }

        private void SaveINI()
        {
            MyINI ini = new MyINI();
            ini.EraseSection("JumpList");
            foreach (JumpTask jumpTask in this.JTData)
            {
                ini.WriteString("JumpList", jumpTask.Title, jumpTask.ApplicationPath);
            }
        }

    }
}

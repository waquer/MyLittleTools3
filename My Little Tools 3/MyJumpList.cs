using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Shell;

namespace MyLittleTools3
{
    class MyJumpList
    {

        public ObservableCollection<JumpTask> JTData = new ObservableCollection<JumpTask>();
        private JumpList jumpList = JumpList.GetJumpList(App.Current);

        public MyJumpList()
        {
            this.LoadINI();
        }

        public void add(String path, String name="", String desc="")
        {
            if (name=="")
            {
                name = Path.GetFileNameWithoutExtension(path);
            }

            if (desc == "")
            {
                desc = name;
            }

            JumpTask jumpTask = new JumpTask();
            jumpTask.ApplicationPath = path;
            jumpTask.IconResourcePath = path;
            jumpTask.Title = name;
            jumpTask.Description = desc;

            this.JTData.Add(jumpTask);
        }

        public void clearAll()
        {
            this.JTData.Clear();
            //this.setJumpList();
        }

        public void removeAt(int idx)
        {
            if (idx > -1 && idx < JTData.Count)
            {
                this.JTData.RemoveAt(idx);
            }
        }

        public void moveUp(int idx)
        {
            if (idx > 0)
            {
                JumpTask jt = JTData[idx];
                JTData.RemoveAt(idx);
                JTData.Insert(idx-1, jt);
            }
        }

        public void moveDown(int idx)
        {
            if (idx < JTData.Count - 1)
            {
                JumpTask jt = JTData[idx];
                JTData.RemoveAt(idx);
                JTData.Insert(idx + 1, jt);
            }
        }

        public void setJumpList()
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
            this.appendDefault();
            this.jumpList.Apply();
            this.SaveINI();
        }

        private void appendDefault()
        {
            String appPath = System.Reflection.Assembly.GetEntryAssembly().Location;

            String[,] argarry =
            {
                {"修改Hosts","-edithosts"}
            };

            for (int i = 0; i < argarry.GetLength(0); i++)
            {
                JumpTask jumpTask = new JumpTask();
                jumpTask.ApplicationPath = appPath;
                jumpTask.IconResourcePath = appPath;
                jumpTask.Title = argarry[i, 0];
                jumpTask.Description = argarry[i, 0];
                jumpTask.Arguments = argarry[i, 1];
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
                JumpTask jumpTask = new JumpTask();
                jumpTask.ApplicationPath = ini.ReadString("JumpList", key, "");
                jumpTask.IconResourcePath = ini.ReadString("JumpList", key, "");
                jumpTask.Title = key;
                jumpTask.Description = key;
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

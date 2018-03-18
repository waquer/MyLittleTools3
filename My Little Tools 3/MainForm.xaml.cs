using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        MyJumpList myJumpList = new MyJumpList();
        MyFileTool myFileTool = new MyFileTool();
        NotifyIcon notifyIcon = new NotifyIcon();
        NotifyMenu notifyMenu = new NotifyMenu();

        public MainWindow(int tabidx = 0)
        {
            InitializeComponent();
            btnUpdateSelf.Content = App.ResourceAssembly.GetName(false).Version;
            lsvJumpList.ItemsSource = myJumpList.JTData;
            ListFiles.ItemsSource = myFileTool.fileList;
            tabMain.SelectedIndex = tabidx;
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.ContextMenuStrip = notifyMenu.Instance();
            notifyIcon.MouseClick += NotifyIcon_Click;
        }

        private void NotifyIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip.Show();
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        #region 跳转列表

        /* 设置JumpList */
        private void BtnSetJumpList_Click(object sender, RoutedEventArgs e)
        {
            myJumpList.SetJumpList();
        }

        /* 清空JumpList */
        private void BtnClearJumpList_Click(object sender, RoutedEventArgs e)
        {
            myJumpList.ClearAll();
        }

        /* 选择要向JL列表中添加的文件 */
        private void BtnSelectJumpFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog
            {
                FileName = "程序",
                Filter = "可执行文件|*.exe",
                DefaultExt = ".exe",
                AddExtension = true
            };
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filename = OpenFileD.FileNames[0];
                tbJFilePath.Text = filename;
                tbJFileName.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        /* 向JL列表中添加文件 */
        private void BtnAddJumpFile_Click(object sender, RoutedEventArgs e)
        {
            if (tbJFilePath.Text != "" && tbJFileName.Text != "")
            {
                myJumpList.Add(tbJFilePath.Text, tbJFileName.Text);
            }
        }

        /* 从JL列表中删除一个文件 */
        private void BtnDelJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.RemoveAt(sidx);
            }
        }

        /* JumpList文件上移 */
        private void BtnUpJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.MoveUp(sidx);
                lsvJumpList.SelectedIndex = sidx - 1;
            }
        }

        /* JumpList文件下移 */
        private void BtnDownJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.MoveDown(sidx);
                lsvJumpList.SelectedIndex = sidx + 1;
            }
        }

        #endregion

        #region 文字处理

        /* 转为简体 */
        private void CoverToSimplified(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = Strings.StrConv(tbTextInput.Text, VbStrConv.SimplifiedChinese, 0);
        }

        /* 转为繁体 */
        private void CoverToTraditional(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = Strings.StrConv(tbTextInput.Text, VbStrConv.TraditionalChinese, 0);
        }

        /* 全部小写 */
        private void AllToLower(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = tbTextInput.Text.ToLowerInvariant();
        }

        /* 全部大写 */
        private void AllToUpper(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = tbTextInput.Text.ToUpperInvariant();
        }

        /* 首字母大写 */
        private void InitialToUpper(object sender, RoutedEventArgs e)
        {
            Char[] tmpchar = tbTextInput.Text.Trim().ToCharArray();
            String tmpstr = tmpchar[0].ToString().ToUpperInvariant();
            for (int i = 1; i < tmpchar.Length; i++)
            {
                if (tmpstr[i - 1] == ' ')
                {
                    tmpstr += tmpchar[i].ToString().ToUpperInvariant();
                }
                else
                {
                    tmpstr += tmpchar[i].ToString().ToLowerInvariant();
                }
            }
            tbTextOutput.Text = tmpstr;
        }

        /* 转换全拼 */
        private void CoverToPYF(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = MyTextTool.GetPinyinFull(tbTextInput.Text);
        }

        /* 全拼首字母 */
        private void CoverToPYI(object sender, RoutedEventArgs e)
        {
            tbTextOutput.Text = MyTextTool.GetPinyinInit(tbTextInput.Text);
        }

        #endregion

        #region 编码转换

        private void CodeFile_add(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog();
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbCodeInput.Text = OpenFileD.FileName;
            }
        }

        private void DoCoding(String codeType)
        {
            MyCodeTool codeTool = new MyCodeTool
            {
                CodeType = codeType
            };
            codeTool.CodeCharset = codeCharset.Text;
            codeTool.CodeMethod = codeMethod.Text;
            codeTool.InputString = tbCodeInput.Text;
            tbCodeOutput.Text = codeTool.DoCoding();
        }


        private void DoEncode(object sender, RoutedEventArgs e)
        {
            DoCoding("encode");
        }

        private void DoDecode(object sender, RoutedEventArgs e)
        {
            DoCoding("decode");
        }

        #endregion

        #region 文件处理

        // 添加文件
        private void ListFiles_add(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog
            {
                Multiselect = true
            };
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String[] files = OpenFileD.FileNames;
                foreach (String file in files)
                {
                    myFileTool.fileList.Add(file);
                }
            }
        }

        // 清空列表
        private void ListFiles_clr(object sender, RoutedEventArgs e)
        {
            myFileTool.fileList.Clear();
            ListFiles.SelectedIndex = -1;
        }

        // 删除所选
        private void ListFiles_del(object sender, RoutedEventArgs e)
        {
            int sidx = ListFiles.SelectedIndex;
            int count = myFileTool.fileList.Count;

            if (sidx > -1 && sidx < count)
            {
                myFileTool.fileList.RemoveAt(sidx);

                if (count < 1)
                    ListFiles.SelectedIndex = -1;
                else if (sidx < count - 1)
                    ListFiles.SelectedIndex = sidx;
                else
                    ListFiles.SelectedIndex = sidx - 1;
            }
        }

        // 选择项改变,读取文件信息
        private void ListFiles_selectchange(object sender, SelectionChangedEventArgs e)
        {
            if (ListFiles.SelectedItem != null)
            {
                MyFileAttr fileAttr = new MyFileAttr(ListFiles.SelectedItem.ToString());
                cbAttrs.DataContext = fileAttr;
            }
        }

        private void RenameByT_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            renameByT.IsChecked = true;
        }

        private void RenameByR_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            renameByR.IsChecked = true;
        }

        // 重命名
        private void BtnRename_Click(object sender, RoutedEventArgs e)
        {

            if (renameByR.IsChecked == true)
            {
                myFileTool.fileSource = renameReSor.Text;
                myFileTool.fileTarget = renameReTar.Text;
            }
            else if (renameByT.IsChecked == true)
            {
                myFileTool.fileSource = renameTemplate.Text;
                myFileTool.fileTarget = null;
            }
            else
            {
                System.Windows.MessageBox.Show("请选择一种方式");
                return;
            }

            MessageBoxResult mbr = System.Windows.MessageBox.Show(myFileTool.BatchRename(), "确定执行重命名吗？", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                myFileTool.DoRename();
            }
        }

        #endregion

        #region 杂项

        /* 更新程序 */
        private void UpdateSelf(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("此操作将覆盖原程序，确定吗？", "程序更新", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                String FileNew = System.Reflection.Assembly.GetEntryAssembly().Location;
                String FileOld = @"D:\My Little Tools\My Little Tools.exe";
                File.Copy(FileNew, FileOld, true);
            }
        }

        /* 设置窗体总在最上状态 */
        private void CbOnTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (cbOnTop.IsChecked == true) ? true : false;
        }

        /* 最小化时显示托盘图标 */
        private void WinMain_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
            }
            else
            {
                notifyIcon.Visible = false;
                ShowInTaskbar = true;
            }
           
        }


        #endregion

    }
}

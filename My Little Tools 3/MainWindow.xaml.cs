using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Web;

namespace MyLittleTools3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        MyJumpList myJumpList = new MyJumpList();
        MyFileTool myFileTool = new MyFileTool();

        public MainWindow()
        {
            InitializeComponent();
            btnUpdateSelf.Content = App.ResourceAssembly.GetName(false).Version;
            lsvJumpList.ItemsSource = myJumpList.JTData;
            ListFiles.ItemsSource = myFileTool.fileList;
        }

        #region 跳转列表

        /* 设置JumpList */
        private void btnSetJumpList_Click(object sender, RoutedEventArgs e)
        {
            myJumpList.setJumpList();
        }

        /* 清空JumpList */
        private void btnClearJumpList_Click(object sender, RoutedEventArgs e)
        {
            myJumpList.clearAll();
        }

        /* 选择要向JL列表中添加的文件 */
        private void btnSelectJumpFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog();
            OpenFileD.FileName = "程序";
            OpenFileD.Filter = "可执行文件|*.exe";
            OpenFileD.DefaultExt = ".exe";
            OpenFileD.AddExtension = true;
            if (OpenFileD.ShowDialog() == true)
            {
                String filename = OpenFileD.FileNames[0];
                tbJFilePath.Text = filename;
                tbJFileName.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        /* 向JL列表中添加文件 */
        private void btnAddJumpFile_Click(object sender, RoutedEventArgs e)
        {
            if (tbJFilePath.Text != "" && tbJFileName.Text != "")
            {
                myJumpList.add(tbJFilePath.Text, tbJFileName.Text);
            }
        }

        /* 从JL列表中删除一个文件 */
        private void btnDelJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.removeAt(sidx);
            }
        }

        /* JumpList文件上移 */
        private void btnUpJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.moveUp(sidx);
                lsvJumpList.SelectedIndex = sidx - 1;
            }
        }

        /* JumpList文件下移 */
        private void btnDownJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1)
            {
                myJumpList.moveDown(sidx);
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
            OpenFileD.Multiselect = false;
            if (OpenFileD.ShowDialog() == true)
            {
                tbCodeInput.Text = OpenFileD.FileName;
            }
        }

        private void DoCoding(String codeType)
        {
            MyCodeTool codeTool = new MyCodeTool();

            codeTool.inputString = tbCodeInput.Text;

            codeTool.codeType = codeType;

            if (rbCodeASCII.IsChecked == true)
            {
                codeTool.codeCharset = "ASCII";
            }
            else
            {
                codeTool.codeCharset = "UTF-8";
            }

            if (rbCodeMD5.IsChecked == true)
            {
                codeTool.codeMethod = "MD5";
            }
            else if (rbCodeSHA1.IsChecked == true)
            {
                codeTool.codeMethod = "SHA1";
            }
            else if (rbCodeBASE64.IsChecked == true)
            {
                codeTool.codeMethod = "BASE64";
            }
            else
            {
                codeTool.codeMethod = "URL";
            }

            tbCodeOutput.Text = codeTool.doCoding();
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
            OpenFileDialog OpenFileD = new OpenFileDialog();
            OpenFileD.Multiselect = true;
            if (OpenFileD.ShowDialog() == true)
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

        private void renameByT_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            renameByT.IsChecked = true;
        }

        private void renameByR_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            renameByR.IsChecked = true;
        }

        // 重命名
        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            
            if (renameByR.IsChecked == true)
            {
                myFileTool.brRepSor = renameReSor.Text;
                myFileTool.brRepTar = renameReTar.Text;
            }
            else if (renameByT.IsChecked == true)
            {
                myFileTool.brRepSor = renameTemplate.Text;
                myFileTool.brRepTar = null;
            }
            else
            {
                MessageBox.Show("请选择一种方式");
                return;
            }

            MessageBoxResult mbr = MessageBox.Show(myFileTool.batchRename(), "确定执行重命名吗？", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                myFileTool.doRename();
            }
        }

        #endregion

        #region 杂项

        /* 更新程序 */
        private void UpdateSelf(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("此操作将覆盖原程序，确定吗？", "程序更新", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                String FileNew = System.Reflection.Assembly.GetEntryAssembly().Location;
                String FileOld = @"D:\My Little Tools\My Little Tools.exe";
                File.Copy(FileNew, FileOld, true);
            }
        }

        /* 设置窗体总在最上状态 */
        private void cbOnTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (cbOnTop.IsChecked == true) ? true : false;
        }

        #endregion

        

        






    }
}

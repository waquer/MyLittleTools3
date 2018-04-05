using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// 跳转列表
    /// </summary>
    public partial class MainWindow : Window
    {

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
            OpenFileDialog OpenFileD = new OpenFileDialog {
                FileName = "程序",
                Filter = "可执行文件|*.exe",
                DefaultExt = ".exe",
                AddExtension = true
            };
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                String filename = OpenFileD.FileNames[0];
                tbJFilePath.Text = filename;
                tbJFileName.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        /* 向JL列表中添加文件 */
        private void BtnAddJumpFile_Click(object sender, RoutedEventArgs e)
        {
            if (tbJFilePath.Text != "" && tbJFileName.Text != "") {
                myJumpList.Add(tbJFilePath.Text, tbJFileName.Text);
            }
        }

        /* 从JL列表中删除一个文件 */
        private void BtnDelJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1) {
                myJumpList.RemoveAt(sidx);
            }
        }

        /* JumpList文件上移 */
        private void BtnUpJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1) {
                myJumpList.MoveUp(sidx);
                lsvJumpList.SelectedIndex = sidx - 1;
            }
        }

        /* JumpList文件下移 */
        private void BtnDownJumpFile_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lsvJumpList.SelectedIndex;
            if (sidx > -1) {
                myJumpList.MoveDown(sidx);
                lsvJumpList.SelectedIndex = sidx + 1;
            }
        }

    }
}

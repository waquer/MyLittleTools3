using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// 跳转列表
    /// </summary>
    public partial class MainWindow
    {

        /* 设置JumpList */
        private void BtnSetJumpList_Click(object sender, RoutedEventArgs e)
        {
            _myJumpList.SetJumpList();
        }

        /* 清空JumpList */
        private void BtnClearJumpList_Click(object sender, RoutedEventArgs e)
        {
            _myJumpList.ClearAll();
        }

        /* 选择要向JL列表中添加的文件 */
        private void BtnSelectJumpFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileD = new OpenFileDialog {
                FileName = "程序",
                Filter = @"可执行文件|*.exe",
                DefaultExt = ".exe",
                AddExtension = true
            };
            if (openFileD.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var filename = openFileD.FileNames[0];
            TbJFilePath.Text = filename;
            TbJFileName.Text = Path.GetFileNameWithoutExtension(filename);
        }

        /* 向JL列表中添加文件 */
        private void BtnAddJumpFile_Click(object sender, RoutedEventArgs e)
        {
            if (TbJFilePath.Text != "" && TbJFileName.Text != "") {
                _myJumpList.Add(TbJFilePath.Text, TbJFileName.Text);
            }
        }

        /* 从JL列表中删除一个文件 */
        private void BtnDelJumpFile_Click(object sender, RoutedEventArgs e)
        {
            var sidx = LsvJumpList.SelectedIndex;
            if (sidx > -1) {
                _myJumpList.RemoveAt(sidx);
            }
        }

        /* JumpList文件上移 */
        private void BtnUpJumpFile_Click(object sender, RoutedEventArgs e)
        {
            var sidx = LsvJumpList.SelectedIndex;
            if (sidx <= -1) return;
            _myJumpList.MoveUp(sidx);
            LsvJumpList.SelectedIndex = sidx - 1;
        }

        /* JumpList文件下移 */
        private void BtnDownJumpFile_Click(object sender, RoutedEventArgs e)
        {
            var sidx = LsvJumpList.SelectedIndex;
            if (sidx <= -1) return;
            _myJumpList.MoveDown(sidx);
            LsvJumpList.SelectedIndex = sidx + 1;
        }

    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MyLittleTools3.MyTools;

namespace MyLittleTools3
{
    /// <summary>
    /// 文件处理
    /// </summary>
    public partial class MainWindow
    {

        // 添加文件
        private void ListFiles_add(object sender, RoutedEventArgs e)
        {
            var openFileD = new OpenFileDialog {
                Multiselect = true
            };
            if (openFileD.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var files = openFileD.FileNames;
            foreach (var file in files) {
                _myFileTool.FileList.Add(file);
            }
        }

        // 清空列表
        private void ListFiles_clr(object sender, RoutedEventArgs e)
        {
            _myFileTool.FileList.Clear();
            ListFiles.SelectedIndex = -1;
        }

        // 删除所选
        private void ListFiles_del(object sender, RoutedEventArgs e)
        {
            var sidx = ListFiles.SelectedIndex;
            var count = _myFileTool.FileList.Count;

            if (sidx <= -1 || sidx >= count) return;
            _myFileTool.FileList.RemoveAt(sidx);

            if (sidx < count - 1)
                ListFiles.SelectedIndex = sidx;
            else
                ListFiles.SelectedIndex = sidx - 1;
        }

        // 选择项改变,读取文件信息
        private void ListFiles_selectchange(object sender, SelectionChangedEventArgs e)
        {
            if (ListFiles.SelectedItem == null) return;
            var fileAttr = new MyFileAttr(ListFiles.SelectedItem.ToString());
            CbAttrs.DataContext = fileAttr;
        }

        private void RenameByT_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            RenameByT.IsChecked = true;
        }

        private void RenameByR_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            RenameByR.IsChecked = true;
        }

        // 重命名
        private void BtnRename_Click(object sender, RoutedEventArgs e)
        {

            if (RenameByR.IsChecked == true) {
                _myFileTool.FileSource = RenameReSor.Text;
                _myFileTool.FileTarget = RenameReTar.Text;
            } else if (RenameByT.IsChecked == true) {
                _myFileTool.FileSource = RenameTemplate.Text;
                _myFileTool.FileTarget = null;
            } else {
                System.Windows.MessageBox.Show("请选择一种方式");
                return;
            }

            var mbr = System.Windows.MessageBox.Show(_myFileTool.BatchRename(), "确定执行重命名吗？", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) {
                _myFileTool.DoRename();
            }
        }

    }
}

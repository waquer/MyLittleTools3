using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// 文件处理
    /// </summary>
    public partial class MainWindow : Window
    {

        // 添加文件
        private void ListFiles_add(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog {
                Multiselect = true
            };
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                String[] files = OpenFileD.FileNames;
                foreach (String file in files) {
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

            if (sidx > -1 && sidx < count) {
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
            if (ListFiles.SelectedItem != null) {
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

            if (renameByR.IsChecked == true) {
                myFileTool.fileSource = renameReSor.Text;
                myFileTool.fileTarget = renameReTar.Text;
            } else if (renameByT.IsChecked == true) {
                myFileTool.fileSource = renameTemplate.Text;
                myFileTool.fileTarget = null;
            } else {
                System.Windows.MessageBox.Show("请选择一种方式");
                return;
            }

            MessageBoxResult mbr = System.Windows.MessageBox.Show(myFileTool.BatchRename(), "确定执行重命名吗？", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) {
                myFileTool.DoRename();
            }
        }

    }
}

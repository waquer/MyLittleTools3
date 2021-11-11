using System;
using System.Windows;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// 编码转换
    /// </summary>
    public partial class MainWindow : Window
    {

        private void CodeFile_add(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileD = new OpenFileDialog();
            if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                tbCodeInput.Text = OpenFileD.FileName;
            }
        }

        private void DoCoding(String codeType)
        {
            MyCodeTool codeTool = new MyCodeTool {
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

    }
}

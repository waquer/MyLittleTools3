using System.Windows;
using System.Windows.Forms;
using MyLittleTools3.MyTools;

namespace MyLittleTools3
{
    /// <summary>
    /// 编码转换
    /// </summary>
    public partial class MainWindow
    {

        private void CodeFile_add(object sender, RoutedEventArgs e)
        {
            var openFileD = new OpenFileDialog();
            if (openFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                TbCodeInput.Text = openFileD.FileName;
            }
        }

        private void DoCoding(string codeType)
        {
            var codeTool = new MyCodeTool {
                CodeType = codeType,
                CodeCharset = CodeCharset.Text,
                CodeMethod = CodeMethod.Text,
                InputString = TbCodeInput.Text
            };
            TbCodeOutput.Text = codeTool.DoCoding();
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

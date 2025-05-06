using Microsoft.VisualBasic;
using System.Windows;
using MyLittleTools3.MyTools;

namespace MyLittleTools3
{
    /// <summary>
    /// 文字处理
    /// </summary>
    public partial class MainWindow
    {

        /* 转为简体 */
        private void CoverToSimplified(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = Strings.StrConv(TbTextInput.Text, VbStrConv.SimplifiedChinese);
        }

        /* 转为繁体 */
        private void CoverToTraditional(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = Strings.StrConv(TbTextInput.Text, VbStrConv.TraditionalChinese);
        }

        /* 全部小写 */
        private void AllToLower(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = TbTextInput.Text.ToLowerInvariant();
        }

        /* 全部大写 */
        private void AllToUpper(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = TbTextInput.Text.ToUpperInvariant();
        }

        /* 首字母大写 */
        private void InitialToUpper(object sender, RoutedEventArgs e)
        {
            var tmpchar = TbTextInput.Text.Trim().ToCharArray();
            var tmpstr = tmpchar[0].ToString().ToUpperInvariant();
            for (var i = 1; i < tmpchar.Length; i++) {
                if (tmpstr[i - 1] == ' ') {
                    tmpstr += tmpchar[i].ToString().ToUpperInvariant();
                } else {
                    tmpstr += tmpchar[i].ToString().ToLowerInvariant();
                }
            }
            TbTextOutput.Text = tmpstr;
        }

        /* 转换全拼 */
        private void CoverToPyf(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = MyTextTool.GetPinyinFull(TbTextInput.Text);
        }

        /* 全拼首字母 */
        private void CoverToPyi(object sender, RoutedEventArgs e)
        {
            TbTextOutput.Text = MyTextTool.GetPinyinInit(TbTextInput.Text);
        }

    }
}

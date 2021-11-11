using Microsoft.VisualBasic;
using System;
using System.Windows;

namespace MyLittleTools3
{
    /// <summary>
    /// 文字处理
    /// </summary>
    public partial class MainWindow : Window
    {

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
            for (int i = 1; i < tmpchar.Length; i++) {
                if (tmpstr[i - 1] == ' ') {
                    tmpstr += tmpchar[i].ToString().ToUpperInvariant();
                } else {
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

    }
}

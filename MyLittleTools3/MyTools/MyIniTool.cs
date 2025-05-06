using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MyLittleTools3.MyTools
{
    internal static class MyIniTool
    {
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal,
            int size, string filePath);

        private static readonly string IniFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "My Little Tools.ini");

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Ident"></param>
        /// <param name="Value"></param>
        public static void WriteString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, IniFile))
            {
                throw (new ApplicationException("写入INI文件出错"));
            }
        }

        /// <summary>
        /// 读INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Ident"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string ReadString(string Section, string Ident, string Default)
        {
            var buffer = new Byte[65535];
            var bufLen = GetPrivateProfileString(Section, Ident, Default, buffer, buffer.GetUpperBound(0), IniFile);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            var s = Encoding.GetEncoding(0).GetString(buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        /// <summary>
        /// 读整数
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Ident"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static int ReadInteger(string Section, string Ident, int Default)
        {
            var intStr = ReadString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }

        /// <summary>
        /// 写整数
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Ident"></param>
        /// <param name="Value"></param>
        public static void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        /// <summary>
        /// 将指定的Section名称中的所有Ident添加到列表中
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Idents"></param>
        public static void ReadSection(string Section, StringCollection Idents)
        {
            var buffer = new byte[16384];
            //Idents.Clear();
            var bufLen = GetPrivateProfileString(Section, null, null, buffer, buffer.GetUpperBound(0), IniFile);
            //对Section进行解析
            GetStringsFromBuffer(buffer, bufLen, Idents);
        }

        /// <summary>
        /// 读取所有的Sections的名称
        /// </summary>
        /// <param name="SectionList"></param>
        public static void ReadSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个 Section
            var buffer = new byte[65535];
            var bufLen = GetPrivateProfileString(null, null, null, buffer, buffer.GetUpperBound(0), IniFile);
            GetStringsFromBuffer(buffer, bufLen, SectionList);
        }

        /// <summary>
        /// 清除某个Section
        /// </summary>
        /// <param name="Section"></param>
        public static void EraseSection(string Section)
        {
            if (!WritePrivateProfileString(Section, null, null, IniFile))
            {
                throw (new ApplicationException("清除INI文件出错"));
            }
        }

        private static void GetStringsFromBuffer(byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen == 0) return;
            var start = 0;
            for (var i = 0; i < bufLen; i++)
            {
                if ((Buffer[i] != 0) || ((i - start) <= 0)) continue;
                var s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                Strings.Add(s);
                start = i + 1;
            }
        }
    }
}
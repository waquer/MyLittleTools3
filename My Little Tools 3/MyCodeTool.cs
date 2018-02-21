using System;
using System.IO;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;


namespace MyLittleTools3
{
    class MyCodeTool
    {
        private Encoding MyEncoding;
        private Boolean IfDecode;

        public String InputString;
        public String CodeMethod;
        public String CodeCharset
        {
            set
            {
                if (value.ToLower() == "ascii")
                    MyEncoding = ASCIIEncoding.Default;
                else
                    MyEncoding = UTF8Encoding.Default;
            }
        }
        public String CodeType
        {
            set {
                IfDecode = value.ToLower() == "decode";
            }
        }

        public MyCodeTool()
        {
            InputString = "";
            CodeMethod = "";
            IfDecode = false;
            MyEncoding = UTF8Encoding.Default;
        }

        public String DoCoding()
        {
            switch (CodeMethod)
            {
                case "BASE64":
                    return IfDecode ? DoBase64Decode() : DoBase64Encode();
                case "URL":
                    return IfDecode ? DoUrlDecode() : DoUrlEncode();
                default:
                    return IfDecode ? "暂不支持" : DoHash(CodeMethod);
            }
        }

        /* HASH编码 */
        private String DoHash(String method)
        {
            HashAlgorithm algorithm;

            switch (method)
            {
                case "MD5":
                    algorithm = MD5.Create();
                    break;
                case "SHA1":
                    algorithm = SHA1.Create();
                    break;
                case "SHA256":
                    algorithm = SHA256.Create();
                    break;
                default:
                    return "暂不支持";
            }

            byte[] hashBytes;
            String result = "";

            if (File.Exists(InputString))
            {
                FileStream fs = new FileStream(InputString, FileMode.Open, FileAccess.Read);
                hashBytes = algorithm.ComputeHash(fs);
                fs.Close();
            }
            else
            {
                hashBytes = algorithm.ComputeHash(MyEncoding.GetBytes(InputString));
            }
            for (int i = 0; i < hashBytes.Length; i++)
                result += hashBytes[i].ToString("x").PadLeft(2, '0');
            result = result.ToUpper();
            return result;
        }

        /* URL编码 */
        private String DoUrlEncode()
        {
            return HttpUtility.UrlEncode(InputString);
        }

        /* URL解码 */
        private String DoUrlDecode()
        {
            return HttpUtility.UrlDecode(InputString);
        }

        /* BASE64编码 */
        private String DoBase64Encode()
        {
            if (File.Exists(InputString))
            {
                String result = "";
                Image img = Image.FromFile(InputString);
                MemoryStream mstream = new MemoryStream();
                switch (Path.GetExtension(InputString).ToLower())
                {
                    case ".bmp":
                        img.Save(mstream, ImageFormat.Bmp);
                        result = "data:image/bmp;base64," + Convert.ToBase64String(mstream.GetBuffer());
                        break;
                    case ".png":
                        img.Save(mstream, ImageFormat.Png);
                        result = "data:image/png;base64," + Convert.ToBase64String(mstream.GetBuffer());
                        break;
                    case ".gif":
                        img.Save(mstream, ImageFormat.Gif);
                        result = "data:image/gif;base64," + Convert.ToBase64String(mstream.GetBuffer());
                        break;
                    case ".ico":
                        img.Save(mstream, ImageFormat.Icon);
                        result = "data:image/icon;base64," + Convert.ToBase64String(mstream.GetBuffer());
                        break;
                    case ".jpg":
                    case ".jpge":
                        img.Save(mstream, ImageFormat.Jpeg);
                        result = "data:image/jpge;base64," + Convert.ToBase64String(mstream.GetBuffer());
                        break;
                    default:
                        result = "无法识别的类型";
                        break;
                }
                mstream.Dispose();
                img.Dispose();
                return result;
            }
            else
            {

                byte[] bytedata = MyEncoding.GetBytes(InputString);
                return Convert.ToBase64String(bytedata, 0, bytedata.Length);
            }
        }

        /* BASE64解码 */
        private String DoBase64Decode()
        {
            try
            {
                Byte[] bytes = Convert.FromBase64String(InputString);
                return MyEncoding.GetString(bytes);
            }
            catch
            {
                return "不是有效的BASE64编码";
            }
            
        }

    }
}

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

        public String inputString;
        public String codeMethod;
        public String codeCharset
        {
            set
            {
                if (value.ToLower() == "ascii")
                    MyEncoding = ASCIIEncoding.Default;
                else
                    MyEncoding = UTF8Encoding.Default;
            }
        }
        public String codeType
        {
            set {
                IfDecode = value.ToLower() == "decode";
            }
        }

        public MyCodeTool()
        {
            inputString = "";
            codeMethod = "";
            IfDecode = false;
            MyEncoding = UTF8Encoding.Default;
        }

        public String doCoding()
        {
            switch (codeMethod)
            {
                case "BASE64":
                    return IfDecode ? DoBase64Decode() : DoBase64Encode();
                case "URL":
                    return IfDecode ? DoUrlDecode() : DoUrlEncode();
                default:
                    return IfDecode ? "暂不支持" : DoHash(codeMethod);
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
                default:
                    return "暂不支持";
            }

            byte[] hashBytes;
            String result = "";

            if (File.Exists(inputString))
            {
                FileStream fs = new FileStream(inputString, FileMode.Open, FileAccess.Read);
                hashBytes = algorithm.ComputeHash(fs);
                fs.Close();
            }
            else
            {
                hashBytes = algorithm.ComputeHash(MyEncoding.GetBytes(inputString));
            }
            for (int i = 0; i < hashBytes.Length; i++)
                result += hashBytes[i].ToString("x").PadLeft(2, '0');
            result = result.ToUpper();
            return result;
        }

        /* URL编码 */
        private String DoUrlEncode()
        {
            return HttpUtility.UrlEncode(inputString);
        }

        /* URL解码 */
        private String DoUrlDecode()
        {
            return HttpUtility.UrlDecode(inputString);
        }

        /* BASE64编码 */
        private String DoBase64Encode()
        {
            if (File.Exists(inputString))
            {
                String result = "";
                Image img = Image.FromFile(inputString);
                MemoryStream mstream = new MemoryStream();
                switch (Path.GetExtension(inputString).ToLower())
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

                byte[] bytedata = MyEncoding.GetBytes(inputString);
                return Convert.ToBase64String(bytedata, 0, bytedata.Length);
            }
        }

        /* BASE64解码 */
        private String DoBase64Decode()
        {
            try
            {
                Byte[] bytes = Convert.FromBase64String(inputString);
                return MyEncoding.GetString(bytes);
            }
            catch
            {
                return "不是有效的BASE64编码";
            }
            
        }

    }
}

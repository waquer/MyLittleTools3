using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyLittleTools3.MyTools
{
    internal class MyCodeTool
    {
        private Encoding _myEncoding = Encoding.Default;
        private bool _ifDecode;

        public string InputString = "";
        public string CodeMethod = "";

        public string CodeCharset
        {
            set
            {
                InputString = value;
                _myEncoding = Encoding.Default;
            }
        }

        public string CodeType
        {
            set => _ifDecode = value.ToLower() == "decode";
        }

        public string DoCoding()
        {
            switch (CodeMethod)
            {
                case "BASE64":
                    return _ifDecode ? DoBase64Decode() : DoBase64Encode();
                case "URL":
                    return _ifDecode ? DoUrlDecode() : DoUrlEncode();
                default:
                    return _ifDecode ? "暂不支持" : DoHash(CodeMethod);
            }
        }

        /* HASH编码 */
        private string DoHash(string method)
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

            if (File.Exists(InputString))
            {
                var fs = new FileStream(InputString, FileMode.Open, FileAccess.Read);
                hashBytes = algorithm.ComputeHash(fs);
                fs.Close();
            }
            else
            {
                hashBytes = algorithm.ComputeHash(_myEncoding.GetBytes(InputString));
            }

            var result = hashBytes.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));

            result = result.ToUpper();
            return result;
        }

        /* URL编码 */
        private string DoUrlEncode()
        {
            return HttpUtility.UrlEncode(InputString);
        }

        /* URL解码 */
        private string DoUrlDecode()
        {
            return HttpUtility.UrlDecode(InputString);
        }

        /* BASE64编码 */
        private string DoBase64Encode()
        {
            if (File.Exists(InputString))
            {
                string result;
                var img = Image.FromFile(InputString);
                var mstream = new MemoryStream();
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
                var bytedata = _myEncoding.GetBytes(InputString);
                return Convert.ToBase64String(bytedata, 0, bytedata.Length);
            }
        }

        /* BASE64解码 */
        private string DoBase64Decode()
        {
            try
            {
                var bytes = Convert.FromBase64String(InputString);
                return _myEncoding.GetString(bytes);
            }
            catch
            {
                return "不是有效的BASE64编码";
            }
        }
    }
}
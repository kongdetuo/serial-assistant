using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace WPFSerialAssistant
{
    public static class Utilities
    {
        /// <summary>
        /// 使用指定分隔符连接集合成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="separator">分隔符, 默认为 " "</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> ts, string separator = " ")
        {
            return string.Join(separator, ts);
        }

        /// <summary>
        /// 使用指定分隔符分隔字符串并删除空白
        /// </summary>
        /// <param name="textData"></param>
        /// <param name="separator">分隔符, 默认为 ' '</param>
        /// <returns></returns>
        public static string[] SplitWithoutEmpty(this string textData, char separator = ' ')
        {
            return textData.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static string BytesToText(List<byte> bytesBuffer, ReceiveMode mode, Encoding encoding)
        {
            //StringBuilder sb = new StringBuilder();
            ////string result = "";

            //if (mode == ReceiveMode.Character)
            //{
            //    return encoding.GetString(bytesBuffer.ToArray<byte>());
            //}

            //foreach (var item in bytesBuffer)
            //{
            //    switch (mode)
            //    {
            //        case ReceiveMode.Hex:
            //            {
            //                string tmp = Convert.ToString(item, 16).ToUpper();
            //                if (tmp.Length < 2)
            //                {
            //                    sb.Append("0");
            //                }
            //                sb.Append(tmp).Append(" ");
            //            }
            //            break;

            //        case ReceiveMode.Decimal:
            //            sb.Append(Convert.ToString(item, 10)).Append(" ");
            //            break;

            //        case ReceiveMode.Octal:
            //            sb.Append(Convert.ToString(item, 8)).Append(" ");
            //            break;

            //        case ReceiveMode.Binary:
            //            {
            //                string tmp = Convert.ToString(item, 2);
            //                if (tmp.Length < 8)
            //                {
            //                    sb.Append('0', 8 - tmp.Length);
            //                }
            //                sb.Append(tmp).Append(" ");
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}

            var str = "";
            switch (mode)
            {
                case ReceiveMode.Character:
                    str = encoding.GetString(bytesBuffer.ToArray());
                    break;

                case ReceiveMode.Hex:
                    str = string.Join(" ", bytesBuffer.Select(item => item.ToString("X2")));
                    break;

                case ReceiveMode.Decimal:
                    str = string.Join(" ", bytesBuffer.Select(item => item.ToString("D")));
                    break;

                case ReceiveMode.Octal:
                    str = string.Join(" ", bytesBuffer.Select(item => Convert.ToString(item, 8)));
                    break;

                case ReceiveMode.Binary:
                    str = string.Join(" ", bytesBuffer.Select(item => Convert.ToString(item, 2)));
                    break;
            }

            return str;
        }

        public static string ToSpecifiedText(string text, SendMode mode, Encoding encoding)
        {
            string result = "";
            switch (mode)
            {
                case SendMode.Character:
                    //text = text.Trim();

                    //// 转换成字节
                    //List<byte> src = new List<byte>();

                    //string[] grp = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //foreach (var item in grp)
                    //{
                    //    src.Add(Convert.ToByte(item, 16));
                    //}

                    //// 转换成字符串
                    //result = encoding.GetString(src.ToArray<byte>());

                    result = encoding.GetString(text
                        .SplitWithoutEmpty()
                        .Select(item => Convert.ToByte(item, 16))
                        .ToArray());
                    break;

                case SendMode.Hex:

                    //byte[] byteStr = encoding.GetBytes(text.ToCharArray());

                    //foreach (var item in byteStr)
                    //{
                    //    result += Convert.ToString(item, 16).ToUpper() + " ";
                    //}

                    result = encoding
                        .GetBytes(text.ToArray())
                        .Select(item => item.ToString("X")) // 转换为16进制
                        .Join();                            // 连接字符串
                    break;

                default:
                    break;
            }

            return result.Trim();
        }
    }
}
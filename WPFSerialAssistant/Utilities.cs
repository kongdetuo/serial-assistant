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

        /// <summary>
        /// 将 byte 转换为长度为8的二进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToString2(this byte b)
        {
            var str = Convert.ToString(b, 2);
            var sb = new StringBuilder();
            if (str.Length < 8)
            {
                sb.Append('0', 8 - str.Length);
            }
            sb.Append(str);
            return sb.ToString();
        }

        /// <summary>
        /// 使用指定编码将一个字节序列解码为一个字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        public static string BytesToText(List<byte> bytesBuffer, ReceiveMode mode, Encoding encoding)
        {
            var result = "";
            switch (mode)
            {
                case ReceiveMode.Character:
                    result = encoding.GetString(bytesBuffer.ToArray());
                    break;

                case ReceiveMode.Hex:
                    result = bytesBuffer
                        .Select(item => item.ToString("X2"))
                        .Join();
                    break;

                case ReceiveMode.Decimal:
                    result = bytesBuffer
                        .Select(item => item.ToString("D"))
                        .Join();
                    break;

                case ReceiveMode.Octal:
                    result = bytesBuffer
                        .Select(item => Convert.ToString(item, 8))
                        .Join();
                    break;

                case ReceiveMode.Binary:
                    result = bytesBuffer
                        .Select(item => item.ToString2())
                        .Join();
                    break;
            }

            return result;
        }

        public static string ToSpecifiedText(string text, SendMode mode, Encoding encoding)
        {
            string result = "";
            switch (mode)
            {
                case SendMode.Character:
                    result = text
                        .SplitWithoutEmpty()                        //分割字符串
                        .Select(item => Convert.ToByte(item, 16))   //转换为byte
                        .ToArray()
                        .GetString(encoding);
                    break;

                case SendMode.Hex:
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
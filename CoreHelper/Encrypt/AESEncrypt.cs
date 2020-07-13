using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Core.Encrypt
{
   public  class AESEncrypt
    {
        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param> 
        /// <returns></returns>  
        public static string Encrypt(string source, string keyVal, string ivVal)
        {
            var encoding = Encoding.UTF8;
            byte[] btKey = FormatByte(keyVal, encoding);
            byte[] btIv = FormatByte(ivVal, encoding);
            byte[] byteArray = encoding.GetBytes(source);
            string encrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param>  
        /// <returns></returns>  
        public static string Decryption(string source, string keyVal, string ivVal)
        {
            var encoding = Encoding.UTF8;
            byte[] btKey = FormatByte(keyVal, encoding);
            byte[] btIv = FormatByte(ivVal, encoding);
            byte[] byteArray = Convert.FromBase64String(source);
            string decrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = encoding.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        private static byte[] FormatByte(string strVal, Encoding encoding)
        {
            var base64String = Base64(strVal);
            while (base64String.Length < 16 && base64String.Length > 0)
                base64String += base64String;
            return encoding.GetBytes(base64String.Substring(0, 16).ToUpper());
        }

        private static string Base64(string source)
        {
            var btArray = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }
    }
}

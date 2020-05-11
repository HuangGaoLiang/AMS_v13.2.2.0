/****************************************************************************\
所属系统:招生系统
所属模块:公共库
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描述：用于微信数据的AES解密
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-18</para>
    /// </summary>
    public class AESHelper
    {

        protected AESHelper()
        {

        }

        /// <summary>  
        /// AES解密  
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-18</para> 
        /// </summary>  
        /// <param name="inputData">输入的数据encryptedData</param>  
        /// <param name="key">key</param>  
        /// <param name="iv">向量128</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static string AESDecrypt(string inputData,string key,string iv)
        {
                byte[] encryptedData = Convert.FromBase64String(inputData);

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(key); 
                rijndaelCipher.IV = Convert.FromBase64String(iv);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);
                return result;
        }

        /// <summary>
        /// Aes解密,这个是预留方法，有可能上面的有问题在使用此方法
        /// </summary>
        /// <param name="inputData">需要解密的字符串</param>
        /// <param name="key">密钥,长度不够时空格补齐,超过时从左截取</param>
        /// <param name="iv">偏移量,长度不够时空格补齐,超过时从左截取</param>
        /// <param name="keyLenth">秘钥长度,16 24 32</param>
        /// <param name="aesMode">解密模式</param>
        /// <param name="aesPadding">填充方式</param>
        /// <returns></returns>
        public static string AESDecrypt2(string inputData, string key, string iv, int keyLenth = 16)
        {
            if (!new List<int> { 16, 24, 32 }.Contains(keyLenth))
            {
                return null;//密钥的长度，16位密钥 = 128位，24位密钥 = 192位，32位密钥 = 256位。
            }
            var oldBytes = Convert.FromBase64String(inputData);
            var bKey = new Byte[keyLenth];
            Array.Copy(Convert.FromBase64String(key.PadRight(keyLenth)), bKey, keyLenth);
            var bIv = new Byte[16];
            Array.Copy(Convert.FromBase64String(iv.PadRight(16)), bIv, 16);

            var rijalg = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = bKey,
                IV = bIv,
            };
            var decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);
            var rtByte = decryptor.TransformFinalBlock(oldBytes, 0, oldBytes.Length);
            return Encoding.UTF8.GetString(rtByte);
        }

    }
}

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace WeDonekRpc.Helper
{
    /// <summary>
    /// Cryptography加解密
    /// </summary>
    public class Cryptography
    {
        /// <summary>
        /// AEAD_AES_256_GCM
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nonce"></param>
        /// <param name="encryptedData"></param>
        /// <param name="associatedData"></param>
        /// <returns></returns>
        public static string AEAD_AES_256_GCM(string key, string nonce, string encryptedData, string associatedData)
        {
            byte[] nonceBytes = Encoding.UTF8.GetBytes(nonce);
            byte[] associatedBytes = Encoding.UTF8.GetBytes(associatedData);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            //tag size is 16
            byte[] cipherBytes = encryptedBytes[..^16];
            byte[] tag = encryptedBytes[^16..];
            byte[] decryptedData = new byte[cipherBytes.Length];
            using (AesGcm cipher = new AesGcm(Encoding.UTF8.GetBytes(key)))
            {
                cipher.Decrypt(nonceBytes, cipherBytes, tag, decryptedData, associatedBytes);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        public static uint HostToNetworkOrder(uint inval)
        {
            uint outval = 0;
            for (int i = 0; i < 4; i++)
            {
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            }

            return outval;
        }

        public static int HostToNetworkOrder(int inval)
        {
            int outval = 0;
            for (int i = 0; i < 4; i++)
            {
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            }

            return outval;
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Input">密文</param>
        /// <param name="EncodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AES256_decrypt(string Input, string EncodingAESKey, bool isBasic64 = true)
        {
            byte[] Key = isBasic64 ? Convert.FromBase64String(EncodingAESKey + "=") : Encoding.UTF8.GetBytes(EncodingAESKey);
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            byte[] btmpMsg = _AES256_decrypt(Input, Iv, Key);
            return Encoding.UTF8.GetString(btmpMsg);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Input">密文</param>
        /// <param name="EncodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AES_decrypt(string Input, string EncodingAESKey)
        {
            byte[] Key = Convert.FromBase64String(EncodingAESKey + "=");
            byte[] Iv = new byte[16];
            Buffer.BlockCopy(Key, 0, Iv, 0, 16);
            byte[] btmpMsg = _AES_decrypt(Input, Iv, Key);
            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);
            byte[] bMsg = new byte[len];
            Buffer.BlockCopy(btmpMsg, 20, bMsg, 0, len);
            return Encoding.UTF8.GetString(bMsg);
        }
        public static string AES256_encrypt(string encryptStr, string privateKey)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(privateKey);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cTransform = rDel.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }
        public static string AES_encrypt(string Input, string EncodingAESKey, string appid)
        {
            byte[] Key = Convert.FromBase64String(EncodingAESKey + "=");
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            string Randcode = _CreateRandCode(16);
            byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
            byte[] bAppid = Encoding.UTF8.GetBytes(appid);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(Input);
            byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];
            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);
            return _AES_encrypt(bMsg, Iv, Key);

        }
        private static string _CreateRandCode(int codeLen)
        {
            string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            string[] arr = codeSerial.Split(',');
            string code = string.Empty;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }
        private static string _AES_encrypt(byte[] Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                //秘钥的大小，以位为单位
                KeySize = 256,
                //支持的块大小
                BlockSize = 128,
                //填充模式
                //aes.Padding = PaddingMode.PKCS7;
                Padding = PaddingMode.None,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = Iv
            };
            ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行
            byte[] msg = new byte[Input.Length + 32 - (Input.Length % 32)];
            Array.Copy(Input, msg, Input.Length);
            byte[] pad = _KCS7Encoder(Input.Length);
            Array.Copy(pad, 0, msg, Input.Length, pad.Length);
            #endregion

            #region 注释的也是一种方法，效果一样
            //ICryptoTransform transform = aes.CreateEncryptor();
            //byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
            #endregion

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            string Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static byte[] _KCS7Encoder(int text_length)
        {
            int block_size = 32;
            // 计算需要填充的位数
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            // 获得补位所用的字符
            char pad_chr = _Chr(amount_to_pad);
            string tmp = string.Empty;
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }

        /**
         * 将数字转化成ASCII码对应的字符，用于对明文进行补码
         * 
         * @param a 需要转化的数字
         * @return 转化得到的字符
         */
        private static char _Chr(int a)
        {

            byte target = (byte)(a & 0xFF);
            return (char)target;
        }
        private static byte[] _AES256_decrypt(string Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                Key = Key,
                IV = Iv
            };
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    byte[] msg = new byte[xXml.Length + 32 - (xXml.Length % 32)];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = _Decode2(ms.ToArray());
            }
            return xBuff;
        }
        private static byte[] _AES_decrypt(string Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                Key = Key,
                IV = Iv
            };
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    byte[] msg = new byte[xXml.Length + 32 - (xXml.Length % 32)];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = _Decode2(ms.ToArray());
            }
            return xBuff;
        }
        private static byte[] _Decode2(byte[] decrypted)
        {
            int pad = decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
    }
}

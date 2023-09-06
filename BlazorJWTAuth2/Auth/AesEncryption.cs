using System.Security.Cryptography;
using System.Text;

namespace BlazorJWTAuth2.Auth
{

    public class AesEncryption
    {
        private readonly string key = "ole5rEIKer3rlg8F"; // 16, 24 또는 32 바이트 길이의 키 사용
        private readonly string iv = "0123456789123456"; // 16 바이트 길이의 초기화 벡터 사용

        public AesEncryption()
        {
        }

        public byte[] Encrypt(byte[] data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                // AES 암호화스트림 생성
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(data, 0, data.Length);
                            csEncrypt.FlushFinalBlock();
                        }
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                // AES 복호화스트림 생성
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                        {
                            csDecrypt.Write(encryptedData, 0, encryptedData.Length);
                            csDecrypt.FlushFinalBlock();
                        }
                    }

                    return msDecrypt.ToArray();
                }
            }
        }
    }
}

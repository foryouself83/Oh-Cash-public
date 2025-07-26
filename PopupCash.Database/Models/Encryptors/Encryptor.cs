using System.Security.Cryptography;

namespace PopupCash.Database.Models.Encryptors
{
    public static class Encryptor
    {
        // Generate Key and IV using salt key
        public static void GenerateKeyAndIV(byte[] saltKey, out byte[] key, out byte[] iv)
        {
            int KeySize = 192; // 24 bytes for TripleDES
            int BlockSize = 64; // 8 bytes for TripleDES

            if (saltKey.Length < (KeySize / 8) + (BlockSize / 8))
                throw new ArgumentException("Salt key is too short.");

            key = new byte[KeySize / 8];
            iv = new byte[BlockSize / 8];

            Array.Copy(saltKey, 0, key, 0, KeySize / 8);
            Array.Copy(saltKey, KeySize / 8, iv, 0, BlockSize / 8);
        }
        // Encrypt a string using TripleDES
        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using TripleDES tripleDES = TripleDES.Create();
            tripleDES.Key = key;
            tripleDES.IV = iv;
            tripleDES.Padding = PaddingMode.PKCS7; // JSON data 누락 방지를 위한 패딩 설정

            ICryptoTransform encryptor = tripleDES.CreateEncryptor(tripleDES.Key, tripleDES.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        // Decrypt a string using TripleDES
        public static string Decrypt(string cipherText, byte[] key, byte[] iv)
        {
            using TripleDES tripleDES = TripleDES.Create();
            tripleDES.Key = key;
            tripleDES.IV = iv;
            tripleDES.Padding = PaddingMode.PKCS7; // JSON data 누락 방지를 위한 패딩 설정

            ICryptoTransform decryptor = tripleDES.CreateDecryptor(tripleDES.Key, tripleDES.IV);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
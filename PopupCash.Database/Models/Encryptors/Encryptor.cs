using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PopupCash.Database.Models.Encryptors
{
    internal static class Encryptor
    {
        private static byte[] _saltKey = Encoding.UTF8.GetBytes("PopupCash-Enliple");
        internal static byte[] EncryptTextToBytes(string text)
        {
            try
            {
                // Create or open the specified file.
                using MemoryStream stream = new MemoryStream();
                // Create a new TripleDES object.
                using TripleDES tripleDes = TripleDES.Create();
                // Create a TripleDES encryptor from the key and IV
                using ICryptoTransform encryptor = tripleDes.CreateEncryptor(_saltKey, _saltKey);
                // Create a CryptoStream using the FileStream and encryptor
                using var cStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);

                // Convert the provided string to a byte array.
                byte[] toEncrypt = Encoding.UTF8.GetBytes(text);

                // Write the byte array to the crypto stream.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);

                return stream.ToArray();

            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                throw;
            }
        }

        internal static string DecryptTextFromBytes(byte[] chiperText)
        {
            try
            {
                // Open the specified file
                using (MemoryStream fStream = new MemoryStream(chiperText))
                // Create a new TripleDES object.
                using (TripleDES tripleDes = TripleDES.Create())
                // Create a TripleDES decryptor from the key and IV
                using (ICryptoTransform decryptor = tripleDes.CreateDecryptor(_saltKey, _saltKey))
                // Create a CryptoStream using the FileStream and decryptor
                using (var cStream = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                // Create a StreamReader to turn the bytes back into text
                using (StreamReader reader = new StreamReader(cStream, Encoding.UTF8))
                {
                    // Read back all of the text from the StreamReader, which receives
                    // the decrypted bytes from the CryptoStream, which receives the
                    // encrypted bytes from the FileStream.
                    return reader.ReadToEnd();
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                throw;
            }
        }
    }
}

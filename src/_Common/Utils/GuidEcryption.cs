using System.Security.Cryptography;
using System.Text;


namespace _Common.Utils;

public static class GuidEncryprion
{

    // AES encryption key (must be 16, 24, or 32 bytes)
    const string keystring = "MichalBolehradsky890123456789012";

    private static readonly byte[] key = Encoding.UTF8.GetBytes(keystring);

    // Scramble a GUID using AES encryption
    public static string ScrambleGuid(Guid guid)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV(); // Generate a random initialization vector

            // Create an encryptor to perform the stream transform
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write the GUID to the stream
                        swEncrypt.Write(guid.ToString());
                    }
                }
                // Concatenate the IV with the encrypted data
                byte[] encryptedData = msEncrypt.ToArray();
                byte[] combinedData = new byte[aesAlg.IV.Length + encryptedData.Length];
                Array.Copy(aesAlg.IV, 0, combinedData, 0, aesAlg.IV.Length);
                Array.Copy(encryptedData, 0, combinedData, aesAlg.IV.Length, encryptedData.Length);

                // Convert the encrypted data to a base64 string
                return Convert.ToBase64String(combinedData);
            }
        }
    }

    // Descramble a GUID using AES decryption
    public static Guid DescrambleGuid(string scrambledGuid)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;

            // Extract the IV from the encrypted data
            byte[] combinedData = Convert.FromBase64String(scrambledGuid);
            byte[] iv = new byte[aesAlg.BlockSize / 8];
            Array.Copy(combinedData, 0, iv, 0, iv.Length);

            // Create a decryptor to perform the stream transform
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

            // Create the streams used for decryption
            using (MemoryStream msDecrypt = new MemoryStream(combinedData, iv.Length, combinedData.Length - iv.Length))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and convert them back to a Guid
                        string decryptedGuid = srDecrypt.ReadToEnd();
                        return new Guid(decryptedGuid);
                    }
                }
            }
        }
    }
}


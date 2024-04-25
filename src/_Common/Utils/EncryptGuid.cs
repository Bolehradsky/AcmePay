using System.Security.Cryptography;
using System.Text;

namespace _Common.Utils;

public static class EncryptGuid
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 characters for AES128, 24 characters for AES192, 32 characters for AES256
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456"); // Initialization vector (IV) is important for security

    public static string Encrypt(Guid guid)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(guid.ToString());
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public static Guid Decrypt(string encryptedGuid)
    {

        encryptedGuid = encryptedGuid.Replace("%2B", "+");
        encryptedGuid = encryptedGuid.Replace("%2F", "/");



        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedGuid)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string decryptedGuid = srDecrypt.ReadToEnd();
                        return Guid.Parse(decryptedGuid);
                    }
                }
            }
        }
    }
}
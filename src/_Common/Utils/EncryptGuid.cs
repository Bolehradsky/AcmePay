using System.Security.Cryptography;
using System.Text;

namespace _Common.Utils;

public class EncryptGuid
    {

    private static EncryptGuid _encryptGuid;
    private static readonly object _locker = new Object();
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 characters for AES128, 24 characters for AES192, 32 characters for AES256
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456"); // Initialization vector (IV) is important for security
    public static EncryptGuid GetInstance()
        {
        if (_encryptGuid == null)
            {
            lock (_locker)
                {

                _encryptGuid = new EncryptGuid();
                }

            }
        return _encryptGuid;

        }


    public string Encrypt(Guid guid)
        {
        lock (_locker)
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
        }

    public Guid Decrypt(string encryptedGuid)
        {
        lock (_locker)
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
    }
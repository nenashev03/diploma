using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public class EncryptionManager
{
    private string _logFilePath = "encryption_log.txt";
    private RichTextBox _logTextBox;
    public enum EncryptionAlgorithm { AES, DES, XOR }
    public EncryptionManager(RichTextBox logTextBox = null)
    {
        _logTextBox = logTextBox;
    }
    public void Process(string path, string key, EncryptionAlgorithm algorithm, bool encrypt, bool overwrite)
    {
        try
        {
            if (File.Exists(path))
            {
                ProcessFile(path, key, algorithm, encrypt, overwrite);
            }
            else if (Directory.Exists(path))
            {
                foreach (var file in GetFilesFromFolder(path))
                {
                    ProcessFile(file, key, algorithm, encrypt, overwrite);
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Ошибка: {ex.Message}");
            throw;
        }
    }
    private byte[] ReadFile(string path)
    {
        try
        {
            Log($"Чтение файла: {path}");
            return File.ReadAllBytes(path);
        }
        catch (Exception ex)
        {
            Log($"Ошибка чтения: {ex.Message}");
            throw;
        }
    }

    private void WriteFile(string path, byte[] data)
    {
        try
        {
            File.WriteAllBytes(path, data);
            Log($"Файл записан: {path} ({data.Length} байт)");
        }
        catch (Exception ex)
        {
            Log($"Ошибка записи: {ex.Message}");
            throw;
        }
    }

    private List<string> GetFilesFromFolder(string folderPath)
    {
        return new List<string>(Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories));
    }
    private byte[] AESEncrypt(byte[] data, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = NormalizeKey(key, 32);
            aes.IV = new byte[16];

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
    private byte[] AESDecrypt(byte[] data, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = NormalizeKey(key, 32);
            aes.IV = new byte[16];

            using (var ms = new MemoryStream(data))
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (var output = new MemoryStream())
            {
                cs.CopyTo(output);
                return output.ToArray();
            }
        }
    }

    private byte[] DESEncrypt(byte[] data, string key)
    {
        using (DES des = DES.Create())
        {
            des.Key = NormalizeKey(key, 8);
            des.IV = new byte[8];

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
    private byte[] DESDecrypt(byte[] data, string key)
    {
        using (DES des = DES.Create())
        {
            des.Key = NormalizeKey(key, 8);
            des.IV = new byte[8];

            using (var ms = new MemoryStream(data))
            using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
            using (var output = new MemoryStream())
            {
                cs.CopyTo(output);
                return output.ToArray();
            }
        }
    }
    private byte[] XORCrypt(byte[] data, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
            result[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);

        return result;
    }
    private byte[] NormalizeKey(string key, int requiredLength)
    {
        return Encoding.UTF8.GetBytes(key.PadRight(requiredLength, '0')[..requiredLength]);
    }

    private void Log(string message)
    {
        string logEntry = $"[{DateTime.Now}] {message}";
        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        _logTextBox?.AppendText(logEntry + Environment.NewLine);
    }
    public void BindAlgorithms(ComboBox comboBox)
    {
        comboBox.DataSource = Enum.GetValues(typeof(EncryptionAlgorithm));
        comboBox.SelectedIndex = 0;
    }

    private void ProcessFile(string filePath, string key, EncryptionAlgorithm algorithm, bool encrypt, bool overwrite)
    {
        byte[] data = ReadFile(filePath);
        byte[] processedData;
        switch (algorithm)
        {
            case EncryptionAlgorithm.AES:
                processedData = encrypt ? AESEncrypt(data, key) : AESDecrypt(data, key);
                break;
            case EncryptionAlgorithm.DES:
                processedData = encrypt ? DESEncrypt(data, key) : DESDecrypt(data, key);
                break;
            case EncryptionAlgorithm.XOR:
                processedData = XORCrypt(data, key);
                break;
            default:
                throw new ArgumentException("Неизвестный алгоритм");
        }

        string outputPath = overwrite ? filePath : GetOutputPath(filePath, encrypt);
        WriteFile(outputPath, processedData);
    }

    private string GetOutputPath(string originalPath, bool encrypt)
    {
        string ext = encrypt ? ".enc" : ".dec";
        return Path.ChangeExtension(originalPath, ext);
    }
}
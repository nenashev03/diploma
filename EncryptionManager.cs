using diplom;
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
    private readonly Action<string> _logAction;

    public EncryptionManager(Action<string> logAction = null)
    {
        _logAction = logAction;
    }

    private void Log(string message)
    {
        string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";

        // В лог-файл
        try
        {
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            // В случае ошибки записи в файл — вывести в интерфейс
            _logAction?.Invoke($"Ошибка записи лога: {ex.Message}");
        }

        // В интерфейс
        _logAction?.Invoke(logMessage);
    }

    public string Process(string path, string key, EncryptionAlgorithm algorithm, bool encrypt, bool overwrite)
    {
        try
        {
            Log($"Начато {(encrypt ? "шифрование" : "дешифрование")} ({algorithm}) для: {path}");

            string outputPath = null;

            if (File.Exists(path))
            {
                outputPath = ProcessFile(path, key, algorithm, encrypt, overwrite);
            }
            else if (Directory.Exists(path))
            {
                Log($"Обработка папки: {path}");
                foreach (var file in GetFilesFromFolder(path))
                {
                    outputPath = ProcessFile(file, key, algorithm, encrypt, overwrite);
                }
                string currentDir = Directory.GetCurrentDirectory();
                Log($"Директория с резервными копиями: {currentDir}");
            }
            
            Log($"Операция завершена успешно!");
            return outputPath;
        }
        catch (Exception ex)
        {
            Log($"ОШИБКА: {ex.Message}");
            throw;
        }
    }
    internal string ProcessFile(string filePath, string key, EncryptionAlgorithm algorithm, bool encrypt, bool overwrite)
    {
        var backupManager = new BackupManager();
        string backupPath = backupManager.CreateBackup(filePath);
        Log($"Создана резервная копия файла: {backupPath}");

        byte[] data = ReadFile(filePath);
        byte[] processedData;
        byte[] iv = null;

        switch (algorithm)
        {
            case EncryptionAlgorithm.AES:
                using (Aes aes = Aes.Create())
                {
                    aes.Key = NormalizeKey(key, 32);
                    if (encrypt)
                    {
                        aes.GenerateIV();
                        iv = aes.IV;
                        using (var ms = new MemoryStream())
                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                            processedData = ms.ToArray();
                        }
                    }
                    else
                    {
                        aes.IV = new byte[16];
                        using (var ms = new MemoryStream(data))
                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (var output = new MemoryStream())
                        {
                            cs.CopyTo(output);
                            processedData = output.ToArray();
                        }
                    }
                }
                break;

            case EncryptionAlgorithm.DES:
                using (DES des = DES.Create())
                {
                    des.Key = NormalizeKey(key, 8);
                    if (encrypt)
                    {
                        des.GenerateIV();
                        iv = des.IV;
                        using (var ms = new MemoryStream())
                        using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                            processedData = ms.ToArray();
                        }
                    }
                    else
                    {
                        des.IV = new byte[8];
                        using (var ms = new MemoryStream(data))
                        using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                        using (var output = new MemoryStream())
                        {
                            cs.CopyTo(output);
                            processedData = output.ToArray();
                        }
                    }
                }
                break;

            case EncryptionAlgorithm.XOR:
                processedData = XORCrypt(data, key);
                iv = null; // IV не нужен для XOR
                break;

            default:
                throw new ArgumentException("Неизвестный алгоритм");
        }

        string outputPath = overwrite ? filePath : GetOutputPath(filePath, encrypt);
        WriteFile(outputPath, processedData);

        var dbService = new DatabaseService();
        int fileId = dbService.AddFile(filePath, outputPath, processedData.Length);
        string keyHash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(key)))
            .Replace("-", "").ToLower();
        dbService.AddEncryptionOperation(fileId, algorithm.ToString(), keyHash, iv, encrypt);

        return outputPath;
      
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
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Ключ не может быть пустым");

        // Хешируем ключ чтобы получить нужную длину
        using (var sha256 = SHA256.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = sha256.ComputeHash(keyBytes);
            Array.Resize(ref hash, requiredLength);
            return hash;
        }
    }

    public void BindAlgorithms(ComboBox comboBox)
    {
        comboBox.DataSource = Enum.GetValues(typeof(EncryptionAlgorithm));
        comboBox.SelectedIndex = 0;
    }

    

    private string GetOutputPath(string originalPath, bool encrypt)
    {
        string ext = encrypt ? ".enc" : ".dec";
        return Path.ChangeExtension(originalPath, ext);
    }

}
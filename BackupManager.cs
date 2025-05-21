using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    internal class BackupManager
    {
        private readonly string _backupDirectory;
        private readonly ILogger _logger;

        public BackupManager(string backupDirectory = "Backups", ILogger logger = null)
        {
            _backupDirectory = backupDirectory;
            _logger = logger;

            // Создаем директорию для бэкапов, если её нет
            Directory.CreateDirectory(_backupDirectory);
        }

        public string CreateBackup(string sourceFilePath)
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                    throw new FileNotFoundException("Файл для резервного копирования не найден", sourceFilePath);

                string originalDirectory = Path.GetDirectoryName(sourceFilePath); // Папка, где находится файл
                string backupDir = Path.Combine(originalDirectory, "Backups");

                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string backupFileName = GenerateBackupFileName(sourceFilePath);
                string backupPath = Path.Combine(backupDir, backupFileName);

                File.Copy(sourceFilePath, backupPath, overwrite: true);

                return backupPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании резервной копии: {ex.Message}", ex);
            }
        }

        

        private string GenerateBackupFileName(string sourceFilePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            string extension = Path.GetExtension(sourceFilePath);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return $"{fileName}_backup_{timestamp}{extension}";
        }

        public string GetFileHash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger?.LogError($"Файл для хеширования не найден: {filePath}");
                throw new FileNotFoundException("Файл не найден", filePath);
            }

            try
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Ошибка при вычислении хеша: {ex.Message}");
                throw;
            }
        }
        
        
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace diplom
{
    internal class FileService  
    {
        private readonly ILogger<FileService> _logger;
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public byte[] ReadFile(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка чтения файла {Path}", path);
                throw;
            }
        }

        public void WriteFile(string path, byte[] data)
        {
            try
            {
                File.WriteAllBytes(path, data);
                _logger.LogInformation("Файл {Path} успешно записан", path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка записи файла {Path}", path);
                throw;
            }
        }

        public List<string> GetFilesFromFolder(string folderPath, bool recursive = true)
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            try
            {
                return Directory.GetFiles(folderPath, "*.*", searchOption).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения файлов из {FolderPath}", folderPath);
                return new List<string>();
            }
        }
    }
}
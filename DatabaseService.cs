using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    internal class DatabaseService
    {
        private readonly string _connectionString = "Data Source=encryption_db.sqlite;";


        public DatabaseService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(@"
    CREATE TABLE IF NOT EXISTS Files (
        file_id INTEGER PRIMARY KEY AUTOINCREMENT,
        original_path TEXT NOT NULL,
        encrypted_path TEXT NOT NULL,
        file_size INTEGER NOT NULL
    );");

                connection.Execute(@"
    CREATE TABLE IF NOT EXISTS EncryptionOperations (
        operation_id INTEGER PRIMARY KEY AUTOINCREMENT,
        file_id INTEGER NOT NULL,
        algorithm TEXT NOT NULL,
        key_hash TEXT NOT NULL,
        iv BLOB,
        is_encrypted BOOLEAN NOT NULL,
        timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY(file_id) REFERENCES Files(file_id)
    );");
            }
        }

        // Добавление записи о файле
        public int AddFile(string originalPath, string encryptedPath, long fileSize)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Вставка данных
                connection.Execute(
                    @"INSERT INTO Files (original_path, encrypted_path, file_size) 
              VALUES (@originalPath, @encryptedPath, @fileSize);",
                    new { originalPath, encryptedPath, fileSize });

                // Получение ID последней вставки
                return connection.ExecuteScalar<int>("SELECT last_insert_rowid();");
            }
        }

        // Добавление записи об операции
        public void AddEncryptionOperation(int fileId, string algorithm, string keyHash, byte[] iv, bool isEncrypted)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(
                    @"INSERT INTO EncryptionOperations 
                  (file_id, algorithm, key_hash, iv, is_encrypted)
                  VALUES (@fileId, @algorithm, @keyHash, @iv, @isEncrypted)",
                    new { fileId, algorithm, keyHash, iv, isEncrypted });
            }
        }

        // Получение истории операций по файлу
        public IEnumerable<dynamic> GetFileHistory(string filePath)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Query(
                    @"SELECT f.original_path, f.encrypted_path, e.algorithm, e.is_encrypted, e.timestamp
                  FROM Files f
                  JOIN EncryptionOperations e ON f.file_id = e.file_id
                  WHERE f.original_path = @filePath OR f.encrypted_path = @filePath
                  ORDER BY e.timestamp DESC",
                    new { filePath });
            }
        }
    }
}

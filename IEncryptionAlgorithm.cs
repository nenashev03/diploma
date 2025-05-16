using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
       public interface IEncryptionAlgorithm
    {
        /// <summary> Шифрование данных </summary>
        /// <param name="data">Исходные данные</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns>Зашифрованные данные</returns>
        byte[] Encrypt(byte[] data, string key);
        
        /// <summary> Дешифрование данных </summary>
        /// <param name="data">Зашифрованные данные</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns>Расшифрованные данные</returns>
        byte[] Decrypt(byte[] data, string key);
        
        /// <summary> Поддерживаемые размеры ключей (в битах) </summary>
        int[] SupportedKeySizes { get; }
        
        /// <summary> Проверка валидности ключа </summary>
        bool IsKeyValid(string key);
    }
}

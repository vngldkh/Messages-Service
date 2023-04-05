using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Peergrade_07.Models;

namespace Peergrade_07
{
    /// <summary>
    /// Статический класс, позволяющий сериализовывать и десериализовывать объекты.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Обобщённый метод для сериализации списка обхектов.
        /// </summary>
        /// <param name="list"> Список объектов. </param>
        /// <param name="fileName"> Путь к файлу. </param>
        /// <typeparam name="T"> Тип элементов списка. </typeparam>
        /// <returns> True, если сериализация прошла успешно, false - в ином случае. </returns>
        public static bool Serialize<T>(List<T> list, string fileName)
        {
            var info = JsonSerializer.Serialize(list);
            try
            {
                File.WriteAllText(fileName, info);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Обобщённый метод для десериализации списка обхектов.
        /// </summary>
        /// <param name="fileName"> Путь к файлу, хранящему сериализованный объект. </param>
        /// <typeparam name="T"> Тип элементов списка. </typeparam>
        /// <returns> Список объектов заданного типа. Пустой, если возникло исключение. </returns>
        public static List<T> Deserialize<T>(string fileName)
        {
            List<T> list;
            try
            {
                var info = File.ReadAllText(fileName);
                list = (List<T>) JsonSerializer.Deserialize(info, typeof(List<T>));
            }
            catch
            {
                return new List<T>();
            }

            return list;
        }
    }
}

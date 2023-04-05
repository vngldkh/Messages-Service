using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Peergrade_07.Models
{
    /// <summary>
    /// Класс, описывающий пользователя.
    /// </summary>
    [DataContract]
    public class User : IComparable
    {
        /// <summary>
        /// Email пользователя (идентификатор).
        /// </summary>
        [DataMember, Required]
        public string Email { get; set; }
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [DataMember, Required]
        public string UserName { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public User() { }
        
        /// <summary>
        /// Конструктор пользователя.
        /// </summary>
        /// <param name="email"> Идентифиактор пользователя. </param>
        /// <param name="userName"> Имя пользователя. </param>
        public User(string email, string userName)
            => (Email, UserName) = (email, userName);

        /// <summary>
        /// Изменение реализации метода Equals для проверки двух пользователей на равенство.
        /// </summary>
        /// <param name="obj"> Объект, с которым происходит сравнение. </param>
        /// <returns> True, если объекты равны, false - в ином случае. </returns>
        public override bool Equals(object obj)
            => obj is User u && this.Email == u.Email;

        /// <summary>
        /// Изменение рализация метода получения хэш-кода (для корректного сравнения объектов).
        /// </summary>
        /// <returns> Длина идентификатора. </returns>
        public override int GetHashCode()
            => Email.Length;

        /// <summary>
        /// Метод позволяет случайным образом сгенерировать пользователя.
        /// </summary>
        /// <returns> Сгенерированный пользователь. </returns>
        public static User GenerateUser()
        {
            var rnd = new Random();

            // Generate email.
            var emailMainLength = rnd.Next(1, 10);
            var emailDomenLength = rnd.Next(1, 5);
            var email = new StringBuilder(emailMainLength + emailDomenLength + 1);
            for (var i = 0; i < emailMainLength; i++)
                email.Append((char)rnd.Next('A', 'z' + 1));
            email.Append('@');
            for (var i = 0; i < emailDomenLength; i++)
                email.Append((char)rnd.Next('a', 'z' + 1));
            email.Append(".com");

            // Generate user name.
            var nameLength = rnd.Next(1, 10);
            var name = new StringBuilder(nameLength);
            for (var i = 0; i < nameLength; i++)
                name.Append((char)rnd.Next('A', 'z' + 1));

            return new User(email.ToString(), name.ToString());
        }

        /// <summary>
        /// Реализация интерфейса IComparable (для корректной сортировки).
        /// </summary>
        /// <param name="obj"> Объект, с которым происходит сравнение. </param>
        /// <returns> Результат сравнения. </returns>
        public int CompareTo(object obj)
            => string.Compare(Email, ((User)obj).Email, StringComparison.Ordinal);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text;
using Peergrade_07.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Peergrade_07.Models
{
    /// <summary>
    /// Класс, описывающий сообщение.
    /// </summary>
    [DataContract]
    public class Letter
    {
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        [DataMember, Required]
        public string Subject { get; set; }
        
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        
        /// <summary>
        /// Идентификатор отправителя.
        /// </summary>
        [DataMember, Required]
        public string SenderId { get; set; }
        
        /// <summary>
        /// Идентификатор получателя.
        /// </summary>
        [DataMember, Required]
        public string ReceiverId { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Letter() { }
        
        /// <summary>
        /// Конструктор сообщения.
        /// </summary>
        /// <param name="sbj"> Тема. </param>
        /// <param name="message"> Текст. </param>
        /// <param name="senderId"> Отправитель. </param>
        /// <param name="receiverId"> Получатель. </param>
        public Letter(string sbj, string message, string senderId, string receiverId)
        {
            Subject = sbj;
            if (message is null)
                message = "";
            else 
                Message = message;
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        /// <summary>
        /// Метод, позволяющий случайным образом сгенерировать сообщение.
        /// </summary>
        /// <returns> Сгенерированное сообщение. </returns>
        public static Letter GenerateLetter()
        {
            var rnd = new Random();

            // Generate subject.
            var sbjLength = rnd.Next(1, 10);
            var subject = new StringBuilder(sbjLength);
            subject.Append((char)rnd.Next('A', 'Z' + 1));
            for (var i = 0; i < sbjLength - 1; i++)
                subject.Append((char)rnd.Next('a', 'z' + 1));

            // Generate message.
            var msgLength = rnd.Next(1, 15);
            var message = new StringBuilder(msgLength);
            for (var i = 0; i < msgLength; i++)
                message.Append((char)rnd.Next('!', '~' + 1));

            // Choose sender.
            var sender = UsersController.Users[rnd.Next(UsersController.Users.Count)].Email;

            // Choose receiver.
            var receiver = UsersController.Users[rnd.Next(UsersController.Users.Count)].Email;

            return new Letter(subject.ToString(), message.ToString(), sender, receiver);
        }
    }
}

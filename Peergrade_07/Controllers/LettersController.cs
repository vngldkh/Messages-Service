using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peergrade_07.Models;
using System.IO;

namespace Peergrade_07.Controllers
{
    /// <summary>
    /// Контроллер сообщений.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class LettersController : Controller
    {
        /// <summary>
        /// Список сообщений.
        /// </summary>
        public static List<Letter> Letters { get; } = Serializer.Deserialize<Letter>("Info/Letters.json");
        
        /// <summary>
        /// HttpGet метод для получения полного списка сообщений.
        /// </summary>
        /// <returns> Полный список сообщений. </returns>
        [HttpGet("request/all")]
        public IEnumerable<Letter> Get()
            => Letters;

        /// <summary>
        /// HttpGet метод для получения списка сообщений по отправителю и получателю.
        /// </summary>
        /// <param name="senderId"> Email отправителя. </param>
        /// <param name="receiverId"> Email получателя. </param>
        /// <returns> Список сообщений от заданного отправителя к заданному получателю. </returns>
        [HttpGet("request/by_sender&receiver:{senderId};{receiverId}")]
        public IEnumerable<Letter> Get(string senderId, string receiverId)
        {
            var letters = from x in Letters
                where x.SenderId == senderId && x.ReceiverId == receiverId
                select x;
            
            return letters;
        }
        
        /// <summary>
        /// HttpGet метод для получения списка сообщений по отправителю.
        /// </summary>
        /// <param name="senderId"> Email отправителя. </param>
        /// <returns> Список сообщений от заданного отправителя. </returns>
        [HttpGet("request/by_sender:{senderId}")]
        public IEnumerable<Letter> GetBySender(string senderId)
        {
            var letters = from x in Letters
                where  x.SenderId == senderId
                select x;
            
            return letters;
        }
        
        /// <summary>
        /// HttpGet метод для получения списка сообщений по получателю.
        /// </summary>
        /// <param name="receiverId"> Email получателя. </param>
        /// <returns> Список сообщений заданному получателю. </returns>
        [HttpGet("request/by_receiver:{receiverId}")]
        public IEnumerable<Letter> GetByReceiver(string receiverId)
        {
            var letters = from x in Letters
                where  x.ReceiverId == receiverId
                select x;
            
            return letters;
        }

        /// <summary>
        /// HttpPost метод для отправки нового сообщения.
        /// </summary>
        /// <param name="subject"> Тема сообщения. </param>
        /// <param name="senderId"> Email отправителя. </param>
        /// <param name="receiverId"> Email получателя. </param>
        /// <param name="message"> Текст сообщения. </param>
        /// <returns> Результат запроса. </returns>
        [HttpPost("add:{subject};{senderId};{receiverId}")]
        public IActionResult Post(string subject, string senderId, string receiverId, string message)
        {
            if (UsersController.Users.SingleOrDefault(u => u.Email == senderId) == null ||
                UsersController.Users.SingleOrDefault(u => u.Email == receiverId) == null)
                return NotFound();
            Letters.Add(new Letter(subject, message, senderId, receiverId));
            if (!Serializer.Serialize(Letters, "Info/Letters.json"))
                return new BadRequestResult();
            return new OkObjectResult(Letters);
        }

        /// <summary>
        /// HttpPost запрос для отправки нового случайно сгенерированного сообщения.
        /// </summary>
        /// <returns> Результат запроса. </returns>
        [HttpPost("add:random")]
        public IActionResult Post()
        {
            if (UsersController.Users.Count == 0)
                return new BadRequestResult();
            var letter = Letter.GenerateLetter();
            Letters.Add(letter);
            if (Serializer.Serialize(Letters, "Info/Letters.json")) 
                return new ObjectResult(letter);
            Letters.Remove(letter);
            return new BadRequestResult();
        }
        
        /// <summary>
        /// HttpDelete метод, который очищает список сообщений.
        /// </summary>
        /// <returns> Результат запроса. </returns>
        [HttpDelete("clear")]
        public IActionResult Clear()
        {
            if (!Serializer.Serialize(new List<Letter>(), "Info/Letters.json"))
                return new BadRequestResult();
            Letters.Clear();
            return new OkResult();
        }
    }
}

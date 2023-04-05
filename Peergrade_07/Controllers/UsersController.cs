using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peergrade_07.Models;

namespace Peergrade_07.Controllers
{
    /// <summary>
    /// Контроллер пользователей.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        /// <summary>
        /// Список пользователей.
        /// </summary>
        public static List<User> Users { get; } =  Serializer.Deserialize<User>("Info/Users.json");

        /// <summary>
        /// HttpGet метод для получения полного списка пользователей.
        /// </summary>
        /// <returns> Полный список пользователей. </returns>
        [HttpGet("request/all")]
        public IEnumerable<User> Get()
            => Users;
        
        /// <summary>
        /// HttpGet метод для получения среза списка пользователей.
        /// </summary>
        /// <param name="limit"> Максимольный размер среза. </param>
        /// <param name="offset"> Отступ от начала списка. </param>
        /// <returns> Срез списка пользователей. </returns>
        [HttpGet("request/segment:{limit:int};{offset:int}")]
        public IActionResult Get(int limit, int offset)
        {
            if (offset < 0 || limit <= 0)
                return new BadRequestResult();
            return new OkObjectResult(Users.ToArray()[offset..Math.Min(limit + offset, Users.Count)]);
        }

        /// <summary>
        /// HttpGet метод для получения информации о пользователе по идентификатору (email).
        /// </summary>
        /// <param name="email"> Идентификатор пользователя. </param>
        /// <returns> Информация о пользователе с данным идентификатором. </returns>
        [HttpGet("request/by_email:{email}")]
        public IActionResult Get(string email)
        {
            var user = Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound();

            return new OkObjectResult(user);
        }

        /// <summary>
        /// HttpPost метод для регистрации нового пользователя. 
        /// </summary>
        /// <param name="email"> Email пользователя (идентификатор). </param>
        /// <param name="userName"> Имя пользователя. </param>
        /// <returns> Результат запроса. </returns>
        [HttpPost("add:{email};{userName}")]
        public IActionResult Post(string email, string userName)
        {
            var user = new User(email, userName);
            if (Users.Contains(user))
                return new ConflictResult();
            Users.Add(user);
            Users.Sort();
            if (!Serializer.Serialize(Users, "Info/Users.json"))
                return new BadRequestResult();
            return new OkObjectResult(Users);
        }

        /// <summary>
        /// HttpPost метод для регистрации случайно сгенерированного пользователя. 
        /// </summary>
        /// <returns> Результат запроса. </returns>
        [HttpPost("add:random")]
        public IActionResult Post()
        {
            User user;
            do
            {
                user = Models.User.GenerateUser();
            } while (Users.Contains(user));
            Users.Add(user);
            Users.Sort();
            if (Serializer.Serialize(Users, "Info/Users.json"))
                return new OkObjectResult(user);
            Users.Remove(user);
            return new BadRequestResult();
        }

        /// <summary>
        /// HttpDelete метод, который очищает список пользователей.
        /// </summary>
        /// <returns> Результат запроса. </returns>
        [HttpDelete("clear")]
        public IActionResult Clear()
        {
            if (!Serializer.Serialize(new List<User>(), "Info/Users.json"))
                return new BadRequestResult();
            Users.Clear();
            return new OkResult();
        }
    }
}

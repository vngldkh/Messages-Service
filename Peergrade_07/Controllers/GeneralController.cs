using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peergrade_07.Models;

namespace Peergrade_07.Controllers
{
    /// <summary>
    /// Общий контроллер.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class GeneralController : Controller
    {
        /// <summary>
        /// HttpPost метод для инициализации списков.
        /// </summary>
        /// <returns> Результат запроса. </returns>
        [HttpPost("init")]
        public IActionResult Init()
        {
            var rnd = new Random();

            // Generate users.
            var usersCount = rnd.Next(2, 5);
            for (var i = 0; i < usersCount; i++)
            {
                var user = Models.User.GenerateUser();
                if (UsersController.Users.Contains(user))
                {
                    i--;
                    continue;
                }
                UsersController.Users.Add(user);
            }
            UsersController.Users.Sort();
            Serializer.Serialize(UsersController.Users, "Info/Users.json");

            // Generate letters.
            var lettersCount = rnd.Next(1, 10);
            for (var i = 0; i < lettersCount; i++)
                LettersController.Letters.Add(Letter.GenerateLetter());
            Serializer.Serialize(LettersController.Letters, "Info/Letters.json");

            return new OkResult();
        }
    }
}

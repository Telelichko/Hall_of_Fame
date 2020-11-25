using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers
{

    [ApiController]
    public class ErrorController : Controller
    {
        private static string _message;

        [Route("/error")]
        public static string HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    _message = $"{statusCode} - успешное выполнение запроса.";
                    break;
                case 400:
                    _message = $"{statusCode} - неверный запрос.";
                    break;
                case 404:
                    _message = $"{statusCode} - сущность не найдена в системе.";
                    break;
                case 500:
                    _message = $"{statusCode} - серверная ошибка";
                    break;
                default:
                    _message = $"{statusCode} - ошибка не детализирована.";
                    break;
            }

            return _message;
        }

    }
}

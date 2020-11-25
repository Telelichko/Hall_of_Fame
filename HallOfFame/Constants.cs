using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame
{
    public class Constants
    {
        public class Response
        {
            public const string Response200 = "200 – успешное выполнение запроса.";

            public const string Error400 = "400 – неверный запрос.";

            public const string Error404 = "404 – сущность не найдена в системе.";

            public const string Error500 = "500 – серверная ошибка.";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public class StaticDetails
    {
        public static string APIBaseUrl = "https://localhost:7116/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalparks";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trails";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/users";
    }
}

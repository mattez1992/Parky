using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class TrailRepo : GenericRepository<TrailWeb>, ITrailRepo
    {
        public TrailRepo(HttpClient client) : base(client)
        {
        }
    }
}

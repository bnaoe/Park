using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ParkWebApp.Models;
using ParkWebApp.Repository.IRepository;

namespace ParkWebApp.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) :base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}

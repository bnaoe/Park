using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkAPI.Data;
using ParkAPI.Models;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {

        private readonly ApplicationDBContext _db;

        public TrailRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(a => a.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public ICollection<Trail> geTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails.Include(a => a.NationalPark)
                .Where(b => b.NationalParkId == nationalParkId).ToList();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(a => a.NationalPark).FirstOrDefault(a => a.Id == trailId);
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a => a.Id == id);
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

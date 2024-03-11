using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fixit.Models;


namespace fixit.Data
{
 public interface IRepository<T>
    {
        Task<List<T>> GetData();
        Task<T> GetDataById(int id);

        Task<T> InsertData(T service);
        Task<T> UpdateData(T service);
        Task<bool> DeleteData(T service);

    }

    public class ServiceRepository: IRepository<Transakcija>
    {
        private readonly DataContext _context;
        public ServiceRepository(DataContext context)
        {
            _context = context;
        }
        // Delete Service objects
        public async Task<bool> DeleteData(Transakcija service)
        {
            Console.WriteLine("Delete method invoked");
            _context.Transakcija.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<Transakcija>> GetData()
        {
            //    Getting database data here
            var model = await _context.Transakcija.ToListAsync();
            return model;

        }
        public async Task<Transakcija> GetDataById(int id)
        {
            var model = await _context.Transakcija.FirstOrDefaultAsync(x => x.Transakcija_id == id);
            return model;
        }


        public async Task<Transakcija> InsertData(Transakcija transakcija)
        {

            Console.WriteLine("Insert transakcije");
            _context.Transakcija.Add(transakcija);

            await _context.SaveChangesAsync();
            return transakcija;
        }

        public async Task<Transakcija> UpdateData(Transakcija transakcija)
        {

            Console.WriteLine("Update transakcije");
            _context.Update(transakcija).Property(x => x.Transakcija_id).IsModified = false;
            _context.SaveChanges();

            return transakcija;
        }
    }
}
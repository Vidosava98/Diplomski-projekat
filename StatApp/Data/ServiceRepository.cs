using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fixit.Models;
using MySqlX.XDevAPI;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mysqlx.Crud;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using StatApp.Data;
using Confluent.Kafka;


namespace fixit.Data
{
 public interface IRepository<T>
    {
        Task<List<T>> GetData();
        Task<List<T>> GetCurrentData();
        Task<T> GetDataById(int id);
        Task<T> InsertData(T service);
        Task<T> UpdateData(T service);
        Task<bool> DeleteData(T service);
        void Refresh();
        //void kreirajKafkaPotrosaca();

    }

    public class ServiceRepository: IRepository<Transakcija>
    {
        private readonly DataContext _context;
        private readonly IHubContext<TransakcijaHub> _hubContext;
        public ServiceRepository(DataContext context, IHubContext<TransakcijaHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<bool> DeleteData(Transakcija service)
        {
            Console.WriteLine("Delete method invoked");
            _context.Transakcija.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transakcija>> GetCurrentData()
        {


            //SELECT t.Transakcija_id, t.Potrosnja, t.Ime, t.Prezime, t.Jmbg, t.Proizvod, t.Datum
            //FROM transakcija AS t
            // INNER JOIN(
            //   SELECT Jmbg, Proizvod,
            //   MAX(datum) AS MaxDatum
            //   FROM transakcija
            //   GROUP BY Jmbg, Proizvod
            //   ) AS g ON g.Jmbg = t.Jmbg AND g.Proizvod = t.Proizvod AND g.MaxDatum = t.Datum
            //   ORDER BY t.Datum;

            var model = await _context.Transakcija
                .Where(t => _context.Transakcija
                    .Where(inner => inner.Jmbg == t.Jmbg && inner.Proizvod == t.Proizvod)
                    .OrderByDescending(inner => inner.Datum)
                    .Select(inner => inner.Datum)
                    .FirstOrDefault() == t.Datum)
                .OrderBy(t => t.Datum)
                .ToListAsync();

            return model;

        }
        public async Task<List<Transakcija>> GetData()
        {        
            var model = await _context.Transakcija.ToListAsync();
            return model;

        }   
        public async void Refresh()
        {
            await _hubContext.Clients.All.SendAsync("OdradiReload", "Server");
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
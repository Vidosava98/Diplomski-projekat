using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fixit.Data;
using fixit.DTO;
using Microsoft.AspNetCore.Mvc;
using fixit.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StatApp.Data;
using MySqlX.XDevAPI;

namespace Controllers
{



    public class TransakcijaController : Controller
    {
        private readonly IRepository<Transakcija> _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<TransakcijaHub> _hubContext;
        public TransakcijaController(IRepository<Transakcija> repo, IMapper mapper, IHubContext<TransakcijaHub> hubContext)
        {
            _repo = repo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("Index")]
        public  async Task<IActionResult> Index()
        {
            var model =  await _repo.GetCurrentData();

            var modelDTO =  _mapper.Map<IEnumerable<TransakcijaDto>>(model);
            await _hubContext.Clients.All.SendAsync("PrimiPoruku", "Server", "Ova poruka je iz index kontrolera");
            return View(modelDTO);
        }
        //Treba da postoji Kafka subscriber koji ce da na odredjenom topiku ceka
        //poruku od ticdc-a i ukoliko je bude da onda pozove Refresh metodu koja ce da kaze svim clientima
        //da rade reload stranice, ne radim parcijalni reload tabele


        [HttpGet]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            await _hubContext.Clients.All.SendAsync("OdradiReload", "Server");         
            return Ok();
        }

        [HttpGet]
        [Route("getTransakcije")]
        public async Task<IActionResult> GetTransakcije()
        {
            Console.WriteLine("This is the get All service method");

            var model = await _repo.GetData();

            return Ok(_mapper.Map<IEnumerable<TransakcijaDto>>(model));

        }

        [HttpGet]
        [Route("getTransakcijaById")]
        public async Task<IActionResult> GetTransakcijaById(int id)
        {
            Console.WriteLine("This is the comming id ");
            Console.WriteLine(id);


            var model = await _repo.GetDataById(id);
            return Ok(_mapper.Map<TransakcijaDto>(model));
        }



        [HttpPost]
        [Route("updateTransakcija")]
        public async Task<IActionResult> UpdateTransakcija(Transakcija transakcija)
        {
            //var transakcijadto = _mapper.Map<TransakcijaDto>(transakcija);
            await _repo.UpdateData(transakcija);
            return Ok(transakcija);
        }

        [HttpDelete]
        [Route("deleteTransakcija")]
        public async Task<IActionResult> DeleteTransakcija(int id)
        {
            var transakcija = await _repo.GetDataById(id);
            // var service = _mapper.Map<Service>(serviceDto);
            await _repo.DeleteData(transakcija);
            return Ok(_mapper.Map<TransakcijaDto>(transakcija));


        }

    }
}
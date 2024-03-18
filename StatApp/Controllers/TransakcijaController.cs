using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fixit.Data;
using fixit.DTO;
using Microsoft.AspNetCore.Mvc;
using fixit.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StatApp.Data;
using MySqlX.XDevAPI;
using Amazon.Kinesis.Model;

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
            Task.Run(() => kreirajKafkaPotrosaca());
        } 
        public async Task kreirajKafkaPotrosaca()
        {
            IConsumer<Ignore, string> _consumer;
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "my-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe("topik");
            await Task.Run(() =>
            {
                while (true)
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(10));
                    if (consumeResult != null)
                    {
                        Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                        _hubContext.Clients.All.SendAsync("PrimiPoruku", "Server", $"Received message: {consumeResult.Message.Value}");
                    }
                }
            });
        }    
        [HttpGet]
        [Route("Index")]
        public  async Task<IActionResult> Index()
        {         
            var model = await _repo.GetCurrentData();
            var modelDTO =  _mapper.Map<IEnumerable<TransakcijaDto>>(model);         
            return View(modelDTO);
        
        }
        //Treba da postoji Kafka consumer  koji ce da na odredjenom topiku ceka
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
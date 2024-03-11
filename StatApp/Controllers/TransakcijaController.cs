using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fixit.Data;
using fixit.DTO;
using Microsoft.AspNetCore.Mvc;
using fixit.Models;

using Microsoft.AspNetCore.Authorization;

namespace Controllers
{



    public class TransakcijaController : Controller
    {
        private readonly IRepository<Transakcija> _repo;
        private readonly IMapper _mapper;
        public TransakcijaController(IRepository<Transakcija> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Index")]
        public  async Task<IActionResult> Index()
        {
            var model =  await _repo.GetData();

            var modelDTO =  _mapper.Map<IEnumerable<TransakcijaDto>>(model);
            return View(modelDTO);
        }

        [HttpGet]
        [Route("getTransakcije")]
        public async Task<IActionResult> GetTransakcije()
        {
            Console.WriteLine("This is the get All service method");

            var model = await _repo.GetData();

            return Ok(_mapper.Map<IEnumerable<TransakcijaDto>>(model));

        }

        [HttpGet("{id}")]
        [Route("getTransakcijaId")]
        public async Task<IActionResult> GetTransakcijaById(int id)
        {
            Console.WriteLine("This is the comming id ");
            Console.WriteLine(id);


            var model = await _repo.GetDataById(id);
            return Ok(_mapper.Map<TransakcijaDto>(model));
        }


        //  Service Post method

         [HttpPost]
        public async Task<IActionResult> CreateTransakcija(TransakcijaDto transakcijaDto)
        {
            var transakcija = _mapper.Map<Transakcija>(transakcijaDto);
            await _repo.UpdateData(transakcija);
            return Ok(transakcijaDto);
        }

        // Service Delete method
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransakcija(int id)
        {
            var transakcija = await _repo.GetDataById(id);
            // var service = _mapper.Map<Service>(serviceDto);
            await _repo.DeleteData(transakcija);
            return Ok(_mapper.Map<TransakcijaDto>(transakcija));


        }
    }
}
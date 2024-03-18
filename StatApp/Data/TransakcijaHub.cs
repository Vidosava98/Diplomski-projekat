using Microsoft.AspNetCore.SignalR;
namespace StatApp.Data
{
    public class TransakcijaHub: Hub
    {
        public async Task PosaljiPoruku(string korisnik, string poruka)
        {
            // Ova metoda može biti pozvana sa klijentske strane
            // i poslati poruku svim povezanim klijentima
          //await Clients.All.SendAsync("PrimiPoruku", korisnik, poruka);
        }
    }
}

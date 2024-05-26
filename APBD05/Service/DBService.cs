using APBD05.Context;
using APBD05.DTOs;
using APBD05.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD05.Service
{
    public class DBService : IDBService
    {
        private readonly MasterContext _context;

        public DBService(MasterContext context)
        {
            _context = context;
        }

        public async Task AddClientToTripAsync(int idTrip, ClientDTO clientDTO)
        {
            var trip  = await _context.Trips.FindAsync(idTrip);
            if (trip == null) throw new Exception("Trip not found");

            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Pesel == clientDTO.Pesel);
            if ( client==null){
                client = new Client
                {
                    IdClient = clientDTO.IdClient,
                    FirstName = $"{clientDTO.FirstName}",
                    LastName = $"{clientDTO.LastName}",
                    Email = $"{clientDTO.Email}",
                    Telephone = $"{clientDTO.Telephone}",
                    Pesel = $"{clientDTO.Pesel}"
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var existingClientTrip = await _context.ClientTrips.SingleOrDefaultAsync(ct => ct.IdTrip == idTrip && ct.IdClient == client.IdClient);

            if (existingClientTrip != null) throw new Exception("Client is already assigned to this trip");

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDTO.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteClientAsync(int idClient)
        {
            if (_context.ClientTrips.AnyAsync(c => c.IdClient == idClient).Result) return false;

            if (!_context.Clients.AnyAsync(c => c.IdClient == idClient).Result) return false;

            _context.Clients.Remove(_context.Clients.Where(c => c.IdClient == idClient).First());
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<IEnumerable<TripDTO>> GetTripsAsync()
        {
            return await _context.Trips
                .OrderByDescending(c => c.DateFrom)
                .Select(d => new TripDTO
                {
                    Name = d.Name,
                    Description = d.Description,
                    DateFrom = d.DateFrom,
                    DateTo = d.DateTo,
                    MaxPeople = d.MaxPeople,
                    Countries = d.CountryTrips.Select(c => new CountryDTO
                    {
                        Name = c.IdTripNavigation.Name
                    }).ToList(),
                    Clients = d.ClientTrips.Select( c => new ClientDTO
                    {
                        FirstName = c.IdClientNavigation.FirstName,
                        LastName = c.IdClientNavigation.LastName
                    }).ToList()
                }).ToListAsync();
        }
    }
}

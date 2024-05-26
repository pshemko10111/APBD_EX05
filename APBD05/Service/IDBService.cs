using APBD05.DTOs;

namespace APBD05.Service
{
    public interface IDBService
    {
        public Task<IEnumerable<TripDTO>> GetTripsAsync();
        public Task<bool> DeleteClientAsync(int idClient);
        public Task AddClientToTripAsync(int idTrip, ClientDTO clientDTO);
    }
}

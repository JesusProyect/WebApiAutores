namespace API.Services.Interfaces
{
    public interface IAutorLibroService
    {
        public Task<bool> CheckAutorExist(int autorId);
    }
}

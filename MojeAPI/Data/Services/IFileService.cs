namespace MojeAPI.Data.Services
{
    public interface IFileService
    {
        public Task<bool> ReadFile(IFormFile filePath);
        public Task<byte[]> WriteFile(int numberOfRecordsToTake, int skip, string filteredBooks);
    }
}

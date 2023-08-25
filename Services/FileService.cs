namespace CompanyNewsAPI.Services
{
    public class FileService
    {
        private static Semaphore _fileSemaphore = new Semaphore(1, 1);

        public async static Task<string[]> ReadAllLinesAsync(string path)
        {
            try
            {
                _fileSemaphore.WaitOne();
                return await File.ReadAllLinesAsync(path);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
        public static string[] ReadAllLines(string path)
        {
            try
            {
                _fileSemaphore.WaitOne();
                return  File.ReadAllLines(path);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
        public async static Task<string> ReadFileAsync(string path)
        {
            try
            {
                _fileSemaphore.WaitOne();
                return await File.ReadAllTextAsync(path);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
        public async static Task AppendAllTextAsync(string path, string data)
        {
            try
            {
                _fileSemaphore.WaitOne();
                File.AppendAllTextAsync(path, data);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
        public async static Task WriteAllLinesAsync(string path, List<string> data)
        {
            try
            {
                _fileSemaphore.WaitOne();
                File.WriteAllLinesAsync(path, data);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
        public static void WriteAllLines(string path, List<string> data)
        {
            try
            {
                _fileSemaphore.WaitOne();
                File.WriteAllLines(path, data);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
    }
}

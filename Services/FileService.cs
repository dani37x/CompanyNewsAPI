namespace CompanyNewsAPI.Services
{
    public class FileService
    {
        private static Semaphore _fileSemaphore = new Semaphore(1, 1);

        public static string[] ReadAllLines(string path)
        {
            try
            {
                _fileSemaphore.WaitOne();
                return File.ReadAllLines(path);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        public static void AppendAllText(string path, string data)
        {
            try
            {
                _fileSemaphore.WaitOne();
                File.AppendAllText(path, data);
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

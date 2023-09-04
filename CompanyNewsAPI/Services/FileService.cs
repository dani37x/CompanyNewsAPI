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
            catch (Exception)
            {
                throw;
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
                return File.ReadAllLines(path);
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
                await File.AppendAllTextAsync(path, data);
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
                await File.WriteAllLinesAsync(path, data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
    }
}

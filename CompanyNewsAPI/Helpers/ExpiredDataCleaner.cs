using CompanyNewsAPI.Services;
using Hangfire;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompanyNewsAPI.Helpers
{
    [AutomaticRetry(Attempts = 3)]
    public class ExpiredDataCleaner
    {
        public void CleanExpiredData(string path)
        {
            List<string> availableKeys = new List<string>();
            var lines = FileService.ReadAllLines(path);
                
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        var data = line.Substring(0, line.Length - 1);
                        var json = (JObject)JsonConvert.DeserializeObject(data);

                        if (DateTime.Now < Convert.ToDateTime(json["Date"]))
                        {
                            availableKeys.Add(line);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            FileService.WriteAllLines(path, availableKeys);
        }
    }
}


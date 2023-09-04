using CompanyNewsAPI.Services;
using System.Text;

namespace CompanyNewsAPI.Generators
{
    public class KeysGenerator
    {
        private static readonly string _smallChars = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _bigChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string _numbers = "0123456789";
        private static readonly string _specialChars = "!@#$%^&*()";

        public async static Task<string> RandomStr(string path, int length)
        {
            var everyChar = _smallChars + _bigChars + _numbers + _specialChars;
            List<char> chars = new List<char>(everyChar);
            var random = new Random();
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Count - 1)]);
            }

            var existingKeys = await FileService.ReadFileAsync(path);
            if (existingKeys.Contains(sb.ToString()))
            {
                return await RandomStr(path, length);
            }

            return sb.ToString();
        }
    }
}

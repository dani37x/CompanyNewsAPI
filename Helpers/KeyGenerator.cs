﻿namespace CompanyNewsAPI.Generators
{
    public class KeyGenerator
    {
        private static readonly string _smallChars = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _bigChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string _numbers = "0123456789";
        private static readonly string _specialChars = "!@#$%^&*()";

        public static string RandomStr(int length)
        {
            var everyChar = _smallChars + _bigChars + _numbers + _specialChars;
            List<char> chars = new List<char>(everyChar);
            var random = new Random();
            var randomString = "";

            for (int i = 0; i < length; i++)
            {
                randomString += chars[random.Next(chars.Count - 1)];
            }

            return randomString;
        }
    }
}
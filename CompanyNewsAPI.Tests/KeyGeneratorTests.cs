using CompanyNewsAPI.Generators;
using CompanyNewsAPI.Services;

namespace CompanyNewsAPI.Tests

{
    [TestClass]
    public class KeyGeneratorTests
    {
        private string _keysGeneratorTests = @"keysGeneratorTests.txt";

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(10)]
        public async Task KeysGenerator_ExpectedLength_ReturnsTrue(int keyLength)
        {
            var generatedKey = await KeysGenerator.RandomStr(_keysGeneratorTests, keyLength);

            var generatedKeyLength = generatedKey.Length;

            Assert.AreEqual(generatedKeyLength, keyLength);

        }
        [TestMethod]
        [DataRow(1, 3)]
        [DataRow(3, 5)]
        [DataRow(10, 9)]
        public async Task KeysGenerator_DifferentLengthTwoKeys_ReturnsFalse(int keyOneLength, int keyTwoLength)
        {
            var firstKey = await KeysGenerator.RandomStr(_keysGeneratorTests, keyOneLength);
            var secondKey = await KeysGenerator.RandomStr(_keysGeneratorTests, keyTwoLength);

            var equal = firstKey.Length == secondKey.Length;

            Assert.IsFalse(equal);
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(10)]
        [DataRow(13)]
        [DataRow(24)]
        public async Task KeysGenerator_UniqueKeys_ReturnsTrue(int keyLenght)
        {
            string exampleGeneratedKeyToSave = await KeysGenerator.RandomStr(_keysGeneratorTests, keyLenght);
            await File.WriteAllTextAsync(_keysGeneratorTests, exampleGeneratedKeyToSave);

            var generatedKey = await KeysGenerator.RandomStr(_keysGeneratorTests, keyLenght);

            Assert.AreNotEqual(exampleGeneratedKeyToSave, generatedKey);

        }
    }
}
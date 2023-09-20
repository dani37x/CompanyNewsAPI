using CompanyNewsAPI.Helpers;
using CompanyNewsAPI.Services;
using CompanyNewsAPI.Tests.Models;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace CompanyNewsAPI.Tests
{
    [TestClass]
    public class ExpiredDataCleanerTests
    {
        private string _expiredDataCleanerTestsFile = @"expiredDataCleanerTests.json";

        [TestMethod]
        public void ExpiredDataCleaner_ExpiredData_ReturnsTrue() 
        {
            var cleaner = new ExpiredDataCleaner();
            ExpiredDataCleanerModel data = new ExpiredDataCleanerModel {Id = 1,  Date = DateTime.Now };
            var modelData = System.Text.Json.JsonSerializer.Serialize(data) + ",";
            FileService.WriteAllText(_expiredDataCleanerTestsFile, modelData);

            cleaner.CleanExpiredData(_expiredDataCleanerTestsFile);
            var existingData = FileService.ReadAllLines (_expiredDataCleanerTestsFile);

            Assert.AreNotEqual(1, existingData.Length);
        }      
        [TestMethod]
        public void ExpiredDataCleaner_NotExpiredData_ReturnsTrue() 
        {
            var cleaner = new ExpiredDataCleaner();
            var date = DateTime.Now.AddMinutes(15);
            ExpiredDataCleanerModel data = new ExpiredDataCleanerModel {Id = 1,  Date = date };
            var modelData = System.Text.Json.JsonSerializer.Serialize(data) + ",";
            FileService.WriteAllText(_expiredDataCleanerTestsFile, modelData);

            cleaner.CleanExpiredData(_expiredDataCleanerTestsFile);
            var existingData = FileService.ReadAllLines (_expiredDataCleanerTestsFile);

            Assert.AreEqual(1, existingData.Length);
        }
        [TestMethod]
        public void ExpiredDataCleaner_DataWithoutDate_ReturnsJsonSerializationException()
        {
            var cleaner = new ExpiredDataCleaner();
            var data = new ExpiredDataCleanerModel();
            data.Id = 1;
            var modelData = JsonConvert.SerializeObject(data);
            FileService.WriteAllText(_expiredDataCleanerTestsFile, modelData);

            Assert.ThrowsException<JsonSerializationException>(() =>
            {
                cleaner.CleanExpiredData(_expiredDataCleanerTestsFile);
            });
        }

    }
}

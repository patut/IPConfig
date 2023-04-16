using System;
using System.Diagnostics;
using IPLocatorAPI.Services;

namespace IPLocatorAPI.Tests.Services
{
	public class DatabaseServiceTests
	{
		private readonly DatabaseService _service;
		private string _databaseAddress = "geobase.dat";

		public DatabaseServiceTests()
		{
			_service = new DatabaseService(_databaseAddress, 96, 12, 4);

        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void LoadData_LoadingTime_IsReadingTheFilePlus5Milliseconds(int numberOfLoops)
		{
            var timer = new Stopwatch();
            
            timer.Start();

            var results = new double[numberOfLoops];

            for (var i = 0; i < numberOfLoops; i++)
            {
                var startTime = timer.Elapsed;
                File.ReadAllBytes(_databaseAddress);
                var elapsed = timer.Elapsed;

                _service.LoadData();
                var loadElapsed = timer.Elapsed;

                var firstTime = elapsed.TotalMilliseconds - startTime.TotalMilliseconds;
                var secondTime = loadElapsed.TotalMilliseconds - elapsed.TotalMilliseconds;

                var diff = secondTime - firstTime;

                results[i] = diff;
            }

            var avg = results.Average();
            Assert.True(avg <= 5);

        }

    }
}


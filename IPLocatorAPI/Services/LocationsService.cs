using System;
using System.Net;
using System.Reflection;
using IPLocatorAPI.Model;

namespace IPLocatorAPI.Services
{
	public class LocationsService
	{
		private IDatabaseService _databaseService;

        public LocationsService(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
        }

        /// <summary>
        /// Filters locations by a city string
        /// </summary>
        /// <param name="targetCity">City name used for filtering</param>
        /// <returns>List of locations</returns>
		public List<Location> FilterByCity(string targetCity)
		{
            (int from, int to) = SearchCity(targetCity);

            return FindLocationsByCityIndex(from, to);

        }

        /// <summary>
        /// Loads locations in a result array using starting and ending positions in index
        /// </summary>
        /// <param name="from">starting position in index</param>
        /// <param name="to">to: ending position in index</param>
        /// <returns>List of locations</returns>
        private List<Location> FindLocationsByCityIndex(int from, int to)
        {
            if (from == -1) return new List<Location>();

            var ans = new List<Location>(to - from + 1);
            for (var i = from; i <= to; i++)
            {
                var index = _databaseService.GetCityIndexAtIndex(i);
                ans.Add(_databaseService.GetLocationAtIndex((int)index));
            }

            return ans;
        }

        /// <summary>
        /// Search the boundaries in a city index that match the target city
        /// </summary>
        /// <param name="targetCity">City name used for filtering</param>
        /// <returns>from: starting position in index, to: ending position in index</returns>
        private (int from, int to) SearchCity(string targetCity)
		{
            var left = 0;
            var right = _databaseService.RecordsCount - 1;
            var from = -1;
            var to = -1;

            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var city = GetCityByCityIndex(middle);

                var compare = string.Compare(targetCity, city);
                if (compare == 0)
                {
                    from = FindLowestBoundary(left, middle, targetCity);
                    to = FindHighestBoundary(middle, right, targetCity);
                    break;
                }

                if (compare == -1)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return new(from, to);
        }

        /// <summary>
        /// Gets city name by index position
        /// </summary>
        /// <param name="index">Index position in Cities index</param>
        /// <returns>City name</returns>
        private string GetCityByCityIndex(int index)
        {
            var locationOffset = _databaseService.GetCityIndexAtIndex(index);
            return _databaseService.GetLocationAtIndex(locationOffset).City;
        }

        /// <summary>
        /// Filters Locations by an IP range
        /// </summary>
        /// <param name="ip">Searched ip address</param>
        /// <returns>Found or not found location</returns>
        public Location? FilterByIp(string ip)
        {
			var intIp = ConvertIpToInteger(ip);
            var result = BinarySearchIpAddress(intIp);
            return result == null ? null : _databaseService.GetLocationAtIndex((int)result);
        }

        /// <summary>
        /// Using binary search algorithm find an address of location in Locations table
        /// </summary>
        /// <param name="intIp">Target ip address</param>
        /// <returns>Found or not found address of location in Locations table</returns>
        private uint? BinarySearchIpAddress(uint intIp)
        {
            var left = 0;
            var right = _databaseService.RecordsCount - 1;

            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var ipAddress = _databaseService.GetIpAtIndex(middle);
                if (intIp >= ipAddress.IpFrom && intIp <= ipAddress.IpTo)
                {
                    return ipAddress.LocationIndex;
                }

                if (intIp < ipAddress.IpFrom)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }
            return null;
        }

        /// <summary>
        /// Helper method that converts ip address from string to uint
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <returns>Uint representation of IP address</returns>
        /// <exception cref="InvalidDataException">IP address validation exeption</exception>
		public uint ConvertIpToInteger(string ip)
		{
            var parsed = IPAddress.TryParse(ip, out var address);
            if (!parsed) throw new InvalidDataException($"Неверный IP адрес {ip}. Пожалуйста, введите корректный адрес.");

            var bytes = address.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
		}

        /// <summary>
        /// Finds lowest boundary index that matches target city in locations list
        /// </summary>
        /// <param name="left">Starting index</param>
        /// <param name="right">Ending index</param>
        /// <param name="targetCity">Target city</param>
        /// <returns>Lowest boundary index</returns>
		private int FindLowestBoundary(int left, int right, string targetCity)
		{
			var result = right;
            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var city = GetCityByCityIndex(middle);

                var compare = string.Compare(targetCity, city);
                if (compare == 0)
                {
					result = middle;
					right = middle - 1;
                }
				else
				{
					left = middle + 1;
				}
            }

			return result;
        }

        /// <summary>
        /// Finds highest boundary index that matches target city in locations list
        /// </summary>
        /// <param name="left">Starting index</param>
        /// <param name="right">Ending index</param>
        /// <param name="targetCity">Target city</param>
        /// <returns>Highest boundary index</returns>
        private int FindHighestBoundary(int left, int right, string targetCity)
        {
            var result = right;
            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var city = GetCityByCityIndex(middle);

                var compare = string.Compare(targetCity, city);
                if (compare == 0)
                {
                    result = middle;
                    left = middle + 1;
                }
                else
                {
                    right = middle - 1;
                }
            }

            return result;
        }
    }
}


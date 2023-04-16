using System;
using System.Reflection;
using System.Text;
using IPLocatorAPI.Model;

namespace IPLocatorAPI.Services
{
	public class DatabaseService : IDatabaseService
	{
		public int RecordsCount { get; private set; }


		private readonly uint LOCATION_RECORD_WEIGHT;
		private readonly uint IP_RANGE_RECORD_WEIGHT = 12;
		private readonly uint CITY_INDEX_RECORD_WEIGHT = 4;

		// Address of the database file
		private string _databaseAddress;

		// DataBase Metadata
        private int _dbVersion;
        private string _dbName;
        private ulong _dbTimeCreation;

		// смещение относительно начала файла до начала списка записей с геоинформацией
		private uint _offset_ranges;

		// смещение относительно начала файла до начала индекса с сортировкой по названию городов
		private uint _offset_cities;

		// смещение относительно начала файла до начала списка записей о местоположении
		private uint _offset_locations;

		// Read database in bytes
		private byte[] _bytes;

		// Array of Locations
		private Location[] _locations;

        // Array of IP ranges
        private IPRange[] _ipRanges;

        // Array of City Indecies
        private int[] _cityIndecies;

		public IPRange GetIpAtIndex(int index)
		{
			if (_ipRanges[index] != null) return _ipRanges[index];

			_ipRanges[index] = ReadIpRange(index);
			return _ipRanges[index];
		}

		public Location GetLocationAtIndex(int index)
		{
			if (_locations[index] != null) return _locations[index];

			
			_locations[index] = ReadLocation(index);
			return _locations[index];
		}

		public int GetCityIndexAtIndex(int index)
		{
			if (_cityIndecies[index] != -1) return _cityIndecies[index];

			var pointer = (int)(_offset_cities + CITY_INDEX_RECORD_WEIGHT * index);
			
			var res = ReadUInt(ref pointer);
			_cityIndecies[index] = (int)(res / LOCATION_RECORD_WEIGHT);
			return _cityIndecies[index];
		}

		public DatabaseService(string databaseAddress, uint locationRecordWeight, uint ipRangeWeight, uint cityRecordWeight)
		{
			_databaseAddress = databaseAddress;
			LOCATION_RECORD_WEIGHT = locationRecordWeight;
			IP_RANGE_RECORD_WEIGHT = ipRangeWeight;
			CITY_INDEX_RECORD_WEIGHT = cityRecordWeight;
    }

		/// <summary>
		/// Load and parse the databse to our RAM
		/// </summary>
        public void LoadData()
		{
			_bytes = File.ReadAllBytes(_databaseAddress);
			
			ReadMetaData();

			_locations = new Location[RecordsCount];
			_ipRanges = new IPRange[RecordsCount];
			_cityIndecies = new int[RecordsCount];
			
			Array.Fill(_cityIndecies, -1);
		}
		
		private void ReadMetaData()
		{
			var pointer = 0;
			_dbVersion = ReadInt(ref pointer);
			_dbName = ReadString(32, ref pointer);
			_dbTimeCreation = ReadULong(ref pointer);

			RecordsCount = ReadInt(ref pointer);
			_offset_ranges = ReadUInt(ref pointer);
			_offset_cities = ReadUInt(ref pointer);
			_offset_locations = ReadUInt(ref pointer);
		}

		/// <summary>
		/// Reads IP range record starting at calculated pointer
		/// </summary>
		/// <param name="index">Index of the object among all Records</param>
		/// <returns>IP Range</returns>
        private IPRange ReadIpRange(int index)
        {
            var pointer = (int)(_offset_ranges + IP_RANGE_RECORD_WEIGHT * index);
            return new IPRange(ReadUInt(ref pointer), ReadUInt(ref pointer), ReadUInt(ref pointer));
        }

        /// <summary>
        /// Reads location record starting at calculated pointer
        /// </summary>
        /// <param name="index">Index of the object among all Records</param>
        /// <returns>Location record</returns>
        private Location ReadLocation(int index)
        {
            var pointer = (int)(_offset_locations + LOCATION_RECORD_WEIGHT * index);
            return new Location()
	        {
		        Country = ReadString(8, ref pointer),
		        Region = ReadString(12, ref pointer),
		        Postal = ReadString(12, ref pointer),
		        City = ReadString(24, ref pointer),
		        Organization = ReadString(32, ref pointer),
		        Latitude = ReadFloat(ref pointer),
		        Longitude = ReadFloat(ref pointer),
	        };
        }

		/// <summary>
		/// Reads string for given byte range
		/// </summary>
		/// <param name="max">byte range allocated by the string</param>
		/// <param name="pointer">pointer in the byte array</param>
		/// <returns>Read string</returns>
        private string ReadString(int max, ref int pointer)
        {
	        var sb = new StringBuilder();
	        var chars = Encoding.UTF8.GetChars(_bytes, pointer, max);
	        for (var i = 0; i < chars.Length && chars[i] != '\0'; i++)
	        {
		        sb.Append(chars[i]);
	        }
            pointer += max;
	        return sb.ToString();
        }

        /// <summary>
        /// Reads U64 number from a given pointer
        /// </summary>
        /// <param name="pointer">pointer in the byte array</param>
        /// <returns>Read integer</returns>
        private ulong ReadULong(ref int pointer)
        {
	        var result = BitConverter.ToUInt64(_bytes, pointer);
            pointer += 8;
	        return result;
        }

        /// <summary>
        /// Reads integer starting from specific position in the bytes array
        /// </summary>
        /// <param name="pointer">pointer in the byte array</param>
        /// <returns></returns>
        private int ReadInt(ref int pointer)
        {
	        var result = BitConverter.ToInt32(_bytes, pointer);
            pointer += 4;
	        return result;;
        }

        /// <summary>
        /// Reads integer u32 starting from specific position in the bytes array
        /// </summary>
        /// <param name="pointer">pointer in the byte array</param>
        /// <returns></returns>
        private uint ReadUInt(ref int pointer)
        {
	        var result = BitConverter.ToUInt32(_bytes, pointer);
            pointer += 4;
	        return result;
        }

        /// <summary>
        /// Reads float starting from specific position in the bytes array
        /// </summary>
        /// <param name="pointer">pointer in the byte array</param>
        /// <returns></returns>
        private float ReadFloat(ref int pointer)
        {
	        var result = BitConverter.ToSingle(_bytes, pointer);
            pointer += 4;
	        return result;
        }
	}
}


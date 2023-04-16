using System;
using System.Net;

namespace IPLocatorAPI.Model
{
	public class IPRange
    {
        /// <summary>
        /// Начало диапазона IP адресов
        /// </summary>
		public uint IpFrom { get; private set; }

        /// <summary>
        /// Конец диапазона IP адресов
        /// </summary>
		public uint IpTo { get; private set; }

        /// <summary>
        /// Индекс адреса в таблице локаций
        /// </summary>
		public uint LocationIndex { get; private set; }

		public IPRange(uint ipForm, uint ipTo, uint locationIndex)
        {
			IpFrom = ipForm;
			IpTo = ipTo;
			LocationIndex = locationIndex;
		}

		public override string ToString()
        {
			return $"From {ToAddr(IpFrom)} to {ToAddr(IpTo)} at index {LocationIndex}";
        }

        /// <summary>
        /// Сконвертировать IP адрес из цифрового формата в строковый
        /// </summary>
        /// <param name="ipAddress">Числовой формат адреса</param>
        /// <returns>Строковый формат адреса</returns>
        private string ToAddr(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return new IPAddress(bytes).ToString();
        }
    }
}


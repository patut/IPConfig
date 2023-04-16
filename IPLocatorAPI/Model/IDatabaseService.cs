using System;
namespace IPLocatorAPI.Model
{
    /// <summary>
    /// Интерфейс базы данных
    /// </summary>
	public interface IDatabaseService
    {
        /// <summary>
        /// Gets IP range object by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>IP Range Object</returns>
        IPRange GetIpAtIndex(int index);

        /// <summary>
        /// Gets Location object by Index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Location GetLocationAtIndex(int index);

        /// <summary>
        /// Gets City Index by index in Locations array
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetCityIndexAtIndex(int index);

        /// <summary>
        /// Общее количество записей в таблице Локаций
        /// </summary>
        int RecordsCount { get; }
    }
}


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// A serializable record for the coins picked on a track.
    /// </summary>
    [Serializable]
    public class CoinRecord
    {
        /// <summary>
        /// The name of the track this record belongs to.
        /// </summary>
        public string trackName;
        /// <summary>
        /// The number of laps this record is for.
        /// </summary>
        public int laps;
        /// <summary>
        /// The coins of this record.
        /// </summary>
        public int coins;
        /// <summary>
        /// The name of the racer who recorded this time.
        /// </summary>
        public string name;

        const int k_DefaultCoin = 0;
        const string k_FolderName = "BinaryTrackRecordData";
        const string k_FileExtension = ".dat";

        /// <summary>
        /// Set all the information in a record.
        /// </summary>
        /// <param name="track">The new name of the track.</param>
        /// <param name="lapCount">The new lap count.</param>
        /// <param name="racer">The new racer whose name will be recorded.</param>
        /// <param name="newCoins">The new coin amount for the record.</param>
        public void SetRecord(string track, int lapCount, IRacer racer, int newCoins)
        {
            trackName = track;
            laps = lapCount;
            name = racer.GetName();
            coins = newCoins;
        }

        /// <summary>
        /// Creates a CoinRecord with default values.
        /// </summary>
        public static CoinRecord CreateDefault()
        {
            CoinRecord defaultCoin = new CoinRecord();
            defaultCoin.coins = k_DefaultCoin;
            return defaultCoin;
        }

        /// <summary>
        /// Saves a record using a file name based on the track name and number of laps.
        /// </summary>
        public static void Save(CoinRecord record)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, k_FolderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string dataPath = Path.Combine(folderPath, record.trackName + "Coins" + record.laps + k_FileExtension);

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, record);
            }
        }

        /// <summary>
        /// Finds and loads a CoinRecord file.
        /// </summary>
        /// <param name="track">The name of the track to be loaded.</param>
        /// <param name="lapCount">The number of laps of the record to be loaded.</param>
        /// <returns>The loaded record.</returns>
        public static CoinRecord Load(string track, int lapCount)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, k_FolderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string dataPath = Path.Combine(folderPath, track + "Coins" + lapCount + k_FileExtension);

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate))
            {
                if (fileStream.Length == 0)
                    return CreateDefault();

                try
                {
                    CoinRecord loadedRecord = binaryFormatter.Deserialize(fileStream) as CoinRecord;

                    if (loadedRecord == null)
                        return CreateDefault();
                    return loadedRecord;
                }
                catch (Exception)
                {
                    return CreateDefault();
                }
            }
        }
    }
}
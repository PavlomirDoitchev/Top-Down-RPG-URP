using System;
using System.IO;
using UnityEngine;
namespace Assets.Scripts.Save_Manager
{
    public class FileDataHandler
    {
        private string dataDirPath = string.Empty;
        private string dataFileName = string.Empty;
        public FileDataHandler(string _dataDirPath, string _dataFileName)
        {
            dataDirPath = _dataDirPath;
            dataFileName = _dataFileName;
        }

        public void Save(GameData _data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(_data, true);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {

                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.LogError("Error saving data: " + fullPath + e);
            }

        }
        public GameData Load() 
        { 
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath)) 
            {
                try 
                {
                    string dataToLoad = string.Empty;
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open)) 
                    {
                        using (StreamReader reader = new StreamReader(stream)) 
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error loading data: " + fullPath + e);
                }
            }
            return loadedData;
        }
    }
}

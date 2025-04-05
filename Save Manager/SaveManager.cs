using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Assets.Scripts.Save_Manager
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        [SerializeField] private string saveFileName;

        private GameData gameData;
        private List<ISaveManager> saveManagers;
        private FileDataHandler dataHandler;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            else 
            {
                Instance = this;
            }
        }

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, saveFileName);
            saveManagers = FindAllSaveManagers();
            LoadGame();
        }
        public void NewGame() 
        {
            gameData = new GameData();
        }
        public void SaveGame()
        {
            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.SaveData(ref gameData);
            }
            dataHandler.Save(gameData);
            //string json = JsonUtility.ToJson(gameData);
            //System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void LoadGame()
        {
            gameData = dataHandler.Load();
            if (this.gameData == null)
            {
                print("No game data found, creating new game data.");
                NewGame();
            }

            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
            }
            //string path = Application.persistentDataPath + "/savefile.json";
            //if (System.IO.File.Exists(path))
            //{
            //    string json = System.IO.File.ReadAllText(path);
            //    gameData = JsonUtility.FromJson<GameData>(json);
            //}
            //else
            //{
            //    gameData = new GameData();
            //}
        }
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISaveManager> FindAllSaveManagers() 
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }
    }
}

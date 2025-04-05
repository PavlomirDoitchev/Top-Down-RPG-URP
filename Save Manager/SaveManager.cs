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
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
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

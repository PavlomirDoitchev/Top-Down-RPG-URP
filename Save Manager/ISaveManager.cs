namespace Assets.Scripts.Save_Manager
{
    public interface ISaveManager
    {
        void LoadData(GameData _data);
        void SaveData(ref GameData _data);
    }
}

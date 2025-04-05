using System.Collections.Generic;

namespace Assets.Scripts.Save_Manager
{
    [System.Serializable]
    public class GameData
    {
        public int playerLevel;
        public int playerExperience;

        float[] playerPos;
        float[] playerRotation;

        public GameData() 
        {
            this.playerLevel = 0;
            this.playerExperience = 0;
            this.playerPos = new float[3];
            this.playerRotation = new float[3];
        }
    }
}

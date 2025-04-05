using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Save_Manager
{
    [System.Serializable]
    public class GameData
    {
        public int playerLevel;
        public int playerExperience;

        [SerializeField]float[] playerPos;
        [SerializeField]float[] playerRotation;

        public GameData() 
        {
            this.playerLevel = 0;
            this.playerExperience = 0;
            this.playerPos = new float[3];
            this.playerRotation = new float[4];
        }
        public void SavePlayerTransform(Vector3 position, Quaternion rotation)
        {
            playerPos[0] = position.x;
            playerPos[1] = position.y;
            playerPos[2] = position.z;

            playerRotation[0] = rotation.x;
            playerRotation[1] = rotation.y;
            playerRotation[2] = rotation.z;
            playerRotation[3] = rotation.w;
        }

        public Vector3 GetPlayerPosition()
        {
            return new Vector3(playerPos[0], playerPos[1], playerPos[2]);
        }

        public Quaternion GetPlayerRotation()
        {
            return new Quaternion(playerRotation[0], playerRotation[1], playerRotation[2], playerRotation[3]);
        }
    }
}

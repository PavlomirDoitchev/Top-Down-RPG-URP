using UnityEngine;

namespace Assets.Scripts.Enemies.EnemyType
{
    public class ArcherHideArrow : MonoBehaviour
    {
        [SerializeField] GameObject arrowPrefab;

        private void Start()
        {
            if (arrowPrefab != null)
            {
                arrowPrefab.SetActive(false); 
            }
            else
            {
                Debug.LogWarning("Arrow prefab is not assigned in the inspector.");
            }
        }
        public void ShowArrow()
        {
            if (arrowPrefab != null)
            {
                arrowPrefab.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Arrow prefab is not assigned in the inspector.");
            }
        }
        public void HideArrow()
        {
            if (arrowPrefab != null)
            {
                arrowPrefab.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Arrow prefab is not assigned in the inspector.");
            }
        }
    }
}

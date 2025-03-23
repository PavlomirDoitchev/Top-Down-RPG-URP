using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassPicker : MonoBehaviour
{
    [SerializeField] private GameObject[] illustratedCharacter;
    
    [SerializeField] private GameObject[] selectedCharacter;
    int index;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        for (int i = 0; i < illustratedCharacter.Length; i++)
        {
            Instantiate(illustratedCharacter[i]);
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            index = 0;
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);

        }
        else if (Input.GetKey(KeyCode.Alpha2)) 
        {
            index = 1;
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 1) 
        {
            Instantiate(selectedCharacter[index]);
            Destroy(this.gameObject);    
        }
    }
}

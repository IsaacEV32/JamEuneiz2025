using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Menu");
    }
}

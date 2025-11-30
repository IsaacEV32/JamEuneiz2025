using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Hello");
        AudioManager.instance.InitializeMusic(FMOD_Events.instance.MainMenuMusic);
    }
    public void Jugar()
    {
        
        SceneManager.LoadScene("DoomScroller");
        AudioManager.instance.StopMusic();
    }
    public void SonidoClick()
    {
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.ConfirmButton, this.transform.position);
    }
    public void Salir()
    {
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

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
        Debug.Log("[MENU] Botón JUGAR pulsado");   // <-- DEBUG

        SceneManager.LoadScene("DoomScroller");
        AudioManager.instance.StopMusic();
    }
    public void SonidoClick()
    {
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.ConfirmButton, this.transform.position);
    }
    public void Salir()
    {
        Debug.Log("[MENU] Botón SALIR pulsado");    // <-- DEBUG

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    InputAction action;
    private bool isPaused = false;
    [SerializeField]Button p;
    [SerializeField] Button b;
    bool isSelected = false;
    [SerializeField]GameManager gameManager;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        action = InputSystem.actions.FindAction("MenuPausa");
        Time.timeScale = 1f;
    }
    private void OnEnable()
    {
        p.Select();
    }
    void Update()
    {
        if (gameManager.tiempoRestante > 0 && gameManager.sliderAnsiedad.value < 100 && gameManager.sliderFelicidad.value > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || action.WasPressedThisFrame())
            {
                TogglePause();
            }
        }
        
    }

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("Menu");
    }
    public void StopMusic()
    {
        AudioManager.instance.StopMusic();
    }
}

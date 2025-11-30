using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    public float tiempoMax = 120f; // duración total de la partida en segundos

    public float tiempoRestante;
    private bool juegoTerminado = false;

    [Header("UI barras y tiempo")]
    public Slider sliderAnsiedad;
    public Slider sliderFelicidad;
    public TextMeshProUGUI textoTiempo;

    [Header("Panel de resultado")]
    public GameObject panelResultado;
    public TextMeshProUGUI textoResultado;

    [Header("Referencias")]
    public TaskManager taskManager;   // <--- arrastra aquí tu TaskManager en el Inspector

    void Start()
    {
        Time.timeScale = 1f;

        tiempoRestante = tiempoMax;

        if (sliderAnsiedad != null)
        {
            sliderAnsiedad.maxValue = 100;
        }

        if (sliderFelicidad != null)
        {
            sliderFelicidad.maxValue = 100;
        }

        if (panelResultado != null)
            panelResultado.SetActive(false);
        AudioManager.instance.InitializeMusic(FMOD_Events.instance.GameplayMusic);
        ActualizarUI();
    }

    void Update()
    {
        if (juegoTerminado) return;

        // Contador de tiempo de partida
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            Victoria("Has sobrevivido al doomscroll");
            
        }

        // Derrota por barras
        if (sliderAnsiedad.value >= 100f || sliderFelicidad.value <= 0f)
        {
            Derrota("Te ha comido la ansiedad :(");
            
        }

        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoTiempo != null)
        {
            int min = Mathf.FloorToInt(tiempoRestante / 60f);
            int seg = Mathf.FloorToInt(tiempoRestante % 60f);
            textoTiempo.text = $"{min:00}:{seg:00}";
        }
    }

    void Victoria(string mensaje)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (panelResultado != null)
            panelResultado.SetActive(true);
        AudioManager.instance.StopMusic();
        if (textoResultado != null)
            textoResultado.text = mensaje;
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.OutOfBattery, this.transform.position);
        Time.timeScale = 0f;
        Debug.Log("VICTORIA: " + mensaje);
    }

    void Derrota(string mensaje)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (panelResultado != null)
            panelResultado.SetActive(true);
        AudioManager.instance.StopMusic();
        if (textoResultado != null)
            textoResultado.text = mensaje;
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.FullOfAnxiety, this.transform.position);
        Time.timeScale = 0f;
        Debug.Log("DERROTA: " + mensaje);
    }

    // Botones del PanelResultado
    public void Reintentar()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // pon aquí el nombre EXACTO de tu escena de menú
    }

    public void GetBarraFelicidad(Slider B) 
    {
        sliderFelicidad.value = B.value;

    }
}

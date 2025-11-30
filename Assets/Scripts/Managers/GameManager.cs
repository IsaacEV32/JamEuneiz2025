using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    public float Ansiedad = 50f;
    public float felicidad = 50f;
    public float tiempoMax = 120f; // duración total de la partida en segundos

    private float tiempoRestante;
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
            sliderAnsiedad.value = Ansiedad;
        }

        if (sliderFelicidad != null)
        {
            sliderFelicidad.maxValue = 100;
            sliderFelicidad.value = felicidad;
        }

        if (panelResultado != null)
            panelResultado.SetActive(false);

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
        if (Ansiedad >= 100f || felicidad <= 0f)
        {
            Derrota("Te ha comido la ansiedad :(");
        }

        // Victoria por haber completado TODAS las tareas
        if (taskManager != null && taskManager.TodasLasTareasCompletadas())
        {
            Victoria("¡Has completado todas las tareas!");
        }

        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (sliderAnsiedad != null)
            sliderAnsiedad.value = Ansiedad;

        if (sliderFelicidad != null)
            sliderFelicidad.value = felicidad;

        if (textoTiempo != null)
        {
            int min = Mathf.FloorToInt(tiempoRestante / 60f);
            int seg = Mathf.FloorToInt(tiempoRestante % 60f);
            textoTiempo.text = $"{min:00}:{seg:00}";
        }
    }

    // Estas funciones las usan posts y minijuegos
    public void ModificarAnsiedad(float delta)
    {
        Ansiedad = Mathf.Clamp(Ansiedad + delta, 0f, 100f);
    }

    public void ModificarFelicidad(float delta)
    {
        felicidad = Mathf.Clamp(felicidad + delta, 0f, 100f);
    }

    void Victoria(string mensaje)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (panelResultado != null)
            panelResultado.SetActive(true);

        if (textoResultado != null)
            textoResultado.text = mensaje;

        Time.timeScale = 0f;
        Debug.Log("VICTORIA: " + mensaje);
    }

    void Derrota(string mensaje)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (panelResultado != null)
            panelResultado.SetActive(true);

        if (textoResultado != null)
            textoResultado.text = mensaje;

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
        felicidad = B.value;

    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public Slider ansiedad;
    public Slider felicidad;

    public float tiempoRestante = 2;
    private bool juegoTerminado = false;

    private bool isAvailableChronometer = false;
    [Header("UI barras y tiempo")]

    //public TextMeshProUGUI textoTiempo;

    [Header ("Panel de resultado")]

    public GameObject panelResultado;
    public TextMeshProUGUI textoResultado;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        

        

        if (panelResultado != null) 
        {
            panelResultado.SetActive(false);
            ActualizarUI();
        }
    }
    public void GetBarraFelicidad(Slider b)
    {
        felicidad = b;
    }

    // Update is called once per frame
    void Update()
    {
        ansiedad.value = Mathf.Clamp(ansiedad.value, 0, 100);
        if (!juegoTerminado)
        {
            if (!isAvailableChronometer)
            {
                isAvailableChronometer = true;
                tiempoRestante -= Time.deltaTime;
                StartCoroutine(DelayForChronometer());
            }
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                Victoria();
            }
            ComprobarDerrota();
        }
        
        
        //ActualizarUI();
    }
    IEnumerator DelayForChronometer()
    {
        yield return new WaitForSeconds(1);
        isAvailableChronometer = false;
    }
    void ComprobarDerrota()
    {
        if (ansiedad.value >= 100 || felicidad.value <= 0f)
        {
            Derrota();
        }
    }
    void ActualizarUI() 
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
    //    textoTiempo.text = $"{minutos:00}:{segundos:00}";
    }

    void Derrota() 
    {
        if(juegoTerminado) return;
        juegoTerminado = true;

        panelResultado.SetActive(true);
        textoResultado.text = "!Bruh te ha explotado la cabeza!";
        Time.timeScale = 0f;
    }

    void Victoria() 
    {
        if(juegoTerminado) return;
        juegoTerminado = true;
        panelResultado.SetActive(true);
        textoResultado.text = "¡Has logrado desconectarte a tiempo!";
        Time.timeScale = 0f;
    }

    public void Reintentar() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}

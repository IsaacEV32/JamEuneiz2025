using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    //public float ansiedad = 50f;
    //public float felicidad = 50;

    private float tiempoRestante;
    private bool juegoTerminado = false;

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

    // Update is called once per frame
    void Update()
    {
        ////if(juegoTerminado) return;
        //tiempoRestante -= Time.deltaTime;

        //if (tiempoRestante <=0f) 
        //{
        //    tiempoRestante = 0f;
        //    Victoria();
        //}
        ////ComprobarDerrota();
        //ActualizarUI();
    }

    //public void ModificarAnsiedad (float delta) 
    //{
        
    //    ansiedad = Mathf.Clamp(ansiedad + delta, 0f, 100f);
    //}

    //public void ModificarFelicidad (float delta) 
    //{
    //    felicidad = Mathf.Clamp(felicidad + delta, 0f, 100f);
    //}

    //void ComprobarDerrota() 
    //{
    //    if (ansiedad <= 0f || felicidad <= 0f) 
    //    {
    //        Derrota();
    //    }
    //}
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

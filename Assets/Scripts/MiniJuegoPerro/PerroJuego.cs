using Unity.VisualScripting;
using UnityEngine;

public class PerroJuego : MonoBehaviour
{

    [Header("Referencias")]
    public RectTransform pelota;
    public RectTransform perro;

    [Header("Logica")]
    public GameManager gameManager;
    public float velocidadPelota = 200f;
    public float Lanzamientos = 3;

    private Vector2 posInicialPelota;
    private bool lanzando = false;
    private bool pelotayendoAlPerro = false;
    private int lanzamientoCompletado = 0;
    private Vector2 destinoActual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (!lanzando && Input.GetKeyDown(KeyCode.Space)) 
        {
            InicarLanzamiento();
        }
        if (lanzando) 
        {
            MoverPelota();
        }
    }

    void InicarLanzamiento() 
    {
        lanzando = true;
        pelotayendoAlPerro = true;
        destinoActual = perro.anchoredPosition;
    }

    void MoverPelota()
    {
        pelota.anchoredPosition = Vector2.MoveTowards(pelota.anchoredPosition, destinoActual, velocidadPelota * Time.deltaTime);
        if(Vector2.Distance(pelota.anchoredPosition, destinoActual) < 5)
        {
            if (pelotayendoAlPerro)
            {
                pelotayendoAlPerro = false;
                destinoActual = posInicialPelota;
            }
            else 
            {
                lanzando = false;
                lanzamientoCompletado++;
                if (lanzamientoCompletado >= Lanzamientos) 
                {
                    CompletarMiniJuego();
                }
            }
        }
    }

    void CompletarMiniJuego() 
    {
        if (gameManager != null) 
        {
            gameManager.ModificarAnsiedad(-10f);
        }

        gameObject.SetActive(false);

        Debug.Log("Mini juego completado");
    }


}

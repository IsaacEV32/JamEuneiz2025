using UnityEngine;
using UnityEngine.InputSystem;

public class MinijuegoPerro : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform panelArea;   
    public RectTransform pelota;
    public RectTransform perro;
    public GameManager gameManager;

    InputAction throwBall;

    [Header("Parámetros")]
    public float velocidadPelota = 600f;
    public float velocidadPerro = 500f;
    public int atrapadasNecesarias = 3;
    public float distanciaAtrapar = 40f; 
    public float margenBordes = 20f;

    private Vector2 posInicialPelota;
    private Vector2 posInicialPerro;
    private Vector2 velocidadPelotaActual;
    private int atrapadas = 0;

    bool waitForBall = true;

    private enum Estado
    {
        EsperandoLanzar,
        PelotaVolando,
        PerroVolviendoConPelota
    }

    private Estado estadoActual = Estado.EsperandoLanzar;

    
    private Vector2 offsetPelotaPerro;

    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
        posInicialPerro = perro.anchoredPosition;
        estadoActual = Estado.EsperandoLanzar;
        throwBall = InputSystem.actions.FindAction("Button A");
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        switch (estadoActual)
        {
            case Estado.EsperandoLanzar:
                UpdateEsperandoLanzar();
                break;

            case Estado.PelotaVolando:
                UpdatePelotaVolando();
                break;

            case Estado.PerroVolviendoConPelota:
                UpdatePerroVolviendoConPelota();
                break;
        }
    }

    

    void UpdateEsperandoLanzar()
    {
        
        pelota.anchoredPosition = posInicialPelota;

        
        if (throwBall.IsPressed() && waitForBall)
        {
            waitForBall = false;
            LanzarPelota();
        }
    }

    void UpdatePelotaVolando()
    {
        MoverPelotaConRebotes();
        MoverPerroHaciaPelota();
        ComprobarAtraparPelota();
    }

    void UpdatePerroVolviendoConPelota()
    {
        // El perro vuelve a la zona inicial llevando la pelota
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            posInicialPerro,
            velocidadPerro * Time.deltaTime
        );

        
        pelota.anchoredPosition = perro.anchoredPosition + offsetPelotaPerro;

        
        if (Vector2.Distance(perro.anchoredPosition, posInicialPerro) < 5f)
        {
            atrapadas++;

            if (atrapadas >= atrapadasNecesarias)
            {
                perro.anchoredPosition = posInicialPerro;
                pelota.anchoredPosition = posInicialPelota;
                estadoActual = Estado.EsperandoLanzar;
                CompletarMinijuego();
            }
            else
            {
                // para preparar el siguiente lanzamiento
                perro.anchoredPosition = posInicialPerro;
                pelota.anchoredPosition = posInicialPelota;
                estadoActual = Estado.EsperandoLanzar;
            }
        }
        waitForBall = true;

    }

    

    void LanzarPelota()
    {
        
        Vector2 dir = Random.insideUnitCircle.normalized;
        velocidadPelotaActual = dir * velocidadPelota;
        estadoActual = Estado.PelotaVolando;
    }

    void MoverPelotaConRebotes()
    {
        Vector2 pos = pelota.anchoredPosition;
        pos += velocidadPelotaActual * Time.deltaTime;

        Rect r = panelArea.rect;

        float minX = r.xMin + margenBordes;
        float maxX = r.xMax - margenBordes;
        float minY = r.yMin + margenBordes;
        float maxY = r.yMax - margenBordes;

        
        if (pos.x < minX || pos.x > maxX)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            velocidadPelotaActual.x = -velocidadPelotaActual.x;
        }

        
        if (pos.y < minY || pos.y > maxY)
        {
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            velocidadPelotaActual.y = -velocidadPelotaActual.y;
        }

        pelota.anchoredPosition = pos;
    }

    void MoverPerroHaciaPelota()
    {
        perro.anchoredPosition = Vector2.MoveTowards(perro.anchoredPosition,pelota.anchoredPosition,velocidadPerro * Time.deltaTime);
    }

    void ComprobarAtraparPelota()
    {
        float dist = Vector2.Distance(perro.anchoredPosition, pelota.anchoredPosition);

        if (dist < distanciaAtrapar)
        {
            
            offsetPelotaPerro = pelota.anchoredPosition - perro.anchoredPosition;

            
            estadoActual = Estado.PerroVolviendoConPelota;
        }
    }

    void CompletarMinijuego()
    {
        if (gameManager != null)
        {
            //gameManager.ModificarAnsiedad(-10f);
        }

        gameObject.SetActive(false);
        Debug.Log("Minijuego del perro COMPLETADO");
    }
}

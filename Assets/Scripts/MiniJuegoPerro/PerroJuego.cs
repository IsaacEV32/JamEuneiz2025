using System.Collections;
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

    bool canYouIncreaseAnxiety = true;

    bool waitForChange = false;

    private bool minijuegoCompletado = false;
    private enum Estado
    {
        EsperandoLanzar,
        PelotaVolando,
        PerroVolviendoConPelota,
        Completado
    }

    private Estado estadoActual = Estado.EsperandoLanzar;

    
    private Vector2 offsetPelotaPerro;

    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
        posInicialPerro = perro.anchoredPosition;
        estadoActual = Estado.EsperandoLanzar;
        minijuegoCompletado = false;
        throwBall = InputSystem.actions.FindAction("Button A");
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && !minijuegoCompletado)
        {
            if (!waitForChange)
            {
                switch (estadoActual)
                {
                    case Estado.EsperandoLanzar:
                        waitForChange = true;
                        UpdateEsperandoLanzar();
                        waitForChange = false;
                        break;

                    case Estado.PelotaVolando:
                        waitForChange = true;
                        UpdatePelotaVolando();
                        waitForChange = false;
                        break;

                    case Estado.PerroVolviendoConPelota:
                        waitForChange = true;
                        UpdatePerroVolviendoConPelota();
                        waitForChange = false;
                        break;
                    case Estado.Completado:
                        break;
                }
            }
        }

        

        if (canYouIncreaseAnxiety && !minijuegoCompletado)
        {
            canYouIncreaseAnxiety = false;
            gameManager.ansiedad.value++;
            StartCoroutine(DelayForAnxietyIncrease());
        }
    }
    IEnumerator DelayForAnxietyIncrease()
    {
        yield return new WaitForSeconds(1);
        if (!minijuegoCompletado)
        {
            canYouIncreaseAnxiety = true;
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
                CompletarMinijuego();
                waitForBall = true;
                return;
            }
            else
            {
                // para preparar el siguiente lanzamiento
                perro.anchoredPosition = posInicialPerro;
                pelota.anchoredPosition = posInicialPelota;
                estadoActual = Estado.EsperandoLanzar;
                
            }
            waitForBall = true;
        }
       

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
        minijuegoCompletado = true;
        estadoActual = Estado.Completado;
        if (gameManager != null)
        {
            gameManager.ansiedad.value = gameManager.ansiedad.value - 10;
        }
        canYouIncreaseAnxiety = false;
        StopAllCoroutines();
        pelota.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log("Minijuego del perro COMPLETADO");
    }
}

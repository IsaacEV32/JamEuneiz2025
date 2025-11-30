using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class MinijuegoPerroSimple : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform panelArea;   // El panel del minijuego (PerroPanel)
    public RectTransform pelota;
    public RectTransform perro;
    public GameManager gameManager;
    public TaskManager taskManager;

    InputAction throwBall;

    [Header("Parámetros")]
    public float velocidadPelota = 800f;
    public float velocidadPerro = 700f;
    public int lanzamientosNecesarios = 3;
    public float distanciaUmbral = 5f;
    public float margenBordes = 50f;      // margen para no ir pegado al borde

    private Vector2 posInicialPelota;
    private Vector2 posInicialPerro;
    private Vector2 objetivoPelota;       // punto aleatorio al que va la pelota
    private int lanzamientosHechos = 0;

    private bool minijuegoCompletado = false;
    bool waitForChange = false;
    bool canYouIncreaseAnxiety = true;
    bool waitForBall;


    private enum Estado
    {
        EsperandoLanzar,
        PelotaVolando,
        PerroVolviendoConPelota,
        Completado
    }

    private Estado estadoActual = Estado.EsperandoLanzar;

    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
        posInicialPerro = perro.anchoredPosition;
        estadoActual = Estado.EsperandoLanzar;
        taskManager.GetPerro(this);
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
                        UpdatePelotaHaciaObjetivo();
                        waitForChange = false;
                        break;

                    case Estado.PerroVolviendoConPelota:
                        waitForChange = true;
                        UpdatePerroVuelveConPelota();
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
            gameManager.sliderAnsiedad.value++;
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
        // Aseguramos posiciones base
        pelota.anchoredPosition = posInicialPelota;
        perro.anchoredPosition = posInicialPerro;

        if (lanzamientosHechos >= lanzamientosNecesarios)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            estadoActual = Estado.PelotaVolando;
        }
    }
    void UpdatePelotaHaciaObjetivo()
    {
        // Pelota va desde el origen hacia un punto aleatorio del panel
        pelota.anchoredPosition = Vector2.MoveTowards(
            pelota.anchoredPosition,
            objetivoPelota,
            velocidadPelota * Time.deltaTime
        );

        if (Vector2.Distance(pelota.anchoredPosition, objetivoPelota) < distanciaUmbral)
        {
            // Cuando llega, perro empieza a volver con la pelota
            estadoActual = Estado.PerroVolviendoConPelota;
        }
    }
    void UpdatePerroVolviendoConPelota()
    {
        // El perro corre hacia donde est� la pelota
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            pelota.anchoredPosition,
            velocidadPerro * Time.deltaTime
        );

        if (Vector2.Distance(perro.anchoredPosition, pelota.anchoredPosition) < distanciaUmbral)
        {
            // Cuando la alcanza, empieza a volver con ella
            estadoActual = Estado.PerroVolviendoConPelota;
        }
    }

    void UpdatePerroVuelveConPelota()
    {
        // Perro vuelve a su posición inicial
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            posInicialPerro,
            velocidadPerro * Time.deltaTime
        );

        // La pelota va pegada al perro, como si la llevara en la boca
        pelota.anchoredPosition = perro.anchoredPosition;

        if (Vector2.Distance(perro.anchoredPosition, posInicialPerro) < distanciaUmbral)
        {
            lanzamientosHechos++;

            if (lanzamientosHechos >= lanzamientosNecesarios)
            {
                CompletarMinijuego();
                waitForBall = true;
                return;
            }
            else
            {
                // Vuelta al estado de esperar -> siguiente lanzamiento
                estadoActual = Estado.EsperandoLanzar;
            }
        }
    }
    void ElegirObjetivoAleatorio()
    {
        Rect r = panelArea.rect;

        float minX = r.xMin + margenBordes;
        float maxX = r.xMax - margenBordes;
        float minY = r.yMin + margenBordes;
        float maxY = r.yMax - margenBordes;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        objetivoPelota = new Vector2(x, y);
    }
    void CompletarMinijuego()
    {
        minijuegoCompletado = true;
        estadoActual = Estado.Completado;
        if (gameManager != null)
        {
            gameManager.sliderAnsiedad.value = gameManager.sliderAnsiedad.value - 10;
        }
        canYouIncreaseAnxiety = false;
        StopAllCoroutines();
        pelota.gameObject.SetActive(false);


            if (taskManager != null)
                Debug.Log("ChangeMinigame");
            taskManager.NotificarMinijuegoTerminado();

        gameObject.SetActive(false);
        Debug.Log("Minijuego del perro COMPLETADO (random simple)");
    }
    public void ResetMinijuego()
    {
        lanzamientosHechos = 0;
        minijuegoCompletado = false;
        estadoActual = Estado.EsperandoLanzar;
        waitForBall = true;
        canYouIncreaseAnxiety = true;

        pelota.anchoredPosition = posInicialPelota;
        perro.anchoredPosition = posInicialPerro;

        pelota.gameObject.SetActive(true);
        perro.gameObject.SetActive(true);
    }
}

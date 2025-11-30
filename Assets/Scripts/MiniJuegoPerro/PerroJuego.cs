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

    bool esperarPelota = true;
    bool lanzarPelota = true;
    bool volverConPelota = true;

    private enum Estado
    {
        EsperandoLanzar,
        PelotaVolando,
        PerroBuscandoPelota,       // 👉 NUEVO estado
        PerroVolviendoConPelota,
        Completado
    }

    private Estado estadoActual = Estado.EsperandoLanzar;

    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
        posInicialPerro = perro.anchoredPosition;
        estadoActual = Estado.EsperandoLanzar;

        if (taskManager != null)
            taskManager.GetPerro(this);

        minijuegoCompletado = false;

        // Puede ser null en PC si no tienes el asset cargado, así evitamos errores
        if (InputSystem.actions != null)
            throwBall = InputSystem.actions.FindAction("Button A");
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && !minijuegoCompletado)
        {
            if (!waitForChange)
            {
                waitForChange = true;

                switch (estadoActual)
                {
                    case Estado.EsperandoLanzar:
                        UpdateEsperandoLanzar();
                        break;

                    case Estado.PelotaVolando:
                        UpdatePelotaHaciaObjetivo();
                        break;

                    case Estado.PerroBuscandoPelota:
                        UpdatePerroVolviendoConPelota();   // 👉 va hacia la pelota
                        break;

                    case Estado.PerroVolviendoConPelota:
                        UpdatePerroVuelveConPelota();      // 👉 vuelve al origen con la pelota
                        break;

                    case Estado.Completado:
                        break;
                }

                waitForChange = false;
            }
        }

        if (canYouIncreaseAnxiety && !minijuegoCompletado && gameManager != null)
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

        if (esperarPelota)
        {
            volverConPelota = false;
            AudioManager.instance.PlayOneShot(FMOD_Events.instance.LadridoPerro, this.transform.position);
            esperarPelota = false;
        }

        bool botonTeclado = Input.GetKeyDown(KeyCode.Space);
        bool botonGamepad = (throwBall != null && throwBall.IsPressed());

        if (botonTeclado || botonGamepad)
        {
            // 👉 Elegimos un destino ALEATORIO dentro del panel ANTES de lanzar
            ElegirObjetivoAleatorio();

            estadoActual = Estado.PelotaVolando;

            if (lanzarPelota)
            {
                AudioManager.instance.PlayOneShot(FMOD_Events.instance.LanzarPelota, this.transform.position);
                lanzarPelota = false;
            }
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

        // Cuando la pelota llega al objetivo, pasamos al estado en el que el perro la busca
        if (Vector2.Distance(pelota.anchoredPosition, objetivoPelota) < distanciaUmbral)
        {
            estadoActual = Estado.PerroBuscandoPelota;
        }
    }

    // 👉 El perro se mueve HASTA donde está la pelota
    void UpdatePerroVolviendoConPelota()
    {
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            pelota.anchoredPosition,
            velocidadPerro * Time.deltaTime
        );

        if (Vector2.Distance(perro.anchoredPosition, pelota.anchoredPosition) < distanciaUmbral)
        {
            // La ha alcanzado -> ahora toca volver al origen con ella
            estadoActual = Estado.PerroVolviendoConPelota;
        }
    }

    // 👉 El perro vuelve a su posición inicial llevando la pelota
    void UpdatePerroVuelveConPelota()
    {
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            posInicialPerro,
            velocidadPerro * Time.deltaTime
        );

        // La pelota va pegada al perro, como si la llevara en la boca
        pelota.anchoredPosition = perro.anchoredPosition;

        if (volverConPelota)
        {
            AudioManager.instance.PlayOneShot(FMOD_Events.instance.PerroRecogePelota, this.transform.position);
            volverConPelota = false;
            esperarPelota = true;
            lanzarPelota = true;
        }

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
                volverConPelota = true;
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
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.CompletarMinijuego, this.transform.position);

        if (taskManager != null)
        {
            Debug.Log("ChangeMinigame");
            taskManager.NotificarMinijuegoTerminado();
        }

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

        esperarPelota = true;
        lanzarPelota = true;
        volverConPelota = true;
    }
}

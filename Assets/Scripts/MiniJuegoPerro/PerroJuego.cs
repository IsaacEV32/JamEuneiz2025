using UnityEngine;

public class PerroJuego : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform pelota;
    public RectTransform perro;
    public GameManager gameManager;
    public TaskManager taskManager;

    [Header("Parámetros")]
    public float velocidadPelota = 800f;
    public float velocidadPerro = 700f;
    public int lanzamientosNecesarios = 3;
    public float distanciaUmbral = 5f;

    private Vector2 posInicialPelota;
    private Vector2 posInicialPerro;
    private int lanzamientosHechos = 0;

    private bool minijuegoCompletado = false;
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

    void UpdateEsperando()
    {
        // Aseguramos posiciones base
        pelota.anchoredPosition = posInicialPelota;
        perro.anchoredPosition = posInicialPerro;

        if (lanzamientosHechos >= lanzamientosNecesarios)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            estadoActual = Estado.PelotaHaciaPerro;
        }
    }

    void UpdatePelotaHaciaPerro()
    {
        // Pelota va recta al perro
        pelota.anchoredPosition = Vector2.MoveTowards(
            pelota.anchoredPosition,
            perro.anchoredPosition,
            velocidadPelota * Time.deltaTime
        );

        if (Vector2.Distance(pelota.anchoredPosition, perro.anchoredPosition) < distanciaUmbral)
        {
            // Cuando llega, perro empieza a volver con la pelota
            estadoActual = Estado.PelotaDeVuelta;
        }
    }

    void UpdatePelotaDeVuelta()
    {
        // Perro vuelve a su posición inicial
        perro.anchoredPosition = Vector2.MoveTowards(
            perro.anchoredPosition,
            posInicialPerro,
            velocidadPerro * Time.deltaTime
        );

        // Pelota va pegada al perro, como si la llevara en la boca
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
                // Preparado para siguiente lanzamiento
                estadoActual = Estado.EsperandoLanzar;
            }
        }
    }

    void CompletarMinijuego()
    {
        if (estadoActual == Estado.Terminado) return;
        estadoActual = Estado.Terminado;

        // Pequeño ajuste visual: devolvemos pelota al origen
        pelota.anchoredPosition = posInicialPelota;
        perro.anchoredPosition = posInicialPerro;

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
        if (gameManager != null)
            //gameManager.ModificarAnsiedad(-10f);

        if (taskManager != null)
            taskManager.NotificarMinijuegoTerminado();

        gameObject.SetActive(false);
        Debug.Log("Minijuego del perro COMPLETADO (simple)");
    }
    public void ResetMinijuego()
    {
        atrapadas = 0;
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

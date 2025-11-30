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

    private enum Estado
    {
        EsperandoLanzar,
        PelotaHaciaPerro,
        PelotaDeVuelta,
        Terminado
    }

    private Estado estadoActual = Estado.EsperandoLanzar;

    void Start()
    {
        posInicialPelota = pelota.anchoredPosition;
        posInicialPerro = perro.anchoredPosition;
        estadoActual = Estado.EsperandoLanzar;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (estadoActual == Estado.Terminado) return;

        switch (estadoActual)
        {
            case Estado.EsperandoLanzar:
                UpdateEsperando();
                break;

            case Estado.PelotaHaciaPerro:
                UpdatePelotaHaciaPerro();
                break;

            case Estado.PelotaDeVuelta:
                UpdatePelotaDeVuelta();
                break;
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

        if (gameManager != null)
            //gameManager.ModificarAnsiedad(-10f);

        if (taskManager != null)
            taskManager.NotificarMinijuegoTerminado();

        gameObject.SetActive(false);
        Debug.Log("Minijuego del perro COMPLETADO (simple)");
    }
}

using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Lista de minijuegos en orden")]
    public GameObject[] minijuegos;   // 0 = perro, 1 = mesa, etc.

    private int indiceActual = -1;
    private int minijuegosCompletados = 0;

    void Start()
    {
        // Desactivar todos al inicio
        if (minijuegos != null)
        {
            foreach (var mj in minijuegos)
            {
                if (mj != null) mj.SetActive(false);
            }
        }

        ActivarSiguienteMinijuego();
    }

    void ActivarSiguienteMinijuego()
    {
        indiceActual++;

        if (minijuegos == null || minijuegos.Length == 0)
        {
            Debug.LogWarning("TaskManager: no hay minijuegos asignados.");
            return;
        }

        // Si ya hemos pasado el último, no activamos nada más
        if (indiceActual >= minijuegos.Length)
        {
            Debug.Log("TaskManager: todas las tareas completadas.");
            return;
        }

        // Activar solo el minijuego actual y desactivar el resto
        for (int i = 0; i < minijuegos.Length; i++)
        {
            if (minijuegos[i] != null)
                minijuegos[i].SetActive(i == indiceActual);
        }

        Debug.Log("TaskManager: activando minijuego " + minijuegos[indiceActual].name);
    }

    // Llamado por los minijuegos cuando terminan
    public void NotificarMinijuegoTerminado()
    {
        minijuegosCompletados++;
        Debug.Log("TaskManager: minijuego terminado. Completados = " + minijuegosCompletados);
        ActivarSiguienteMinijuego();
    }

    public bool TodasLasTareasCompletadas()
    {
        if (minijuegos == null) return false;
        return minijuegosCompletados >= minijuegos.Length;
    }
}

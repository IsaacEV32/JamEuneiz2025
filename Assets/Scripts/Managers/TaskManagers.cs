using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Lista de minijuegos en orden")]
    public GameObject[] minijuegos;   // 0 = perro, 1 = mesa, etc.
    [SerializeField]MinijuegoPerroSimple dog;

    private int indiceActual = 0;
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
        Debug.Log(minijuegos.Length);
        ActivarSiguienteMinijuego();
    }

    void ActivarSiguienteMinijuego()
    {
        if (minijuegos == null || minijuegos.Length == 0)
        {
            Debug.LogWarning("TaskManager: no hay minijuegos asignados.");
            return;
        }
        // Si ya hemos pasado el último, no activamos nada más
        if (indiceActual > minijuegos.Length - 1)
        {
            indiceActual = 0;
            dog.ResetMinijuego();
            Debug.Log("TaskManager: todas las tareas completadas.");
        }
        if (indiceActual > 0)
        {
            Debug.Log("Entre aqui");
            for (int i = 0; i < minijuegos.Length; i++)
            {
                minijuegos[i].SetActive(false);
            }
            minijuegos[indiceActual].SetActive(true);
        }
        else
        {
            minijuegos[indiceActual].SetActive(true);
        }
            
        Debug.Log("TaskManager: activando minijuego " + minijuegos[indiceActual].name);
    }

    // Llamado por los minijuegos cuando terminan
    public void NotificarMinijuegoTerminado()
    {
        indiceActual++;
        Debug.Log("TaskManager: minijuego terminado. Completados = " + minijuegosCompletados);
        ActivarSiguienteMinijuego();
    }
    public void GetPerro(MinijuegoPerroSimple p)
    {
        dog = p;
    }
}

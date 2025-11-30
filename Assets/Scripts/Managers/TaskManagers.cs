using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameManager gameManager;
    [Header("Lista de minijuegos en orden")]
    public GameObject[] minijuegos;   

    private int indiceActual = -1;

    void Start()
    {
        
        if (minijuegos != null)
        {
            foreach (var mj in minijuegos)
            {
                if (mj != null) mj.SetActive(false);
            }
        }

        // Activar el primero juego
        ActivarSiguienteMinijuego();
    }

    void ActivarSiguienteMinijuego()
    {
        indiceActual++;

        if (minijuegos == null || minijuegos.Length == 0)
        {
            Debug.LogWarning("No hay minijuegos asignados en el TaskManager.");
            return;
        }


        if (indiceActual >= minijuegos.Length)
        {
            Debug.Log("Todas las tareas completadas.");

            // Avisar al GameManager para que muestre el panel de victoria
            if (gameManager != null)
            {
                gameManager.VictoriaPorTareas();
            }

            return;
        }



        for (int i = 0; i < minijuegos.Length; i++)
        {
            if (minijuegos[i] != null)
                minijuegos[i].SetActive(i == indiceActual);
        }

        Debug.Log("Activando minijuego: " + minijuegos[indiceActual].name);
    }

   
    public void NotificarMinijuegoTerminado()
    {
        ActivarSiguienteMinijuego();
    }
}

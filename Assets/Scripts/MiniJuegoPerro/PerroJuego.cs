using UnityEngine;

public class PerroJuego : MonoBehaviour
{
    public GameManager gameManager;
    public int atrapadasNecesarias = 3;

    private int atrapadas = 0;

    public void BallCaught()
    {
        atrapadas++;

        if (atrapadas >= atrapadasNecesarias)
        {
            Completar();
        }
    }

    void Completar()
    {
        //gameManager.ModificarAnsiedad(-10f);
        gameObject.SetActive(false);

        Debug.Log("Minijuego completado: perro atrapó 3 pelotas");
    }
}

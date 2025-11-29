using UnityEngine;
using UnityEngine.UI;

public enum TipoPosts
{
    Divertido, Depresivo
};
public class Posts : MonoBehaviour
{
    public Image imagenPrincipal;
    public Sprite[] imagenesDisponiblesFelices;
    public Sprite[] imagenesDisponiblesDepresivas;

    [SerializeField] internal TipoPosts posts;
    float numRandom = 0;
    float numRandomPost = 0;
    [SerializeField] Scroll_Control scrollControl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollControl.GetThisPost(this);
        numRandom = Random.value;
        float roundedValue = Mathf.Round(numRandom * 10f) / 10f;

        Debug.Log(roundedValue);
        if (numRandom <= 0.5f)
        {
            this.posts = TipoPosts.Divertido;
            numRandomPost = Random.value;
            float roundedValueFunny = Mathf.Round(numRandomPost * 10f) / 10f;

            if (numRandomPost <= 0.3)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[0];
            }
            else if(numRandomPost <= 0.7)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[1];
            }
            else if(numRandomPost <= 1.0f)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[2];
            }
        }
        else if (numRandom > 0.5f)
        {

            this.posts = TipoPosts.Depresivo;
            if (numRandomPost <= 0.3)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[0];
            }
            else if (numRandomPost <= 0.7)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[1];
            }
            else if (numRandomPost <= 1.0f)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[2];
            }
        }
    }
    
    internal void ChangeTipe()
    {
        numRandom = Random.value;
        float roundedValue = Mathf.Round(numRandom * 10f) / 10f;
        Debug.Log(roundedValue);
        if (roundedValue <= 0.5f)
        {
            this.posts = TipoPosts.Divertido;
            numRandomPost = Random.value;
            float roundedValueFunny = Mathf.Round(numRandomPost * 10f) / 10f;

            if (numRandomPost <= 0.3)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[0];
            }
            else if (numRandomPost <= 0.7)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[1];
            }
            else if (numRandomPost <= 1.0f)
            {
                imagenPrincipal.sprite = imagenesDisponiblesFelices[2];
            }
        }
        else if (roundedValue > 0.5f)
        {
            this.posts = TipoPosts.Depresivo;
            if (numRandomPost <= 0.3)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[0];
            }
            else if (numRandomPost <= 0.7)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[1];
            }
            else if (numRandomPost <= 1.0f)
            {
                imagenPrincipal.sprite = imagenesDisponiblesDepresivas[2];
            }
        }
    }
}

using UnityEngine;

public enum TipoPosts
{
    Divertido, Depresivo
};
public class Posts : MonoBehaviour
{
    
    [SerializeField] internal TipoPosts posts;
    float numRandom = 0;
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
        }
        else if (numRandom > 0.5f)
        {

            this.posts = TipoPosts.Depresivo;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        

    }
    internal void ChangeTipe()
    {
        numRandom = Random.value;
        float roundedValue = Mathf.Round(numRandom * 10f) / 10f;
        Debug.Log(roundedValue);
        if (roundedValue <= 0.5f)
        {
            this.posts = TipoPosts.Divertido;
        }
        else if (roundedValue > 0.5f)
        {

            this.posts = TipoPosts.Depresivo;
        }
    }
}

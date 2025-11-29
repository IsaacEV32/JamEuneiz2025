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
        numRandom = Random.value % 1;
        Debug.Log(numRandom);
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
        numRandom = Random.value % 1;
        Debug.Log(numRandom);
        if (numRandom <= 0.5f)
        {
            this.posts = TipoPosts.Divertido;
        }
        else if (numRandom > 0.5f)
        {

            this.posts = TipoPosts.Depresivo;
        }
    }
}

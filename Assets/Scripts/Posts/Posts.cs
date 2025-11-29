using UnityEngine;

 
public class Posts : MonoBehaviour
{
    enum TipoPosts 
    { 
        Divertido, Depresivo
    };
    [SerializeField] TipoPosts posts;
    int numRandom = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numRandom = Random.Range(1,2);
        Debug.Log(numRandom);
        if (numRandom == 1)
        {
            this.posts = TipoPosts.Divertido;
        }
        else if(numRandom == 2)
        {
            this.posts = TipoPosts.Depresivo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

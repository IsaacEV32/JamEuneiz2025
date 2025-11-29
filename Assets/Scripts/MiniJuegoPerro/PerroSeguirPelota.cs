using UnityEngine;

public class PerroSeguirPelota : MonoBehaviour
{
    public Transform pelota;
    public float speed = 600f;

    void Update()
    {
        
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            pelota.localPosition,
            speed * Time.deltaTime
        );
    }
}

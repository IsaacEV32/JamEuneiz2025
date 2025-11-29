using UnityEngine;

public class PelotaPerro : MonoBehaviour
{
    public float speed = 400f;
    private Rigidbody2D rb;
    private Vector2 initialPos;
    private bool launched = false;

    public PerroJuego minigame; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPos = transform.localPosition;
    }

    void Update()
    {
        if (!launched && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        launched = true;

        
        Vector2 dir = Random.insideUnitCircle.normalized;

        rb.AddForce(dir * speed);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        

        
        if (collision.collider.CompareTag("Perro"))
        {
            minigame.BallCaught();
            ResetBall();
        }
    }

    void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.localPosition = initialPos;

        launched = false;
    }
}

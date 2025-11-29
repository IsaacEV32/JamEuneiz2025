using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoLimpiezaMesa : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform panelArea;       
    public RectTransform trapo;
    public List<Image> manchas;          
    public GameManager gameManager;

    [Header("Parámetros")]
    public float velocidadTrapo = 600f;
    public float radioLimpieza = 60f;     
    public float velocidadLimpieza = 1.5f; 
    public float porcentajeNecesario = 95f;

    private Vector2 trapoPosInicial;
    private int totalManchas;
    private int manchasLimpias = 0;

    void Start()
    {
        trapoPosInicial = trapo.anchoredPosition;
        totalManchas = manchas.Count;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        MoverTrapo();
        LimpiarManchas();
        ComprobarCompletado();
    }

    void MoverTrapo()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 delta = new Vector2(h, v).normalized * velocidadTrapo * Time.deltaTime;
        Vector2 nuevaPos = trapo.anchoredPosition + delta;

        
        Rect r = panelArea.rect;
        float halfWidth = trapo.rect.width * 0.5f;
        float halfHeight = trapo.rect.height * 0.5f;

        float minX = r.xMin + halfWidth;
        float maxX = r.xMax - halfWidth;
        float minY = r.xMin + halfHeight;
        float maxY = r.yMax - halfHeight;

        nuevaPos.x = Mathf.Clamp(nuevaPos.x, minX, maxX);
        nuevaPos.y = Mathf.Clamp(nuevaPos.y, minY, maxY);

        trapo.anchoredPosition = nuevaPos;
    }

    void LimpiarManchas()
    {
        foreach (Image mancha in manchas)
        {
            if (mancha == null) continue; 

            
            float dist = Vector2.Distance(trapo.anchoredPosition,
                                          ((RectTransform)mancha.rectTransform).anchoredPosition);

            if (dist < radioLimpieza)
            {
                
                Color c = mancha.color;
                c.a -= velocidadLimpieza * Time.deltaTime;
                c.a = Mathf.Clamp01(c.a);
                mancha.color = c;

                
                if (c.a <= 0.05f)
                {
                    manchasLimpias++;
                    Destroy(mancha.gameObject);
                }
            }
        }

        
        manchas.RemoveAll(m => m == null);
    }

    void ComprobarCompletado()
    {
        if (totalManchas == 0) return;

        float porcentajeLimpio = (manchasLimpias / (float)totalManchas) * 100f;

        if (porcentajeLimpio >= porcentajeNecesario)
        {
            CompletarMinijuego();
        }
    }

    void CompletarMinijuego()
    {
        if (gameManager != null)
        {
            //gameManager.ModificarAnsiedad(-10f);
        }

        
        trapo.anchoredPosition = trapoPosInicial;

        gameObject.SetActive(false);
        Debug.Log("Minijuego de limpiar la mesa completado");
    }
}

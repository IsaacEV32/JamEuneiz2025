using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoLimpiezaMesa : MonoBehaviour
{
    public TaskManager taskManager;

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
    private List<float> alphasIniciales = new List<float>();
    public bool reiniciarAlpha = true;

    bool canYouIncreaseAnxiety = true;
    private bool minijuegoCompletado = false;
    void Start()
    {
        trapoPosInicial = trapo.anchoredPosition;
        totalManchas = manchas.Count;
        GuardarEstadosIniciales();
    }
    void GuardarEstadosIniciales()
    {
        alphasIniciales.Clear();
        foreach (Image mancha in manchas)
        {
            if (mancha != null)
            {
                alphasIniciales.Add(mancha.color.a);
            }
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        MoverTrapo();
        LimpiarManchas();
        ComprobarCompletado();
        if (canYouIncreaseAnxiety && !minijuegoCompletado)
        {
            canYouIncreaseAnxiety = false;
            gameManager.sliderAnsiedad.value++;
            StartCoroutine(DelayForAnxietyIncrease());
        }
    }
    IEnumerator DelayForAnxietyIncrease()
    {
        yield return new WaitForSeconds(0.6f);
        if (!minijuegoCompletado)
        {
            canYouIncreaseAnxiety = true;
        }
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
        float minY = r.yMin + halfHeight;
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
                    //AudioManager.instance.PlayOneShot(FMOD_Events.instance.PasarToalla, this.transform.position);
                    mancha.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    void ComprobarCompletado()
    {
        if (totalManchas == 0) return;

        float limpiezaTotal = 0f;

        foreach (Image mancha in manchas)
        {
            if (mancha != null)
            {
                if (!mancha.gameObject.activeInHierarchy)
                {
                    // Mancha completamente limpia (desactivada)
                    limpiezaTotal += 1f;
                }
                else
                {
                    // Mancha parcialmente limpia - cuanto más transparente, más limpia
                    limpiezaTotal += (1f - mancha.color.a);
                }
            }
        }

        float porcentajeLimpio = (limpiezaTotal / totalManchas) * 100f;

        Debug.Log($"Limpieza total: {limpiezaTotal}/{totalManchas}, Porcentaje: {porcentajeLimpio}%");

        if (porcentajeLimpio >= porcentajeNecesario)
        {
            CompletarMinijuego();
        }
    }

    void CompletarMinijuego()
    {
        minijuegoCompletado = true;
        if (gameManager != null)
        {
            gameManager.sliderAnsiedad.value = gameManager.sliderAnsiedad.value - 10;
        }
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.CompletarMinijuego, this.transform.position);
        // Opcional: resetear trapo si quieres
        trapo.anchoredPosition = trapoPosInicial;

        // Avisar al TaskManager
        if (taskManager != null)
        {
            taskManager.NotificarMinijuegoTerminado();
        }

        gameObject.SetActive(false);
        RestartMesa();
        Debug.Log("Minijuego de limpiar mesa COMPLETADO");
    }
    public void RestartMesa()
    {
        for (int i = 0; i < manchas.Count; i++)
        {
            if (manchas[i] != null)
            {
                manchas[i].gameObject.SetActive(true);
                if (reiniciarAlpha && i < alphasIniciales.Count)
                {
                    Color c = manchas[i].color;
                    c.a = alphasIniciales[i];
                    manchas[i].color = c;
                }
            }
        }
        minijuegoCompletado = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Scroll_Control : MonoBehaviour
{
    //Mapa de acciones del stick derecho
    InputAction scrollController;
    //Controla el slider de la barra de felicidad
    [SerializeField] Slider barraFelicidad;
    //Sirven para controlar que no se haga de forma inmediata los cambios de la barra de felicidad
    bool isPressingAvailable = true;
    bool delayForDecreasing = true;
    //Sirve para controlar el input del jugador
    bool delayForPressScroll = true;
    //Sirve para encontrar el post
    Posts post;
    // Se encuentra el mapa de acciones de PlayerController Y para obtener la barra de felicidad
    void Start()
    {
        scrollController = InputSystem.actions.FindAction("PlayerController");
        barraFelicidad = gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        //Es el comportamiento de los posts 
        if (post.posts == TipoPosts.Divertido)
        {
            //El máximo y mínimo es 0 y 100
            barraFelicidad.value = Mathf.Clamp(barraFelicidad.value, 0, 99);
            //Se sumara la barra de felicidad
            if (isPressingAvailable)
            {
                barraFelicidad.value++;
                isPressingAvailable = false;
                StartCoroutine(DelayOfGrowing());
            }

        }
        else
        {
            //El máximo y mínimo es 0 y 100
            barraFelicidad.value = Mathf.Clamp(barraFelicidad.value, 0, 99);
            //Se restara la barra de felicidad
            if (delayForDecreasing)
            {
                barraFelicidad.value--;
                delayForDecreasing = false;
                StartCoroutine(DelayForDecreasing());
            }
        }
        //Sirve para controlar el input del jugador
        if (scrollController.IsPressed())
        {
            //Se cambiará de posts
            if (delayForPressScroll)
            {
                delayForPressScroll = false;
                post.ChangeTipe();
                StartCoroutine(DelayForScrolling());
            }
        }
    }
    //Pilla la referencia del post
    public void GetThisPost(Posts postActual)
    {
        post = postActual;
    }
    //Son delays para que no se haga por cada frame las sumas y restas
    IEnumerator DelayOfGrowing()
    {
        yield return new WaitForSeconds(2);
        isPressingAvailable = true;
    }
    IEnumerator DelayForDecreasing()
    {
        yield return new WaitForSeconds(2);
        delayForDecreasing = true;
    }
    //Es un delay para que el jugador no haga scroll todo el tiempo mientras esté pulsado el stick derecho
    IEnumerator DelayForScrolling()
    {
        yield return new WaitForSeconds(1);
        delayForPressScroll = true;
    }
}

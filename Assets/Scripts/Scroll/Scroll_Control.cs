using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Scroll_Control : MonoBehaviour
{
    InputAction scrollController;
    [SerializeField] Slider barraFelicidad;
    bool isPressingAvailable = true;
    bool delayForDecreasing = true;
    bool delayForPressScroll = true;
    Posts post;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollController = InputSystem.actions.FindAction("PlayerController");
        barraFelicidad = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (post.posts == TipoPosts.Divertido)
        {
            barraFelicidad.value = Mathf.Clamp(barraFelicidad.value, 0, 99);
            if (isPressingAvailable)
            {
                barraFelicidad.value++;
                isPressingAvailable = false;
                StartCoroutine(DelayOfGrowing());
            }

        }
        else
        {
            barraFelicidad.value = Mathf.Clamp(barraFelicidad.value, 0, 99);
            if (delayForDecreasing)
            {
                barraFelicidad.value--;
                delayForDecreasing = false;
                StartCoroutine(DelayForDecreasing());
            }
        }
        if (scrollController.IsPressed())
        {
            if (delayForPressScroll)
            {
                delayForPressScroll = false;
                post.ChangeTipe();
                StartCoroutine(DelayForScrolling());
            }
           
        }
    }
    */
    
    public void GetThisPost(Posts postActual)
    {
        post = postActual;
    }
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
    IEnumerator DelayForScrolling()
    {
        yield return new WaitForSeconds(1);
        delayForPressScroll = true;
    }
}

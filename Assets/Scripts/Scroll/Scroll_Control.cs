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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollController = InputSystem.actions.FindAction("PlayerController");
        barraFelicidad = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollController.IsPressed())
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
            if (delayForDecreasing)
            {
                barraFelicidad.value--;
                delayForDecreasing = false;
                StartCoroutine(DelayForDecreasing());
            }
        }
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
}

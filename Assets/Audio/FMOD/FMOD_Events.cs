using UnityEngine;
using UnityEngine;
using FMODUnity;
public class FMOD_Events : MonoBehaviour
{
    ///////////      Menu      ///////////
    [field: Header("Menu")]
    [field: SerializeField] public EventReference SelectButton { get; private set; }
    [field: SerializeField] public EventReference ConfirmButton { get; private set; }
    [field: SerializeField] public EventReference BackInMenu { get; private set; }


    ///////////      Scoller      ///////////
    [field: Header("Scroller")]
    [field: SerializeField] public EventReference Scroll { get; private set; }
    [field: SerializeField] public EventReference LikeAPost { get; private set; }
    [field: SerializeField] public EventReference PostEndEffect { get; private set; }



    ///////////      Minigame 1      ///////////
    [field: Header("Minigame 1")]
    [field: SerializeField] public EventReference LadridoPerro { get; private set; }
    [field: SerializeField] public EventReference LanzarPelota { get; private set; }
    [field: SerializeField] public EventReference PerroRecogePelota { get; private set; }
    [field: SerializeField] public EventReference CogerPelotaDeVuelta { get; private set; }


    ///////////      MiniGame 2      ///////////
    [field: Header("Minigame 2")]
    [field: SerializeField] public EventReference PasarToalla { get; private set; }



    ///////////      Miscelanea      ///////////
    [field: Header("Miscelanea")]
    [field: SerializeField] public EventReference CompletarMinijuego { get; private set; }
    [field: SerializeField] public EventReference FallarMinijuego { get; private set; }
    [field: SerializeField] public EventReference MusicWhooshChange { get; private set; }

    ///////////      GameEnd      ///////////
    [field: Header("Game Ending")]
    [field: SerializeField] public EventReference OutOfBattery { get; private set; }
    [field: SerializeField] public EventReference FullOfAnxiety { get; private set; }

    ///////////      OST      ///////////
    [field: Header("OST")]
    [field: SerializeField] public EventReference MainMenuMusic { get; private set; }
    [field: SerializeField] public EventReference GameplayMusic { get; private set; }



    ///////////      CODE      ///////////
    public static FMOD_Events instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD_Event instance in the scene");
        }
        instance = this;
    }
}

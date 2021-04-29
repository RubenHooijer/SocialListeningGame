using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorMinigame : AbstractScreen<DoorMinigame>
{
    private InputManager inputManager;

    public bool NearDoor;

    private GameObject minigameBar;

    private FadeScript fadeScript;

    [SerializeField] private float fadeTime = 3;

    [SerializeField] private Transform newPosition;

    private void Start()
    {
        inputManager = InputManager.Instance;
        fadeScript = FadeScript.Instance;
        Initialize();
    }
    
    private void Initialize()
    {
        NearDoor = true;
        inputManager.InteractPerformed.AddListener(StartMinigame);
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    private void StartMinigame()
    {
        if (NearDoor)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            inputManager.InteractPerformed.RemoveListener(StartMinigame);
        }
    }

    public IEnumerator OpenDoor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        fadeScript.gameObject.SetActive(true);
        fadeScript.Fade(1, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        //Tp player
        PlayerMovementVS.Instance.transform.position = newPosition.position;

        BalanceMinigame.Instance.gameObject.SetActive(true);
        BalanceMinigame.Instance.InitializeMinigame();
        inputManager.DisableInput();
    }
}

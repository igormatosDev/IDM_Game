using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private NewControls newControls;
    private InputAction menu;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        newControls = new NewControls();
    }

    private void OnEnable()
    {
        try
        {
            menu = newControls.PauseMenu.Pause;
        }
        catch
        {
            Awake();
            menu = newControls.PauseMenu.Pause;
        }
        menu.Enable();

        menu.performed += Pause;
    }
    private void OnDisable()
    {
        menu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivatePauseMenu();
        }
        else
        {
            DeactivatePauseMenu();

        }
    }

    void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }


}

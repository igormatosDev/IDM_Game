using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private NewControls newControls;
    private InputAction menu;
    [SerializeField] private MouseCursorController mouseCursorController;
    [SerializeField] private GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Awake()
    {
        newControls = new NewControls();
    }

    private void OnEnable()
    {
        try
        {
            menu = newControls.Menus.Pause;
        }
        catch
        {
            Awake();
            menu = newControls.Menus.Pause;
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
        if (Time.timeScale != 0)
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
        mouseCursorController.SetCursor(mouseCursorController.swordCursorTexture);
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        // RESUME
        mouseCursorController.SetCursor(mouseCursorController.defaultCursorTexture);
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);

    }

    public void ResumeGame()
    {
        DeactivatePauseMenu();
        //Button btnResume = transform.Find("btnResume").GetComponent<Button>();

    }

    public void RestartGame()
    {
        // RESTART
        DeactivatePauseMenu();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        // QUIT
        // TO BUILD: COMMENT THIS UNITYEDITOR.EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPauseMenuEnabled = false;

    [SerializeField] private GameObject pauseMenu;

    void Awake()
    {
        IsPauseMenuEnabled = false;
    }

    private void OnEnable()
    {
        InputManager.Instance.PlayerInputActions.UI.PauseMenu.performed += EnableDisablePauseMenu;
    }

    private void OnDisable()
    {
        InputManager.Instance.PlayerInputActions.UI.PauseMenu.performed -= EnableDisablePauseMenu;
    }

    private void EnableDisablePauseMenu(InputAction.CallbackContext context)
    {
        if (DialogueManager.IsDialogueActivated) return;

        IsPauseMenuEnabled = !IsPauseMenuEnabled;

        pauseMenu.SetActive(!pauseMenu.activeSelf);

        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        IsPauseMenuEnabled = false;
        Time.timeScale = 1;
    }

    public void MainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}

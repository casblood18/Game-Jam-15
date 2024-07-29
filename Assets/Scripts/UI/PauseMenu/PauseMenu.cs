using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

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
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    [ContextMenu("Delete playerprefs")]
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

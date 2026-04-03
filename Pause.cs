using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManagerGamepad : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsPanel;
    public Button[] buttons;
    public RectTransform highlightImage;
    private PlayerInput playerInput;
    private MonoBehaviour[] playerScripts;

    private int currentIndex = 0;
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        foreach (Button btn in buttons)
            btn.onClick.AddListener(() => OnButtonClicked(btn));

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            playerScripts = player.GetComponents<MonoBehaviour>();
        }
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (!isPaused) return;

        
        if (controlsPanel != null && controlsPanel.activeSelf)
        {
            if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                controlsPanel.SetActive(false);
                foreach (Button b in buttons)
                    b.interactable = true;

                
                EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
            }
            return;
        }

        NavigateMenuWithDpad();
        ConfirmSelection();

        if (highlightImage != null && highlightImage.gameObject.activeSelf)
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
            if (selectedObj != null)
            {
                MoveHighlight(selectedObj);
            }
        }

        
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            currentIndex = 0;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);

            if (highlightImage != null)
                highlightImage.gameObject.SetActive(true);

            DisablePlayerControls();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (highlightImage != null)
                highlightImage.gameObject.SetActive(false);

            EnablePlayerControls();
        }
    }

    void NavigateMenuWithDpad()
    {
        
        if (Gamepad.current == null) return;

        if (Gamepad.current.dpad.up.wasPressedThisFrame || Keyboard.current?.upArrowKey.wasPressedThisFrame == true)
        {
            currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
        else if (Gamepad.current.dpad.down.wasPressedThisFrame || Keyboard.current?.downArrowKey.wasPressedThisFrame == true)
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }

    void ConfirmSelection()
    {
        if ((Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame))
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    private void MoveHighlight(GameObject selectedObj)
    {
        if (highlightImage != null)
        {
            highlightImage.gameObject.SetActive(true);
            RectTransform targetRect = selectedObj.GetComponent<RectTransform>();
            if (targetRect != null)
            {
                highlightImage.position = targetRect.position;
                highlightImage.sizeDelta = targetRect.sizeDelta + new Vector2(10, 10);
            }
        }
    }

    void OnButtonClicked(Button btn)
    {
        string btnName = btn.name.ToLower();

        if (btnName.Contains("resume"))
        {
            TogglePause();
        }
        else if (btnName.Contains("controls"))
        {
            if (controlsPanel != null)
            {
                controlsPanel.SetActive(true);
                foreach (Button b in buttons)
                    b.interactable = false;

                if (highlightImage != null)
                    highlightImage.gameObject.SetActive(false);
            }
        }
        else if (btnName.Contains("exit"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
    }

    private void DisablePlayerControls()
    {
        if (playerInput != null) playerInput.enabled = false;

        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                
                if (script != this)
                    script.enabled = false;
            }
        }
    }

    private void EnablePlayerControls()
    {
        if (playerInput != null) playerInput.enabled = true;

        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                script.enabled = true;
            }
        }
    }
}

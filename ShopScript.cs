using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject firstSelectedItem;
    public RectTransform highlightImage; 

    private bool playerInZone = false;
    private PlayerInput playerInput;
    private CharacterController characterController;
    private MonoBehaviour[] playerScripts;

    private void Start()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }

        if (highlightImage != null)
        {
            highlightImage.gameObject.SetActive(false); 
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            characterController = player.GetComponent<CharacterController>();
            playerScripts = player.GetComponents<MonoBehaviour>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        if (playerInZone && (Input.GetKeyDown(KeyCode.DownArrow) || Gamepad.current?.dpad.down.wasPressedThisFrame == true))
        {
            ToggleShop();
        }

    
        if (highlightImage != null && highlightImage.gameObject.activeSelf)
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
            if (selectedObj != null)
            {
                MoveHighlight(selectedObj);
            }
        }
    }

    private void ToggleShop()
    {
        bool isActive = shopUI.activeSelf;
        shopUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;

        if (!isActive)
        {
            DisablePlayerControls();
            SetFirstItemSelected();
        }
        else
        {
            EnablePlayerControls();

            
            if (highlightImage != null)
            {
                highlightImage.gameObject.SetActive(false);
            }
        }
    }

    private void SetFirstItemSelected()
    {
        if (firstSelectedItem != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedItem);

            MoveHighlight(firstSelectedItem);
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

    private void DisablePlayerControls()
    {
        if (playerInput != null) playerInput.enabled = false;
        if (characterController != null) characterController.enabled = false;

        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                if (script != this) script.enabled = false;
            }
        }
    }

    private void EnablePlayerControls()
    {
        if (playerInput != null) playerInput.enabled = true;
        if (characterController != null) characterController.enabled = true;

        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                script.enabled = true;
            }
        }
    }
}

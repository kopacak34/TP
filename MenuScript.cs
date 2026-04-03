using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuManagerGamepad : MonoBehaviour
{
    public Button[] buttons;
    public GameObject controlsPanel;
    
    public RectTransform highlightImage;
    private int currentIndex = 0;
    private float inputCooldown = 0.3f;
    private float nextInputTime = 0f;
    

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);

        foreach (Button btn in buttons)
            btn.onClick.AddListener(() => OnButtonClicked(btn));

        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }

    void Update()
    {
        NavigateMenu();
        ConfirmSelection();

        if (highlightImage != null && highlightImage.gameObject.activeSelf)
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
            if (selectedObj != null)
            {
                MoveHighlight(selectedObj);
            }
        }
    }

    void NavigateMenu()
    {
        float vertical = Input.GetAxis("Vertical");

        if (Time.time > nextInputTime)
        {
            if (vertical > 0.5f)
            {
                currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
                nextInputTime = Time.time + inputCooldown;
            }
            else if (vertical < -0.5f)
            {
                currentIndex = (currentIndex + 1) % buttons.Length;
                nextInputTime = Time.time + inputCooldown;
            }

            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }

    void ConfirmSelection()
    {
        if (Input.GetButtonDown("Submit"))
        {
            buttons[currentIndex].onClick.Invoke();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            controlsPanel.SetActive(false);
            foreach (Button b in buttons)
                b.interactable = true;

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

        if (btnName.Contains("play"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (btnName.Contains("exit"))
        {
            Debug.Log("Exiting game...");
            Application.Quit();
        }
        else if (btnName.Contains("controls"))
        {
            Debug.Log("CON");
            controlsPanel.SetActive(true);
            

            
            foreach (Button b in buttons)
                b.interactable = false;
        }

      
    }
}

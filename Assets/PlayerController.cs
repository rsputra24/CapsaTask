using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Button controlButton;
    public Button closeButton;
    public GameObject controlObjects;
    void Start()
    {
        controlButton.onClick.AddListener(delegate { OnControlButtonClicked(); });
        closeButton.onClick.AddListener(delegate { OnCloseButtonClicked(); });
    }

    void Update()
    {
        
    }

    public void OnControlButtonClicked()
    {
        controlObjects.SetActive(true);
    }

    public void OnCloseButtonClicked()
    {
        controlObjects.SetActive(false);
    }
}

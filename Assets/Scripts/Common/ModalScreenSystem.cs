using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ModalScreenSystem : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject modalScreenPrefab;

    private GameObject modalScreenInstance;

    private void Start()
    {
        if (!canvas)
        {
            Debug.LogError("Canvas not assigned to ModalScreenController");
            Destroy(this);
        }
    }

    public GameObject Show(string messageText, string buttonText, Action onClick)
    {
        modalScreenInstance = Instantiate(modalScreenPrefab, canvas.transform);

        var texts = modalScreenInstance.GetComponentsInChildren<Text>();

        texts.FirstOrDefault(t => t.name == "MessageText").text = messageText;

        texts.FirstOrDefault(t => t.name == "ButtonText").text = buttonText;

        modalScreenInstance.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            onClick();
            Destroy(modalScreenInstance);
        });

        return modalScreenInstance;
    }
}

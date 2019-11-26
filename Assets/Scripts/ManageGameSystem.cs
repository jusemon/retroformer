using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageGameSystem : MonoBehaviour
{
    public string emptyText = "-Empty-";

    public int playerNameCharacterNumber = 5;

    [SerializeField]
    private Text[] slotStateTexts;

    [SerializeField]
    private Text[] slotStateTexts2;

    [SerializeField]
    private Button[] slotButtons;

    [SerializeField]
    private Button[] newGameButtons;

    [SerializeField]
    private Button[] loadGameButtons;

    [SerializeField]
    private Button[] startButtons;

    [SerializeField]
    private Button[] loadButtons;

    [SerializeField]
    private GameObject loadGameScreen;

    [SerializeField]
    private GameObject newGameScreen;

    [SerializeField]
    private GameObject slotScreen;

    [SerializeField]
    private InputField playerNameInput;

    // Start is called before the first frame update
    void Start()
    {
        var slots = SaveLoadSystem.GetAllSlots();
        if (slotStateTexts.Length != slots.Length || slotStateTexts2.Length != slots.Length)
        {
            Debug.LogError("The number of slot state texts does not correspont to the number of slots in the game.");
            Destroy(this);
        }

        if (loadButtons.Length != slots.Length)
        {
            Debug.LogError("The number of load buttons does not correspont to the number of slots in the game.");
            Destroy(this);
        }

        if (startButtons.Length != slots.Length)
        {
            Debug.LogError("The number of load buttons does not correspont to the number of slots in the game.");
            Destroy(this);
        }

        for (int j = 0; j < slots.Length; j++)
        {
            var text = slots[j]?.playerName ?? emptyText;
            slotStateTexts[j].text = text;
            slotStateTexts2[j].text = text;
        }

        SetLoadGameButton();
        SetNewGameButton();
        LimitCharacterName();
    }

    private void LimitCharacterName()
    {
        playerNameInput.characterLimit = playerNameCharacterNumber;
    }

    public void SetLoadGameButton()
    {
        SetButtons(this.loadGameButtons, () =>
        {
            newGameScreen.SetActive(false);
            slotScreen.SetActive(false);
            loadGameScreen.SetActive(true);
        });
    }

    public void SetNewGameButton()
    {
        SetButtons(this.newGameButtons, () =>
        {
            loadGameScreen.SetActive(false);
            slotScreen.SetActive(false);
            newGameScreen.SetActive(true);
        });
    }

    public void SetSelectSlotButton()
    {
        SetButtons(this.slotButtons, () =>
        {
            loadGameScreen.SetActive(false);
            newGameScreen.SetActive(false);
            slotScreen.SetActive(true);
        });
    }

    public void SetStartButton()
    {
        SetButtons(this.startButtons, (position) =>
        {
            var slot = (SaveSlot)position;
            SaveLoadSystem.Save(slot, new SavingData
            {
                level = 1,
                playerName = playerNameInput.text,
                score = 0,
                timeElapsed = 0
            });
            SceneManager.LoadScene("Level1");
        });
    }

    public void SetLoadButton()
    {
        SetButtons(this.loadButtons, (position) =>
        {
            var slot = (SaveSlot)position;
            var data = SaveLoadSystem.Load(slot);
            SceneManager.LoadScene($"Level{data?.level.ToString()}");
        });
    }

    private void SetButtons(Button[] buttons, UnityAction action)
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    private void SetButtons(Button[] buttons, UnityAction<int> action)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                action(i);
            });
        }
    }
}

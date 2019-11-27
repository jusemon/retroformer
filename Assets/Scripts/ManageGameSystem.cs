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
        SetSelectSlotButton();
        SetStartButton();
        SetLoadButton();
        LimitCharacterName();
    }

    private void LimitCharacterName()
    {
        playerNameInput.characterLimit = playerNameCharacterNumber;
    }

    public void SetLoadGameButton()
    {
        SetButtons(loadGameButtons, () =>
        {
            newGameScreen.SetActive(false);
            slotScreen.SetActive(false);
            loadGameScreen.SetActive(true);
        });
    }

    public void SetNewGameButton()
    {
        SetButtons(newGameButtons, () =>
        {
            loadGameScreen.SetActive(false);
            slotScreen.SetActive(false);
            newGameScreen.SetActive(true);
        });
    }

    public void SetSelectSlotButton()
    {
        SetButtons(slotButtons, () =>
        {
            if (playerNameInput.text.Trim().Length == 0)
            {
                return;
            }
            loadGameScreen.SetActive(false);
            newGameScreen.SetActive(false);
            slotScreen.SetActive(true);
        });
    }

    public void SetStartButton()
    {
        SetButtons(startButtons, (position) =>
        {
            var slot = (SaveSlot)position;

            Debug.Log($"Selected Slot: {slot.ToString()}");
            SaveLoadSystem.Load(slot);
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
        SetButtons(loadButtons, (position) =>
        {
            var slot = (SaveSlot)position;

            Debug.Log($"Selected Slot: {slot.ToString()}");

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
            button.onClick.AddListener(ActionWithParam(action, i));
        }
    }

    private UnityAction ActionWithParam<T>(UnityAction<T> action, T param)
    {
        return () => action(param);
    }
}

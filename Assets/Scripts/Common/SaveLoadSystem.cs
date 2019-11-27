using System;
using UnityEngine;

public static class SaveLoadSystem
{
    public readonly static string SaveKey = "Save_";

    private static float internalTime = 0;

    public static void Save(SaveSlot slotKey, SavingData data)
    {
        PlayerPrefs.SetInt("Current", (int)slotKey);
        var timePassedSinceLastSave = Time.unscaledTime - internalTime;
        Debug.Log($"timePassedSinceLastSave: {timePassedSinceLastSave}");
        data.timeElapsed = timePassedSinceLastSave;
        internalTime = Time.unscaledTime;
        PlayerPrefs.SetString($"{SaveKey}{slotKey.ToString()}", JsonUtility.ToJson(data));
        Debug.Log($"Saved Data: {JsonUtility.ToJson(data, true)} {data.positionX}");
        PlayerPrefs.Save();
    }

    public static SavingData Load(SaveSlot slotKey)
    {
        internalTime = Time.unscaledTime;
        PlayerPrefs.SetInt("Current", (int)slotKey);
        PlayerPrefs.Save();
        if (HasSlot(slotKey))
        {
            Debug.Log(PlayerPrefs.GetString($"{SaveKey}{slotKey.ToString()}"));
            return JsonUtility.FromJson<SavingData>(PlayerPrefs.GetString($"{SaveKey}{slotKey.ToString()}"));
        }
        return null;
    }

    public static SavingData Get(SaveSlot slotKey)
    {
        return JsonUtility.FromJson<SavingData>(PlayerPrefs.GetString($"{SaveKey}{slotKey.ToString()}"));
    }

    public static bool HasSlot(SaveSlot slotKey)
    {
        return PlayerPrefs.HasKey($"{SaveKey}{slotKey.ToString()}");
    }

    public static void DeleteSlot(SaveSlot slotKey)
    {
        PlayerPrefs.DeleteKey($"{SaveKey}{slotKey.ToString()}");
    }

    public static void DeleteAllSlots()
    {
        foreach (var slotKey in (SaveSlot[])Enum.GetValues(typeof(SaveSlot)))
        {
            if (HasSlot(slotKey))
            {
                DeleteSlot(slotKey);
            }
        }
    }

    public static SavingData[] GetAllSlots()
    {
        var savesSlots = (SaveSlot[])Enum.GetValues(typeof(SaveSlot));

        var savingDatas = new SavingData[savesSlots.Length];

        for (int i = 0; i < savesSlots.Length; i++)
        {
            var slot = savesSlots[i];
            savingDatas[i] = HasSlot(slot) ? JsonUtility.FromJson<SavingData>(PlayerPrefs.GetString($"{SaveKey}{slot.ToString()}")) : null;
        }

        return savingDatas;
    }

    public static SaveSlot GetCurrentSlot()
    {
        return (SaveSlot)PlayerPrefs.GetInt("Current", 0);
    }
}

public class SavingData
{
    public int level;

    public float positionX;

    public float positionY;

    public int score;

    public float timeElapsed;

    public string playerName;

    public int timeLimit;
}

public enum SaveSlot
{
    Slot1,
    Slot2,
    Slot3
}
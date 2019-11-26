using System;
using UnityEngine;

public static class SaveLoadSystem
{
    public readonly static string SaveKey = "Save_";

    public static void Save(SaveSlot slotKey, SavingData data)
    {
        PlayerPrefs.SetInt("Current", (int)slotKey);
        PlayerPrefs.SetString($"{SaveKey}{slotKey.ToString()}", JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public static SavingData? Load(SaveSlot slotKey)
    {
        PlayerPrefs.SetInt("Current", (int)slotKey);
        PlayerPrefs.Save();
        return JsonUtility.FromJson<SavingData?>(PlayerPrefs.GetString($"{SaveKey}{slotKey.ToString()}"));
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
        foreach (var slot in (SaveSlot[])Enum.GetValues(typeof(SaveSlot)))
        {
            if (HasSlot(slot))
            {
                DeleteSlot(slot);
            }
        }
    }

    public static SavingData?[] GetAllSlots()
    {
        var savesSlots = (SaveSlot[])Enum.GetValues(typeof(SaveSlot));

        var savingDatas = new SavingData?[savesSlots.Length];

        for (int i = 0; i < savesSlots.Length; i++)
        {
            var slot = savesSlots[i];
            savingDatas[i] = HasSlot(slot) ? JsonUtility.FromJson<SavingData>(PlayerPrefs.GetString($"{SaveKey}{slot.ToString()}")) : (SavingData?) null;
        }

        return savingDatas;
    }

    public static SaveSlot GetCurrentSlot()
    {
        return (SaveSlot)PlayerPrefs.GetInt("Current", 0);
    }
}

public struct SavingData
{
    public int level;

    public float? positionX, positionY;

    public int score;

    public float timeElapsed;

    public string playerName;
}

public enum SaveSlot
{
    Slot1,
    Slot2,
    Slot3
}
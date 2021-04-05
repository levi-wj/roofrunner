using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataSaver
{
    public static int credits = 0;
    public static int hiscore = 0;
    public static bool seenTutorial = false;

    public static void SaveData ()
    {
        string path = Path.Combine(Application.persistentDataPath, "log.log");

        SaveData data = new SaveData(credits, hiscore, seenTutorial);

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            formatter.Serialize(stream, data);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadData ()
    {
        string path = Path.Combine(Application.persistentDataPath, "log.log");

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(stream);

            setVarsFromSaveData(data);

            stream.Close();
        } else {
            Debug.Log("Save file not found at " + path);
        }
    }

    static void setVarsFromSaveData(SaveData data)
    {
        credits = data.credits;
        hiscore = data.hiscore;
        seenTutorial = data.seenTutorial;
    }
}

[System.Serializable]
public class SaveData
{
    public int credits;
    public int hiscore;
    public bool seenTutorial;

    public SaveData(int credcount, int hiscoreValue, bool seenTutorialValue)
    {
        credits = credcount;
        hiscore = hiscoreValue;
        seenTutorial = seenTutorialValue;
    }
}
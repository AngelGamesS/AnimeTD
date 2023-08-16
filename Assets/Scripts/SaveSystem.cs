using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveIntoJson(float playerExp, int playerLevel)
    {
        string potion = JsonUtility.ToJson(new PlayerData(playerExp,playerLevel));
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", potion);
    }

    public PlayerData LoadFromJson()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/PlayerData.json");
            var data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }

        return null;
    }
}

[System.Serializable]
public class PlayerData
{
    public float playerExp;
    public int playerLevel;

    public PlayerData(float playerExp, int playerLevel)
    {
        this.playerExp = playerExp;
        this.playerLevel = playerLevel;
    }
}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerHealth playerHealth, PlayerMovement playerMovement, PlayerAttack playerAttack, PlayerUpgrades playerUpgrades)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/player.fun";

        using FileStream stream = new(path, FileMode.Create);
        PlayerData data = new(playerHealth, playerMovement, playerAttack, playerUpgrades);
        formatter.Serialize(stream, data);
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(path, FileMode.Open);
            return formatter.Deserialize(stream) as PlayerData;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}

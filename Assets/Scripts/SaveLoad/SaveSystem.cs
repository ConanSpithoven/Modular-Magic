using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Player
    public static void SavePlayer(PlayerStats player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.mgc";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.mgc";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    #region Seed
    public static void SaveSeed()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/seed.mgc";
        FileStream stream = new FileStream(path, FileMode.Create);

        SeedData data = new SeedData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadSeed()
    {
        string path = Application.persistentDataPath + "/seed.mgc";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SeedData data = formatter.Deserialize(stream) as SeedData;
            stream.Close();

            Random.InitState(data.seed);
            PlayerPrefs.SetInt("Seed", data.seed);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }
    #endregion
}

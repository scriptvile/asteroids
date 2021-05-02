using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Persistence
{
    // Const
    const string FILENAME = "bestResult.sav";

    // Privates
    static ResultData bestResult;

    // Properties
    public static ResultData BestResult { get { return bestResult; } }
    public static string FullPath { get { return Application.persistentDataPath + "/" + FILENAME; } }

    public static void LoadFromFile()
    {
        if (File.Exists(FullPath))
        {
            FileStream file = File.Open(FullPath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            bestResult = (ResultData)bf.Deserialize(file);
            file.Close();
        } else
        {
            CreateNewFile();
            LoadFromFile();
        }
    }

    static void SaveToFile()
    {
        if (bestResult == null)
        {
            CreateNewFile();
            return;
        }

        if (bestResult == null) bestResult = new ResultData(0, 0, 0);       // If there is no data loaded to memory - create a new one.
        FileStream file = File.Create(FullPath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, bestResult);
        file.Close();
    }

    static void CreateNewFile()
    {
        Debug.Log("CreateNewData()");
        if (File.Exists(FullPath))  File.Delete(FullPath);
        bestResult = new ResultData(0, 0, 0);
        
        SaveToFile();
    }

    public static void SaveNewResult(ResultData newResult)
    {
        bestResult = newResult;
        SaveToFile();
    }
}

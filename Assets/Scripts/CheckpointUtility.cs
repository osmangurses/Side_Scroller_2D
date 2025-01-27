using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public int order;
    public string checkpointID;
    public int playerHealth;
    public List<Skill> activeSkills;
}
public static class CheckpointUtility
{
    public static List<CheckpointData> LoadAllCheckpointData()
    {
        List<CheckpointData> checkpointDataList = new List<CheckpointData>();
        string directoryPath = Application.persistentDataPath;
        string[] files = Directory.GetFiles(directoryPath, "checkpoint_*.json");

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);

                if (data != null)
                {
                    checkpointDataList.Add(data);
                }
                else
                {
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Error loading checkpoint file: {file}. Exception: {ex.Message}");
            }
        }

        checkpointDataList.Sort((a, b) => a.order.CompareTo(b.order));

        return checkpointDataList;
    }

    public static Vector3 ConvertIDToVector3(string checkpointID)
    {
        string[] splitID = checkpointID.Split('_');

        if (splitID.Length != 3)
        {
            return Vector3.zero;
        }

        if (float.TryParse(splitID[0], out float x) &&
            float.TryParse(splitID[1], out float y) &&
            float.TryParse(splitID[2], out float z))
        {
            return new Vector3(x, y, z);
        }
        else
        {
            return Vector3.zero;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointPanel : MonoBehaviour
{
    public static CheckpointPanel instance;
    [SerializeField] GameObject select_checkpoint_button;
    private void Awake()
    {
        instance = this;
    }
    public void OpenPanel()
    {
        CreateSelectButtons();
        transform.localScale = Vector3.one;
        GameManager.instance.FreezePlayer();
    }
    public void ClosePanel()
    {
        transform.localScale = Vector3.zero;
    }
    void CreateSelectButtons()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Destroy(
            transform.GetChild(0).GetChild(i).gameObject);
        }
        List<CheckpointData> checkpoints = CheckpointUtility.LoadAllCheckpointData();
        foreach (var checkpoint in checkpoints)
        {
            var checkpointButtonTemp = Instantiate(select_checkpoint_button, transform.GetChild(0));
            checkpointButtonTemp.GetComponentInChildren<Text>().text = $"HEALTH: {checkpoint.playerHealth}";
            checkpointButtonTemp.GetComponent<SelectCheckpointButton>().checkpointID = checkpoint.checkpointID;
        }
    }
}

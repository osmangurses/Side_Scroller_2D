using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCheckpointButton : MonoBehaviour
{
    [HideInInspector]public string checkpointID;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SelectThis);
    }
    void SelectThis()
    {
        GameManager.instance.GoToCheckpoint(checkpointID);
        CheckpointPanel.instance.ClosePanel();

    }
}

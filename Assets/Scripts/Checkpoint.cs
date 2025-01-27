using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Checkpoint : MonoBehaviour
{
    public int order;

    [HideInInspector] public bool isCollected;
    [HideInInspector] public Animator animator;

    private string checkpointID;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {

        checkpointID = GameManager.instance.GenerateCheckpointID(transform);
        CheckpointData loadedData = GameManager.instance.LoadCheckpointData(checkpointID);

        if (loadedData != null)
        {
            animator.Play("CollectedIdle");
        }
        else
        {
            isCollected = false;
            animator.Play("Idle");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }
    public void Collect()
    {
        animator.Play("Collect");
        isCollected = true;
        PlayerPrefs.SetString("LastCheckpoint", checkpointID);
        GameManager.instance.SaveCheckpointData(checkpointID, order);
    }
}

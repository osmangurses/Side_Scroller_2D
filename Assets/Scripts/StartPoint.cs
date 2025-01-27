using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Animator animator;
    private string checkpointID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void Start()
    {

        checkpointID = GameManager.instance.GenerateCheckpointID(transform);
        CheckpointData loadedData = GameManager.instance.LoadCheckpointData(checkpointID);

        if (loadedData == null)
        {
            animator.Play("CollectedState");
            GameManager.instance.SaveCheckpointData(checkpointID,0);
        }
        else
        {
            animator.Play("Idle");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Start") && collision.gameObject.CompareTag("Player"))
        {
            animator.Play("Start");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public CheckpointData last_check_point;
    [HideInInspector] public GameObject player;
    [SerializeField] Transform start_point;
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("LastCheckpoint"))
        {
            SetLastCheckpoint(PlayerPrefs.GetString("LastCheckpoint"));
        }
        else
        {
            last_check_point.activeSkills = new List<Skill>();
            last_check_point.checkpointID = $"{start_point.position.x}_{start_point.position.y}_{start_point.position.z}";
            last_check_point.playerHealth = HealthManager.instance.max_health;
        }
        SetChekcpointDataToPlayer();
    }
    public void SetLastCheckpoint(string checkpointID)
    {
        List<CheckpointData> collected_check_points = CheckpointUtility.LoadAllCheckpointData();
        foreach (var checkpoint in collected_check_points)
        {
            if (checkpoint.checkpointID == checkpointID)
            {
                last_check_point = checkpoint;
                PlayerPrefs.SetString("LastCheckpoint", checkpointID);
                return;
            }
        }
        last_check_point.activeSkills = new List<Skill>();
        last_check_point.checkpointID = $"{start_point.position.x}_{start_point.position.y}_{start_point.position.z}";
        last_check_point.playerHealth = HealthManager.instance.max_health;
    }

    public void GoToCheckpoint(string checkpointID)
    {
        SetLastCheckpoint(checkpointID);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SetChekcpointDataToPlayer()
    {
        HealthManager.instance.health = last_check_point.playerHealth;
        HealthManager.instance.UpdateUI();
        SkillManager.instance.SelectSkill(Skill.None);
        SkillManager.instance.active_skills = last_check_point.activeSkills;
        SkillManager.instance.UpdateUI();
        player.transform.position = CheckpointUtility.ConvertIDToVector3(last_check_point.checkpointID);

        ActivatePlayer();
        var animator = player.GetComponent<Animator>();
        animator.Play("Idle");
    }
    public void TakeDamage()
    {
        ActivatePlayer();
        var animator = player.GetComponent<Animator>(); SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0.3f;
        spriteRenderer.color = color;

        animator.Play("Idle");
    }
    public void FreezePlayer()
    {
        player.GetComponent<CapsuleCollider2D>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.GetComponent<CharacterAnimationManager>().enabled = false;
        player.GetComponent<CharacterMovement>().enabled = false;
        player.GetComponent<CharacterGroundCheck>().enabled = false;
    }
    public void ActivatePlayer()
    {
        player.GetComponent<CapsuleCollider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<CharacterAnimationManager>().enabled = true;
        player.GetComponent<CharacterMovement>().enabled = true;
        player.GetComponent<CharacterGroundCheck>().enabled = true;
    }
    public CheckpointData LoadCheckpointData(string checkpointID)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"checkpoint_{checkpointID}.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);
            Debug.Log($"Checkpoint data loaded from {filePath}");
            return data;
        }
        else
        {
            Debug.LogWarning($"Checkpoint file not found at {filePath}");
            return null;
        }
    }
    public void SaveCheckpointData(string checkpointID, int order)
    {
        CheckpointData data = new CheckpointData
        {
            order = order,
            checkpointID = checkpointID,
            playerHealth = HealthManager.instance.health,
            activeSkills = new List<Skill>(SkillManager.instance.active_skills)
        };

        last_check_point = data;
        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, $"checkpoint_{checkpointID}.json");
        File.WriteAllText(filePath, json);

        Debug.Log($"Checkpoint data saved to {filePath}");
    }

    public string GenerateCheckpointID(Transform cp_transform)
    {
        return $"{cp_transform.position.x}_{cp_transform.position.y}_{cp_transform.position.z}";
    }
}

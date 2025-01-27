using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public bool onShield;

    [HideInInspector] public int health=3;
    [HideInInspector] public int max_health=3;

    [SerializeField] Image[] heart_images;
    [SerializeField] Sprite active_heart_sprite;
    [SerializeField] Sprite deactive_heart_sprite;
    [SerializeField] float shield_time;

    GameObject player;
    float shield_timer;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = GameManager.instance.player;
    }
    private void Update()
    {
        if (onShield)
        {
            shield_timer -= Time.deltaTime;
            if (shield_timer<=0)
            {
                onShield = false; 
                SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;

            }
        }
    }
    public void IncreaseHealth()
    {
        var animator = player.GetComponent<Animator>();
        GameManager.instance.FreezePlayer();
        health--;
        UpdateUI();
        GameManager.instance.FreezePlayer();
        if (health==0)
        {
            animator.Play("Death");
            var delay = GetAnimationRemainingTime(animator, "Death");
            GameManager.instance.Invoke(nameof(GameManager.instance.GoToCheckpoint), delay);
        }
        else
        {
            animator.Play("Damage");
            onShield = true;
            shield_timer = shield_time;
            var delay = GetAnimationRemainingTime(animator, "Damage");
            GameManager.instance.Invoke(nameof(GameManager.instance.TakeDamage), delay);
        }
    }
    public void DecreaseHealth()
    {
        if (health!=max_health)
        {
            health++;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (var heart in heart_images)
        {
            heart.sprite = deactive_heart_sprite;
        }
        for (int i = 0; i < health; i++)
        {
            heart_images[i].sprite = active_heart_sprite;
        }
    }
    float GetAnimationRemainingTime(Animator animator, string animationClipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationClipName)
            {
                return clip.length;
            }
        }

        return 0;
    }
}

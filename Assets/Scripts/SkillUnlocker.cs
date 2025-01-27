using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlocker : MonoBehaviour
{
    [SerializeField] Skill skill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SkillManager.instance.UnlockSkill(skill);
            Destroy(gameObject);
        }
    }
}

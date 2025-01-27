using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SkillManager : MonoBehaviour
{
    public Button speed_up, double_jump, wall_jump, mini_player, selected;
    public static SkillManager instance;
    public Skill selected_skill=Skill.None;
    public List<Skill> active_skills;
    private void Awake()
    {
        instance = this;
        speed_up.onClick.AddListener(() => SelectSkill(Skill.SpeedUp));
        double_jump.onClick.AddListener(() => SelectSkill(Skill.DoubleJump));
        wall_jump.onClick.AddListener(() => SelectSkill(Skill.WallJump));
        mini_player.onClick.AddListener(() => SelectSkill(Skill.MiniPlayer));

        speed_up.onClick.AddListener(() => OpenClosePanel());
        double_jump.onClick.AddListener(() => OpenClosePanel());
        wall_jump.onClick.AddListener(() => OpenClosePanel());
        mini_player.onClick.AddListener(() => OpenClosePanel());
        selected.onClick.AddListener(() => OpenClosePanel());
        UpdateUI();
    }
    public void UpdateUI()
    {
        Button[] skillButtons = { speed_up, double_jump, wall_jump, mini_player, selected };
        foreach(var btn in skillButtons)
        {
            btn.transform.Find("Text (Legacy)")?.GetComponent<Text>().gameObject.SetActive(true);
            btn.transform.Find("Image")?.GetComponent<Image>().gameObject.SetActive(false);
        }
        foreach(Skill skill in active_skills)
        {
            Button btn;
            if (skill==Skill.SpeedUp)
            {
                btn = speed_up;
            }
            else if (skill == Skill.WallJump)
            {
                btn = wall_jump;
            }
            else if (skill == Skill.DoubleJump)
            {
                btn = double_jump;
            }
            else 
            {
                btn = mini_player;
            }
            btn.transform.Find("Text (Legacy)")?.GetComponent<Text>().gameObject.SetActive(false);
            btn.transform.Find("Image")?.GetComponent<Image>().gameObject.SetActive(true);
            if (selected_skill == skill)
            {
                selected.transform.Find("Text (Legacy)")?.GetComponent<Text>().gameObject.SetActive(false);
                Image img = selected.transform.Find("Image")?.GetComponent<Image>();
                img.sprite = btn.transform.Find("Image")?.GetComponent<Image>().sprite;
                selected.transform.Find("Image")?.GetComponent<Image>().gameObject.SetActive(true);
            }
        }
    }
    public void UnlockSkill(Skill skill)
    {
        if (!active_skills.Contains(skill))
        {
            active_skills.Add(skill);
        }
        if (selected_skill==Skill.None)
        {
            selected_skill = skill;
        }
        UpdateUI();
    }
    public void LockSkill(Skill skill)
    {
        if (active_skills.Contains(skill))
        {
            active_skills.Remove(skill);
            UpdateUI();
        }
    }
    public void SelectSkill(Skill skill)
    {
        if (active_skills.Contains(skill))
        {
            selected_skill = skill;
            UpdateUI();
        }
        else if (skill==Skill.None)
        {
            selected_skill = Skill.None;
            selected.transform.Find("Text (Legacy)")?.GetComponent<Text>().gameObject.SetActive(true);
            selected.transform.Find("Image")?.GetComponent<Image>().gameObject.SetActive(false);
        }
    }
    public void OpenClosePanel()
    {
        Button[] skillButtons = { speed_up, double_jump, wall_jump, mini_player};
        if (speed_up.gameObject.activeSelf)
        {
            foreach (var btn in skillButtons)
            {
                btn.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var btn in skillButtons)
            {
                btn.gameObject.SetActive(true);
            }
        }
    }
}

public enum Skill
{
    None,
    SpeedUp,
    WallJump,
    DoubleJump,
    MiniPlayer
}
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using System;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [HideInInspector]
    public static AchievementManager instance;
    public List<Achievement> achievements;

    private string infoFilePath;
    public GameObject achievementBarProfab;
    private GameObject Canvas;

    private void Awake()
    {
        infoFilePath = "Assets/Info/Achievement/test.json";
        Canvas = GameObject.Find("Canvas");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadAchievementsInfo();
    }

    private void LoadAchievementsInfo()
    {
        if(File.Exists(infoFilePath))
        {
            string json = File.ReadAllText(infoFilePath);
            AchievementList achievementList = JsonUtility.FromJson<AchievementList>(json);
            achievements = achievementList.achievements;
        }
        else
        {
            print("file not exist");
        }
    }
 
    public void UnlockAchievement(string achievementId)
    {
        Achievement achievement = achievements.Find(a => a.id == achievementId);
        if (achievement != null)
        {
            achievement.isCompleted = true;
            Debug.Log($"Achievement unlocked: {achievement.name}");
            // 在这里可以添加其他的成就解锁逻辑，比如弹窗提示、特效等
            ShowAchievementBar(achievement);
        }
        UpdateAchievementState();
    }

    private void ShowAchievementBar(Achievement achievement)
    {
        GameObject gameObject = Instantiate(achievementBarProfab, Vector3.zero, Quaternion.identity);
        gameObject.transform.SetParent(Canvas.transform,false);
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        gameObject.GetComponentInChildren<Text>().text = achievement.name;
        StartCoroutine(DestroyAchievementBar(gameObject));
    }

    IEnumerator DestroyAchievementBar(GameObject gameObject)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    public bool IsAchievementCompleted(string achievementId)
    {
        Achievement achievement = achievements.Find(a => a.id == achievementId);
        return achievement != null && achievement.isCompleted;
    }

    public void UpdateAchievementState()
    {
        AchievementList achievementList = new AchievementList(achievements);
        string json = JsonUtility.ToJson(achievementList,true);
        File.WriteAllText(infoFilePath, json);
        print("update achievement state");
    }

    [System.Serializable]
    public class AchievementList
    {
        public List<Achievement> achievements;

        public AchievementList(List<Achievement> achievements)
        {
            this.achievements = achievements;
        }
    }
}

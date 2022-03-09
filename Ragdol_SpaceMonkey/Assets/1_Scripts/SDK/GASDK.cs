
using UnityEngine;
//using GameAnalyticsSDK;

public class GASDK : MonoBehaviour
{
    public static bool start = false;
    /*
    void Start()
    {
        if (!start)
        {
            GameAnalytics.Initialize();
            start = true;
        }
        LevelController.levelStartEvent.AddListener(StartLevel);
        LevelController.levelWinEvent.AddListener(EndLevel);
        LevelController.levelFailEvent.AddListener(FailLevel);
    }

    public static void StartLevel(int levelNum, string sceneName)
    {
       
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level " + levelNum, "scene " + sceneName);
        //Debug.Log("Стартовал уровнеь " + PlayerPrefs.GetInt("level"));
    }

    public static void EndLevel(int levelNum, string sceneName)
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level " + levelNum, "scene " + sceneName);
        //Debug.Log("Пройден уровнеь " + PlayerPrefs.GetInt("level"));
    }

    public static void FailLevel(int levelNum, string sceneName)
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level " + levelNum, "scene " + sceneName);
        //Debug.Log("Пройден уровнеь " + PlayerPrefs.GetInt("level"));
    }
    */
}

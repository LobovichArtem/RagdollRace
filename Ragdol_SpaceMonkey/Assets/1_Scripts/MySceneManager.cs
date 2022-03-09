using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    private AsyncOperation nextScene;
    public Text currentLevelText;
    public Text currentMoneyText;
    public static int currentLevelNum;
    public static string currentSceneName;



    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadMenu();
            return;
        }
        int currentLvl = PlayerPrefs.GetInt("level", 1);
        currentLevelNum = currentLvl;
        currentLevelText.text = "LEVEL "+ currentLvl.ToString();
        
        if (SceneManager.GetActiveScene().name == "menu")
        {            
            currentMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();
            if (currentLvl > SceneManager.sceneCountInBuildSettings-2)
            {
                //Debug.Log(" " + currentLvl  + " " + (currentLvl % (SceneManager.sceneCountInBuildSettings-2)));
                currentLvl = (SceneManager.sceneCountInBuildSettings - 6) + ((currentLvl % (SceneManager.sceneCountInBuildSettings - 2)) % 5);
                
                //currentLvl = Mathf.Clamp(currentLvl, 1, SceneManager.sceneCountInBuildSettings-1);
            }
            LoadNextLevelAsync("level" + currentLvl.ToString());
            
        }
        currentSceneName = "level" + currentLvl.ToString();
        Debug.Log(currentSceneName);
    }

    public void LoadSceneNext ()
    {
        nextScene.allowSceneActivation = true;
    }

    public void LoadNextLevelAsync (string currentLvl)
    {        
        StartCoroutine(LoadYourAsyncScene(currentLvl));
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    IEnumerator LoadYourAsyncScene(string scene)
    {
        nextScene = SceneManager.LoadSceneAsync(scene);
        nextScene.allowSceneActivation = false;

        while (!nextScene.isDone)
        {
            yield return null;
        }
    }

    public static void AddNextLevel ()
    {
        int l = PlayerPrefs.GetInt("level", 1);
        l++;
        PlayerPrefs.SetInt("level", l);
    }

    public static void AddMoney (int money)
    {
        int m = PlayerPrefs.GetInt("money", 0);
        m += money;
        PlayerPrefs.SetInt("money", m);
    }

}

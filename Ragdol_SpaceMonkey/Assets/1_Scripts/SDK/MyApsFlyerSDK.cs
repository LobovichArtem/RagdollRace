using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AppsFlyerSDK; 

public class MyApsFlyerSDK : MonoBehaviour
{
    private void Start()
    {

        LevelController.levelStartEvent.AddListener(StartLevel);
    }

    public void StartLevel (int levelNum, string sceneName) 
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        //eventParameters0.Add(AFInAppEvents.LEVEL, sceneName); //
        
       //AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, eventParameters0);
    }
}

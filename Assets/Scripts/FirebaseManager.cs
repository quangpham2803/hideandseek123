using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static void LogEventType(string eventKey, string value)
    {
        FirebaseAnalytics.LogEvent(eventKey, new Parameter("type", value));
    }

    public static void LogEventTutorial(string eventkey, string value)
    {
        FirebaseAnalytics.LogEvent(GameSetting.TUTORIAL, new Parameter(GameSetting.PLAYFAB_ID, StatusPlayer.Instance.MyPlayfabID), new Parameter(eventkey, value));
    }

    public static void LogEventInfomationUser(string eventkey, int value)
    {
        FirebaseAnalytics.LogEvent(GameSetting.INFORMATION_USER, new Parameter(GameSetting.PLAYFAB_ID, StatusPlayer.Instance.MyPlayfabID), new Parameter(eventkey, value));
    }

    public static void LogEventTimeUserPlay(string eventkey, string value)
    {
        FirebaseAnalytics.LogEvent(GameSetting.TIME_USER_PLAY, new Parameter(GameSetting.PLAYFAB_ID, StatusPlayer.Instance.MyPlayfabID), new Parameter(eventkey, value));
    }
}

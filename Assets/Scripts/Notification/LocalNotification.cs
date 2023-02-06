using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using Unity.Notifications.Android;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
    
#endif

public class LocalNotification : MonoBehaviour
{
#if UNITY_ANDROID
    public AndroidNotificationChannel defaultNotificationChannel;
#elif UNITY_IOS
    using Unity.Notifications.iOS;
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }
#endif
    public void ShowNotification(string title, string text, string subtitleIOS, DateTime dateTimeAndroid, TimeSpan timeSpanIOS)
    {
#if UNITY_ANDROID
        AndroidNotification notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            SmallIcon = "icon_0",
            LargeIcon = "icon_1",
            FireTime = dateTimeAndroid,
        };
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
#elif UNITY_IOS
        iOSNotificationTimeIntervalTrigger timetrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = timeSpanIOS,
            Repeats = false,
        };
        var notificationIOS = new iOSNotification()
        {
            Identifier = "test_notification",
            Title = title,
            Body = text,
            Subtitle = subtitleTOS,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timetrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notificationIOS);

#endif
    }
    void Start()
    {
#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
#elif UNITY_IOS

#endif

    }
    public bool isPaused;
    int id;
    int GetTimeAt(int hour)
    {
        if (DateTime.Now.Hour > hour)
        {
            return (24 - DateTime.Now.Hour + hour)*3600;
        }
        else
        {
            return (0 - DateTime.Now.Hour + hour)*3600;
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        if (isPaused)
        {
            int timeNotification = ((100 - (int)InventoryManager.inventory.energy) * 30) + 5*60;
            if (DateTime.Now.AddSeconds(timeNotification).Hour >= 23 && DateTime.Now.AddSeconds(timeNotification).Hour <= 6)
            {
                timeNotification = GetTimeAt(7);
                ShowNotification("Your energy has been filled full", "Hey Friend, can you want to spend all energy with your friend in Hide N Seek?", "Your energy has been filled full", DateTime.Now.AddSeconds(timeNotification), new TimeSpan(0,0, timeNotification));
            }
            else
            {
                ShowNotification("Your energy has been filled full", "Hey Friend, can you want to spend all energy with your friend in Hide N Seek?", "Your energy has been filled full", DateTime.Now.AddSeconds(timeNotification), new TimeSpan(0, 0, timeNotification));
            }

            int timeOffNotification = GetTimeAt(7);
            for (int i= 0; i < 30; i++)
            {
                ShowNotification("Your Daily Reward has been reset", " Now Login and win 1000 Gold today", " Now Login and win 1000 Gold today", DateTime.Now.AddSeconds(timeOffNotification +i*24*3600), new TimeSpan(0, 0, timeOffNotification + i * 24 *3600));
            }
        }
        else
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
    }


}

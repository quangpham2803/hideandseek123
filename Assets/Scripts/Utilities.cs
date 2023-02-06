using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;
//using Newtonsoft.Json;

public class Utilities : MonoBehaviour
{

	public static T[] ConcatArrays<T>(params T[][] list)
	{
		var result = new T[list.Sum(a => a.Length)];
		int offset = 0;
		for (int x = 0; x < list.Length; x++)
		{
			list[x].CopyTo(result, offset);
			offset += list[x].Length;
		}
		return result;
	}


	public static void ExitGame()
	{
		//#if !UNITY_EDITOR && UNITY_ANDROID
		//        System.Diagnostics.Process.GetCurrentProcess().Kill();
		//#else
		Application.Quit();
		//#endif
	}

	public static void ActivePermission()
	{
#if UNITY_IOS
        Application.OpenURL("app-settings:");
#else
		Application.RequestUserAuthorization(UserAuthorization.Microphone);
#endif

	}


	public static string GetLocalizeParameter(string text, string text1, string text2 = "", string text3 = "", string text4 = "")
	{
		text = text.Replace("{1}", text1);

		if (!string.IsNullOrEmpty(text2))
		{
			text = text.Replace("{2}", text2);
		}

		if (!string.IsNullOrEmpty(text3))
		{
			text = text.Replace("{3}", text3);
		}

		if (!string.IsNullOrEmpty(text4))
		{
			text = text.Replace("{4}", text4);
		}

		return text;
	}

	public static List<T> GetEnumList<T>()
	{
		T[] array = (T[])Enum.GetValues(typeof(T));
		List<T> list = new List<T>(array);
		return list;
	}

	public static DateTime GetDateTimeFromTimeStamp(long time) {
		return DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime;
	}

	//public static string GetTimesFormat(long second)
	//{
	//	TimeSpan timeSpan = TimeSpan.FromSeconds(second);
	//	string timeText = "";

	//	string days = GetLocalize("DAYS");
	//	string hours = GetLocalize("HOURS");
	//	string minutes = GetLocalize("MINUTES");
	//	string seconds = GetLocalize("SECONDS");

	//	if (timeSpan.Days >= 1)
	//	{
	//		timeText = timeSpan.Days.ToString("F0") + " " + days + " " + timeSpan.Hours.ToString("F0") + " " + hours;
	//	}
	//	else if (timeSpan.Hours >= 1)
	//	{
	//		timeText = timeSpan.Hours.ToString("F0") + " " + hours + " " + timeSpan.Minutes.ToString("F0") + " " + minutes + " ";
	//	}
	//	else if (timeSpan.Minutes >= 1)
	//	{
	//		timeText = timeSpan.Minutes.ToString("F0") + " " + minutes + " " + timeSpan.Seconds.ToString("F0") + " " + seconds + " ";
	//	}
	//	else
	//	{
	//		timeText = timeSpan.Seconds.ToString("F0") + " " + seconds + " ";
	//	}

	//	return timeText;
	//}

	//public static string GetHours(long second)
	//{
	//	TimeSpan timeSpan = TimeSpan.FromSeconds(second);
	//	string hours = GetLocalize("HOURS");
	//	string minutes = GetLocalize("MINUTES");
	//	string seconds = GetLocalize("SECONDS");

	//	hours = timeSpan.TotalHours.ToString("F0") + " " + hours;

	//	return hours;
	//}

	public static DateTime GetDateTimeFromUnixTimeStamp(uint timestamp)
	{
		//new DateTime()
		return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp);
	}

	public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
		// Unix timestamp is seconds past epoch
		DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
		return dtDateTime;
	}

	public static float CalculateLineHeight(Text textUI, string textContent, float cellWidth, float minCellHeight)
	{
		Vector2 extents = new Vector2(cellWidth, minCellHeight);
		var lineHeight =
			textUI.cachedTextGeneratorForLayout.GetPreferredHeight(textContent, textUI.GetGenerationSettings(extents));

		if (lineHeight < minCellHeight)
		{
			lineHeight = minCellHeight;
		}

		// Debug.Log(textUI.rectTransform.sizeDelta.x + " --> " + lineHeight);

		return lineHeight;
	}

	public static string LongToTime(long milisecsond)
	{
		string timeText = "";
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(milisecsond);
		timeText = timeSpan.Hours + "h " + timeSpan.Minutes + "m";
		return timeText;
	}

	public static DateTime TimeStampToDateTime(long unixTimeStamp)
	{
		// Unix timestamp is seconds past epoch
		System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
		return dtDateTime;
	}

	public static int GetWeekOfYear(DateTime time)
	{
		var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
		if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
		{
			time = time.AddDays(3);
		}

		return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
	}

	public static DateTime GetLastDayOfMonth(DateTime time)
    {
		DateTime endOfMonth = new DateTime(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month));
		return endOfMonth;
	}

	public static DateTime GetLastDayOfWeek(DateTime time)
    {
		int dayofWeek = time.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)time.DayOfWeek;
		var lastDayOfWeek = time.AddDays(7 - dayofWeek).Date;
		return lastDayOfWeek;
	}

	public static string SubString(string text, int maxChar = 12)
	{
		//Debug.LogError("text: " + text);
		maxChar = 60;

		string newString = "";
        if (!string.IsNullOrEmpty(text))
        {

            if (text.Length <= maxChar) return text;

            newString = text.Substring(0, maxChar);
            newString = newString + "...";
        }
        return newString;
	}

	public static string CleanWord(string input)
	{
		for (int i = 0; i < bad_words.Length; i++)
		{

			//  Replace the word with *'s (but keep it the same length)
			string str_replace = "";
			for (int j = 0; j < bad_words[i].Length; j++)
			{
				if (j == 0)
				{
					str_replace += bad_words[i][0];
				}
				else
				{
					str_replace += "*";
				}
			}
			input = Regex.Replace(input, bad_words[i], str_replace, RegexOptions.IgnoreCase);
		}
		return input;
	}

	public static string ReturnPathDevice(string str)
	{
		if (str.Contains("file://"))
		{
			return str;
		}

		return "file://" + str;
	}

	//public static IEnumerator LoadTexture2DFromContactData(ContactData data, Action<ContactData, Texture2D, Action> callback = null, Action handleDownloadComplete = null)
	//{
	//    //Debug.Log("avatar url: " + data.avatarURL);
	//    if(data.avatarURL == null)
	//        data.avatarURL = "http://img6.downloadapk.net/6/32/c7bb7b_0.png";
	//    WWW www = new WWW(data.avatarURL);
	//    while (!www.isDone)
	//    {
	//        //Debug.Log("Download image on progress" + www.progress);
	//        yield return null;
	//    }

	//    if (!string.IsNullOrEmpty(www.error))
	//    {
	//        //Debug.Log("Download failed");
	//        callback(data,null, null);
	//    }
	//    else
	//    {
	//        //Debug.Log("Download succes");
	//        Texture2D texture = new Texture2D(1, 1);
	//        www.LoadImageIntoTexture(texture);
	//        callback(data,texture, handleDownloadComplete);
	//    }
	//}

	public static GameObject LoadSkinItem(string path, Transform parentTransform)
	{
		var obj = Resources.Load<GameObject>(path);
		if (obj != null)
		{
			GameObject instance = Instantiate(obj, parentTransform);
			return instance;
		}
		Debug.LogError(path);
		return null;
	}


	public static GameObject LoadGameObject(string path, Transform parentTransform, System.Action callback = null, bool isAnimationScale = true)
	{
		GameObject instance = (GameObject)Instantiate(Resources.Load<GameObject>(path));
		instance.transform.SetParent(parentTransform);
		instance.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		instance.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		instance.transform.localScale = Vector3.one;

		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.3f).OnComplete(() =>
			{
				callback?.Invoke();
			});

			if (isAnimationScale)
			{
				canvasGroup.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				canvasGroup.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
			}
		}

		return instance;
	}

	public static void UnloadGameObject(GameObject instance, Action callback = null, float seconds = 0.2f, float timeDelay = 0)
	{
		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.DOFade(0, seconds).OnComplete(() =>
			{
				callback?.Invoke();
				Destroy(instance);
				Resources.UnloadUnusedAssets();
			}).SetDelay(timeDelay);
		}
	}

	public static void FadeInPanel(GameObject instance, float seconds = 0.2f, Action callback = null)
	{
		instance.SetActive(true);
		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
		canvasGroup.DOFade(1, seconds).OnComplete(() =>
		{
			callback?.Invoke();
		});
	}

	public static void FadeOutPanel(GameObject instance, float seconds = 0.2f, float delay = 0, Action callback = null)
	{
		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 1;
		canvasGroup.DOFade(0, seconds).OnComplete(() =>
		{
			callback?.Invoke();
			instance.SetActive(false);
		}).SetDelay(delay);
	}

	public static void FxButtonPress(Transform btn, bool isSoundTap = false)
	{
		RectTransform rt = btn.GetComponent<RectTransform>();
		rt.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f).OnComplete(() =>
		{
			rt.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.08f).OnComplete(() =>
			{
				rt.DOScale(new Vector3(1, 1, 1), 0.05f);
			});
		});

		//if (isSoundTap)
		//{
		//	SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.btn_tap);
		//}
	}

	public static void FxButtonPressSmall(Transform btn, bool isSoundTap = false)
	{
		RectTransform rt = btn.GetComponent<RectTransform>();
		rt.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).OnComplete(() =>
		{
			rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.08f).OnComplete(() =>
			{
				rt.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.05f);
			});
		});

		if (isSoundTap)
		{
			//SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.btn_tap);
		}
	}

	public static void CheckClickedOutSide(GameObject panel, Action callbackOutSide = null, Action callbackInSide = null, Camera _camera = null)
	{
		if (!panel.activeSelf)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(), Input.mousePosition, _camera))
			{
				if (callbackOutSide != null)
				{
					callbackOutSide.Invoke();
				}
			}
			else
			{
				if (callbackInSide != null)
				{
					callbackInSide.Invoke();
				}
			}
		}
	}

	public static void ShowNotice(Transform parentTransform, string des, float seconds)
	{
		//GameObject instance = LoadGameObject(Constant.PATH_NOTICE_POPUP, parentTransform, null) as GameObject;
		//instance.GetComponent<NoticePopup>().Init(des, seconds);
	}

	public static Vector2 GetSpritePivot(Sprite sprite)
	{
		Bounds bounds = sprite.bounds;
		float pivotX = -bounds.center.x / bounds.extents.x / 2f + 0.5f;
		float pivotY = -bounds.center.y / bounds.extents.y / 2f + 0.5f;
		return new Vector2(pivotX, pivotY);
	}

	public static IEnumerator WaitingComplete(bool value, Action callback)
	{
		yield return new WaitUntil(() => value);
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	public static void ActiveObjectScale(GameObject instance)
	{
		instance.SetActive(true);
		instance.GetComponent<RectTransform>().transform.localScale = Vector3.zero;

		instance.GetComponent<RectTransform>().DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.DOFade(1, 0.2f);
		}
	}

	public static void DeactiveObjectScale(GameObject instance)
	{
		CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
			{
				instance.SetActive(false);
			});
		}
		else
		{
			instance.SetActive(false);
		}
	}

	public static void SetActive(GameObject obj, bool isValue)
	{
		if (obj != null)
		{
			if (isValue)
			{
				if (!obj.activeSelf)
				{
					obj.SetActive(true);
				}
			}
			else
			{
				if (obj.activeSelf)
				{
					obj.SetActive(false);
				}
			}
		}
	}

	public static string GetUrlAvatar(Dictionary<string, string> externalIds, int size = 64) {
		string urlAvatar = "";
		if (externalIds != null && externalIds.ContainsKey("FB")) {
			urlAvatar = "https" + "://graph.facebook.com/" + externalIds["FB"] +
				"/picture?type=square&height=" + size + "&width=" + size + "";
		}
		return urlAvatar;
	}

	public void DownLoadImageFromURL(string url, Action<Sprite> OnDownLoadComplete)
	{
		//StartCoroutine(IDownLoadImageFromURL(url, OnDownLoadComplete));
	}

	//public static IEnumerator IDownLoadImageFromURL(string url, Action<Sprite> OnDownLoadComplete)
	//{
	//	if(url == Constant.LINK_AVATAR_DEFAULT) {
	//		OnDownLoadComplete(LoadSprites.Instance.GetIcon("avatarDefault"));
	//		yield return null;
 //       } else {
	//		WWW www = new WWW(url);
	//		yield return www;
	//		if (OnDownLoadComplete != null) {
	//			Sprite avata_sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	//			OnDownLoadComplete(avata_sprite);
	//		}
	//	}
	//}

	public static string StrikeThrough(string s)
	{
		string strikethrough = "";
		foreach (char c in s)
		{
			strikethrough = strikethrough + c + '\u0336';
		}
		return strikethrough;
	}

	static string[] bad_words = new string[] {
		"lon",
		"cac",
		"lồn",
		"loz",
		"lồz",
		"cặc",
		"con di",
		"con đĩ",
		"bake",
		"bac ki",
		"bắc kì",
		"bắc ki",
		"bac kì",
		"đĩ",
		"đéo",
		"dm",
		"đm",
		"đ m",
		"d m",
		"đụ",
		"du ma",
		"dit",
		"nứng",
		"địt",
		"dcm",
		"cc",
		"poop",
		"shit",
		"fuck",
	};

	public static void SetLeft(RectTransform rt, float left) {
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(RectTransform rt, float right) {
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(RectTransform rt, float top) {
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(RectTransform rt, float bottom) {
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}

	//send message to slack
	static Dictionary<string, object> _defaultOption = new Dictionary<string, object>();
	public static IEnumerator PostSlack(string message) {
		var headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/x-www-form-urlencoded");
		var form = new WWWForm();

		if (_defaultOption.ContainsKey("text")) {
			_defaultOption["text"] = message;
		} else {
			_defaultOption.Add("text", message);
		}
		//form.AddField("payload", JsonConvert.SerializeObject(_defaultOption));

		var www = new WWW("https://hooks.slack.com/services/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",
			form.data,
			headers);
		yield return www;
		Debug.LogFormat("slack result {0}", www.text);
		// Utilities.ShowPopupNotice("Send success message to Slack");
	}
}
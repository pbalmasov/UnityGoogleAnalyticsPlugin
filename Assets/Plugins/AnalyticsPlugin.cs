using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class AnalyticsPlugin : MonoBehaviour
{

		//replace with you own TRACKING_ID
		public string TRACKING_ID = "UA-XXXXXXXX-X";
	
	#if DEBUG
	void OnGUI ()
	{
		if (GUI.Button(new Rect(0,0,200,200), "SendEvent"))
		{
			LogEvent("Main","HomeScreen","open","1");
		}
	}
	#endif
	
		void Start ()
		{
				StartSession ();
		}
	#if UNITY_EDITOR
	
	public void StartSession()
	{
		Debug.Log(TRACKING_ID);
	}
	
	public static void LogEvent (string category, string action,string label = null, string value = null)
	{
		Debug.Log("Category "+category+", action "+action+", label "+label+", value "+value);
	}

	#elif UNITY_ANDROID
	private static AndroidJavaClass mGoogleAnalyticsClass;
	private static AndroidJavaClass GA
	{
		get
		{
			if (mGoogleAnalyticsClass == null)
			{
				mGoogleAnalyticsClass = new AndroidJavaClass("ru.peppersstudio.analyticsplugin.AnalyticsPlugin");
				
				if (mGoogleAnalyticsClass == null)
				{
					throw new MissingReferenceException(string.Format("AnalyticsPlugin failed to load {0} class", "ru.peppersstudio.analyticsplugin.AnalyticsPlugin"));
				}
			}
			return mGoogleAnalyticsClass;
		}
	}
	
	
	public void StartSession()
	{
		#if DEBUG
		AndroidJNIHelper.debug = true;
		#endif
		GA.CallStatic("GoogleAnalytics_startSession", TRACKING_ID);
	}
	
	
	public static void LogEvent (string category, string action,string label = null, string value = null)
	{
		GA.CallStatic("GoogleAnalytics_logEventWithParameters", category, action, label, value);
	}
	
	
	#elif UNITY_IPHONE
	
	#region GoogleSDK_Imports
	[DllImport("__Internal")]
	private static extern void GoogleAnalytics_startSession();
	
	[DllImport("__Internal")]
	private static extern void GoogleAnalytics_logEvent(string category, string action,string label, string value)
	
	#endregion
	
	public void StartSession()
	{
		GoogleAnalytics_startSession(TRACKING_ID);
	}
	
	public void LogEvent(string category, string action,string label = null, string value = null)
	{
		GoogleAnalytics_logEvent(category,action,label,value);
	}
	#endif
}

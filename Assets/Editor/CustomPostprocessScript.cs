#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Diagnostics;

public class CustomPostprocessScript : MonoBehaviour
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
	{        
		UnityEngine.Debug.Log("----Custome Script---Executing post process build phase."); 		
		string objCPath = Application.dataPath + "Editor/GooglePlusSDK";
		Process myCustomProcess = new Process();		
		myCustomProcess.StartInfo.FileName = "python";
		UnityEngine.Debug.Log("pathToBuildProject "+pathToBuildProject); 
		UnityEngine.Debug.Log("pathToBuildProject "+objCPath); 
		myCustomProcess.StartInfo.Arguments = string.Format("Assets/Editor/postprocess.py \"{0}\" \"{1}\"", pathToBuildProject, objCPath);
		myCustomProcess.StartInfo.UseShellExecute = false;
		myCustomProcess.StartInfo.RedirectStandardOutput = false;
		myCustomProcess.Start(); 
		myCustomProcess.WaitForExit();
		UnityEngine.Debug.Log("----Custome Script--- Finished executing post process build phase.");  		
	}
}
#endif
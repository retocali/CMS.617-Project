using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Data {

	private static List<string> levels_completed = new List<string>();
	private static string lastCompletedScene = "";
	public static string lastAttemptedScene = "";
	public static bool started = true;

	public static int getLevelsCompleted()
	{
		return levels_completed.Count;
	}
	
	public static bool markLevelCompleted(string levelname)
	{
		lastCompletedScene = levelname;
		if (levels_completed.Contains(levelname))
		{
			return false;
		}
		levels_completed.Add(levelname);
		return true;
	}	

	public static bool checkLevelCompleted(string levelname)
	{
		return levels_completed.Contains(levelname);
	}

	public static string lastCompleted()
	{
		return ""+lastCompletedScene;
	}	
}

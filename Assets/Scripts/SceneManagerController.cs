using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagerController : MonoBehaviour {

	[SerializeField] UrielChallenge.ApplicationData _appData;

	public void ChangeScene()
	{
		//Debug.Log("Cambio!");
		if(_appData.dayTime == UrielChallenge.DayTime.Day && _appData.scenarioType == UrielChallenge.ScenarioType.Garage)
		{
			SceneManager.LoadScene("_garageDay");
		}
		else if(_appData.dayTime == UrielChallenge.DayTime.Night && _appData.scenarioType == UrielChallenge.ScenarioType.Garage)
		{
			SceneManager.LoadScene("_garageNight");
		}
		else if(_appData.dayTime == UrielChallenge.DayTime.Day && _appData.scenarioType == UrielChallenge.ScenarioType.Outskirts)
		{
			SceneManager.LoadScene("_outsideDay");
		}
		else if(_appData.dayTime == UrielChallenge.DayTime.Night && _appData.scenarioType == UrielChallenge.ScenarioType.Outskirts)
		{
			SceneManager.LoadScene("_outsideNight");
		}
		
	}


}

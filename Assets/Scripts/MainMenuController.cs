using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class MainMenuController : MonoBehaviour
{
	public ApplicationData data;

	public Button3D dayButton;
	public Button3D nightButton;
	public Button3D maleButton;
	public Button3D femaleButton;
	public Button3D garageButton;
	public Button3D outskirtsButton;
	public Button3D startButton;

	public GameObject _garage;
	public GameObject _outside;


	private void Awake()
	{
		dayButton.onClick.AddListener(()=>{ UpdateDaytime(DayTime.Day); });
		nightButton.onClick.AddListener(()=>{ UpdateDaytime(DayTime.Night); });
		maleButton.onClick.AddListener(()=> { UpdateGender(Gender.Male); });
		femaleButton.onClick.AddListener(()=> { UpdateGender(Gender.Female); });
		garageButton.onClick.AddListener(()=> { UpdateScenarioType(ScenarioType.Garage); });
		outskirtsButton.onClick.AddListener(()=> { UpdateScenarioType(ScenarioType.Outskirts); });
		startButton.onClick.AddListener(Begin);

		LoadData();
	}

	private void LoadData()
	{
		switch(data.dayTime)
		{
			case DayTime.Day:
			UpdateDaytime(DayTime.Day);
			break;

			case DayTime.Night:
			UpdateDaytime(DayTime.Night);
			break;
		}

		switch(data.scenarioType)
		{
			case ScenarioType.Garage:
			_garage.SetActive(true);
			_outside.SetActive(false);
			break;

			case ScenarioType.Outskirts:
			_garage.SetActive(false);
			_outside.SetActive(true);
			break;
		}
	}

	private void Begin()
	{
		Debug.Log("START SIMULATOR");
		string sceneName = data.scenarioType.ToString() + "_" + data.dayTime;
		AppManager.ChangeScene(sceneName);
	}

	private void UpdateGender(Gender _gender)
	{
		data.gender = _gender;
	}

	private void UpdateDaytime(DayTime _dayTime)
	{
		data.dayTime = _dayTime;
	}
	private void UpdateScenarioType(ScenarioType _scenarioType)
	{
		data.scenarioType = _scenarioType;

		switch(data.scenarioType)
		{
			case ScenarioType.Garage:
			_garage.SetActive(true);
			_outside.SetActive(false);
			break;

			case ScenarioType.Outskirts:
			_garage.SetActive(false);
			_outside.SetActive(true);
			break;
		}
	}
}
}
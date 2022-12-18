using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public delegate void OnChallengeAchieved();

public class Car : MonoBehaviour
{
	private const string MESSAGE_ALL_TIRES_INSTALLEED = "All Tires Are Installed!!";
	private const string MESSAGE_TIRES_NOT_INSTALLED = "There is a flat tire.";

	public static event OnChallengeAchieved onChallengeAchieved;

	[SerializeField] private Disc[] _discs;

	public  Disc[] discs { get { return _discs; } }

	private void OnEnable()
	{
		if(_discs != null)
		{
			foreach(Disc disc in discs)
			{
				disc.onTireInstalled += EvaluateChallenge;
			}
		}
	}

	private void OnDisable()
	{
		if(_discs != null)
		{
			foreach(Disc disc in discs)
			{
				disc.onTireInstalled -= EvaluateChallenge;
			}
		}
	}

	private void Awake()
	{
		Debug.Log("All Tires Ready: " + AllTiresAreReady());
		EvaluateChallenge();
		//Time.timeScale = 0.1f;
	}

	public bool AllTiresAreReady()
	{
		if(_discs != null)
		{
			foreach(Disc disc in discs)
			{
				if(!disc.TireInstalled()) return false;
			}

			return true;
		}
		else return false;
	}

	private void EvaluateChallenge()
	{
		if(AllTiresAreReady() && onChallengeAchieved != null)
		{
			onChallengeAchieved();
			UserFeedbackUI.Instance.ShowMessage(MESSAGE_ALL_TIRES_INSTALLEED);
		}
		//UserFeedbackUI.Instance.ShowMessage(MESSAGE_TIRES_NOT_INSTALLED);
	}
}
}
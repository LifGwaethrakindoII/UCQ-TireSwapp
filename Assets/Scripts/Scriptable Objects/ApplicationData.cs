using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UrielChallenge
{
public enum ScenarioType
{
    Garage,
    Outskirts
}

public enum DayTime
{
    Day,
    Night
}

[CreateAssetMenu(menuName = ASSET_PATH_DATA_ROOT + "Application Data")]
public class ApplicationData : ScriptableObject
{
	public const string ASSET_PATH_DATA_ROOT = "Uriel's Challenge Data / "; 	/// <summary>Data's Asset root path.</summary>
	public const string SCENE_PATH_PREFIX = "Scene_"; 							/// <summary>Scene's Path prefix.</summary>
	public const string SCENE_PATH_MENU = SCENE_PATH_PREFIX + "Menu"; 			/// <summary>Menu Scene's Path.</summary>

    [Header("Data:")]
	[SerializeField] private User _user;                                        /// <summary>User's Prefab reference.</summary>
    [SerializeField] private Gender _gender;                                    /// <summary>User's Gender.</summary>
    [SerializeField] private DayTime _dayTime;                                  /// <summary>Daytime Selected.</summary>
    [SerializeField] private ScenarioType _scenarioType;                        /// <summary>Scenario Type Selected.</summary>
    [Space(5f)]
    [Header("Hands:")]
    [SerializeField] private GameObject _maleLeftHand;                          /// <summary>Male's Left Hand.</summary>
    [SerializeField] private GameObject _maleRightHand;                         /// <summary>Male's Right Hand.</summary>
    [SerializeField] private GameObject _femaleLeftHand;                        /// <summary>Female's Left Hand.</summary>
    [SerializeField] private GameObject _femaleRightHand;                       /// <summary>Female's Right Hand.</summary>
    [Space(5f)]
    [Header("Configurations:")]
    [SerializeField] private float _cycleChangeDuration;
    [SerializeField] private float _fadeInDuration;                             /// <summary>Fade In's Duration.</summary>
    [SerializeField] private float _fadeOutDuration;                            /// <summary>Fade Out's Duration.</summary>
    [SerializeField] private float _fadeSuspendWait;                            /// <summary>Fade Suspend Wait.</summary>

	/// <summary>Gets user property.</summary>
	public User user { get { return _user; } }

    public Gender gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    public DayTime dayTime
    {
        get { return _dayTime; }
        set { _dayTime = value; }
    }

    public ScenarioType scenarioType
    {
        get { return _scenarioType; }
        set { _scenarioType = value; }
    }

    /// <summary>Gets maleLeftHand property.</summary>
    public GameObject maleLeftHand { get { return _maleLeftHand; } }

    /// <summary>Gets maleRightHand property.</summary>
    public GameObject maleRightHand { get { return _maleRightHand; } }

    /// <summary>Gets femaleLeftHand property.</summary>
    public GameObject femaleLeftHand { get { return _femaleLeftHand; } }

    /// <summary>Gets femaleRightHand property.</summary>
    public GameObject femaleRightHand { get { return _femaleRightHand; } }

    public float cycleChangeDuration { get { return _cycleChangeDuration; } }

    /// <summary>Gets fadeInDuration property.</summary>
    public float fadeInDuration { get { return _fadeInDuration; } }

    /// <summary>Gets fadeOutDuration property.</summary>
    public float fadeOutDuration { get { return _fadeOutDuration; } }

    /// <summary>Gets fadeSuspendWait property.</summary>
    public float fadeSuspendWait { get { return _fadeSuspendWait; } }
}
}
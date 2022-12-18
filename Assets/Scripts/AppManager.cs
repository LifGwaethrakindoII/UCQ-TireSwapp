using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UrielChallenge
{
public delegate void OnHandGenreUpdated(GameObject _hand);

public class AppManager : MonoBehaviour
{
    public const int BIT_POSITION_NONE = 0; 
    public const int BIT_POSITION_IDENTIFY_TIRE = 1; 
    public const int BIT_POSITION_PUT_JACK_BELLOW_TIRE = 2; 
    //public const int BIT_POSITION_PUT_JACK_BELLOW_TIRE = 2;

    public const string MESSAGE_IDENTITY_TIRE = "Encuentra la llanta ponchada.";
    public const string MESSAGE_PUT_JACK = "Coloca el gato hidraulico debajo del carro.";
    public const string MESSAGE_USE_JACK = "Hala del gato hidraulico hasta que el carro este elevado";
    public const string MESSAGE_UNTIGHTEN_NUT = "Desenruesca los birlos de la llanta";
    public const string MESSAGE_TAKE_TIRE_OUT = "Retira la llanta del disco"; 

    public static event OnHandGenreUpdated onHandGenreUpdated;  /// <summary>OnHandGenderUpdated's delegate.</summary>

    [SerializeField] private ApplicationData _appData;          /// <summary>Application's Data.</summary>
    [SerializeField] private User _user;                        /// <summary>User.</summary>
    [Space(5f)]
    [Header("GUI:")]
    [SerializeField] private GameObject _pauseCanvas;           /// <summary>Pause's Canvas.</summary>
	[SerializeField] private Image _teleportImage;              /// <summary>Teleport's Image.</summary>
	[SerializeField] private Text _textUIGameplay;              /// <summary>Gameplay's Text.</summary>
    private GameObject maleLeftHand;                            /// <summary>Male's Left Hand.</summary>
	private GameObject maleRightHand;                           /// <summary>Male's Right Hand.</summary>
    private GameObject femaleLeftHand;                          /// <summary>Female's Left Hand.</summary>
    private GameObject femaleRightHand;                         /// <summary>Female's Right Hand.</summary>
    private bool isPaused;                                      /// <summary>Is the Application Paused?.</summary>
    private bool rightHandPressedPause;                         /// <summary>Did the Right Hand press pause?.</summary>
    private bool leftHandPressedPause;                          /// <summary>Did the Left Hand press pause?.</summary>

    /// <summary>Gets appData property.</summary>
    public ApplicationData appData { get { return _appData; } }

    /// <summary>Gets user property.</summary>
    public User user { get { return _user; } }

    /// <summary>Gets pauseCanvas property.</summary>
    public GameObject pauseCanvas { get { return _pauseCanvas; } }

    /// <summary>Gets teleportImage property.</summary>
    public Image teleportImage { get { return _teleportImage; } }

    /// <summary>Gets textUIGameplay property.</summary>
    public Text textUIGameplay { get { return _textUIGameplay; } }

    private void Awake()
    {
        isPaused = false;
        rightHandPressedPause = false;
        leftHandPressedPause = false;

        maleLeftHand = Instantiate(appData.maleLeftHand);
        maleRightHand = Instantiate(appData.maleRightHand);
        femaleLeftHand = Instantiate(appData.femaleLeftHand);
        femaleRightHand = Instantiate(appData.femaleRightHand);

        PutHand(maleLeftHand.transform, user.leftHand.transform);
        PutHand(maleRightHand.transform, user.rightHand.transform);
        PutHand(femaleLeftHand.transform, user.leftHand.transform);
        PutHand(femaleRightHand.transform, user.rightHand.transform);

        UpdateHandsByGender();
    }

    private void Start()
	{
        pauseCanvas.SetActive(false);
	}

	private void Update()
	{
        if(user.leftHand.GetDevice() != null && user.rightHand.GetDevice() != null) TrackInput();
	}

    private void TrackInput()
    {
        if(user.leftHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) || 
        user.rightHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            StartCoroutine(TeleportFade());
        }

        // Solucion Nain
        if(!isPaused)
        {
            if(user.leftHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                leftHandPressedPause = true;
                isPaused = !isPaused;
                pauseCanvas.SetActive(isPaused);
            }
            if(user.rightHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                rightHandPressedPause = true;
                isPaused = !isPaused;
                pauseCanvas.SetActive(isPaused);
            }
        } else if(isPaused && (user.leftHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)
            || user.rightHand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)))
        {
        
            if(leftHandPressedPause)
            {
                leftHandPressedPause = false;
                isPaused = !isPaused;
                pauseCanvas.SetActive(false);
            }
            if(rightHandPressedPause)
            {
                rightHandPressedPause = false;
                isPaused = !isPaused;
                pauseCanvas.SetActive(false);
            }
        }
    }
    
    /// <summary>Changes Scene.</summary>
    /// <param name="_scene">Scene's Name.</param>
    public static void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    /// <summary>Leaves Application.</summary>
    public static void ExitApplication()
    {
        UnityEngine.Application.Quit();
    }

    private void UpdateHandsByGender()
    {
        bool male = appData.gender == Gender.Male;

        maleRightHand.SetActive(male);
        maleLeftHand.SetActive(male);
        femaleRightHand.SetActive(!male);
        femaleLeftHand.SetActive(!male);

        user.rightHand.animator = male ? maleRightHand.GetComponent<Animator>() : femaleRightHand.GetComponent<Animator>();  
        user.leftHand.animator = male ? maleLeftHand.GetComponent<Animator>() : femaleLeftHand.GetComponent<Animator>();
    }

    private void PutHand(Transform _hand, Transform _parent)
    {
        Quaternion handRotation = _hand.rotation;

        _hand.parent = _parent;
        _hand.localPosition = Vector3.zero;
        _hand.localRotation = handRotation;
    }

	private IEnumerator TeleportFade()
	{
		_teleportImage.CrossFadeAlpha(255, appData.fadeInDuration, false);
		yield return new WaitForSeconds(appData.fadeSuspendWait);
		user.leftHand.shouldTeleport = true;
		user.rightHand.shouldTeleport = true;
		_teleportImage.CrossFadeAlpha(0, appData.fadeOutDuration, false);
	}
	
}
}
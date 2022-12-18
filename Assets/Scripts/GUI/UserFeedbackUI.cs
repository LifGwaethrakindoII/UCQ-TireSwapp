using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class UserFeedbackUI : MonoBehaviour
{
	private const float DEFAULT_DURATION = 4.0f; 		/// <summary>Default Text's Duration.</summary>

	private static UserFeedbackUI _Instance; 			/// <summary>Static Instance's Reference.</summary>

	[SerializeField] private Transform _contentPanel; 	/// <summary>Content's Panel.</summary>
	[SerializeField] private TextFeedback _textPrefab; 	/// <summary>Text's Prefab.</summary>
	[SerializeField] private TextFeedback _text; 		/// <summary>Text.</summary>

	/// <summary>Gets and Sets Instance property.</summary>
	public static UserFeedbackUI Instance
	{
		get { return _Instance; }
		private set { _Instance = value; }
	}

	/// <summary>Gets contentPanel property.</summary>
	public Transform contentPanel { get { return _contentPanel; } }

	/// <summary>Gets textPrefab property.</summary>
	public TextFeedback textPrefab { get { return _textPrefab; } }

	/// <summary>Gets text property.</summary>
	public TextFeedback text { get { return _text; } }

	private void Awake()
	{
		if(Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;
	}

	public void ShowMessage(string _message, float _duration = DEFAULT_DURATION)
	{
		/*TextFeedback text = Instantiate(textPrefab) as TextFeedback;
		text.transform.parent = contentPanel;
		text.transform.localScale = Vector3.one;
		text.transform.localPosition = Vector3.zero;
		text.transform.localRotation = Quaternion.identity;*/
		text.StartFeedbackCoroutine(_message, _duration);
	}
}
}
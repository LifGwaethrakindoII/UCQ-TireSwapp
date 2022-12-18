using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ulyssess;

namespace UrielChallenge
{
public class TextFeedback : MonoBehaviour
{
	[SerializeField] private float _alphaChangeSpeed; 	/// <summary>Alpha Change's Speed.</summary>
	private float _feedbackDuration; 					/// <summary>Feedback's Duration.</summary>
	private Coroutine feedbackCoroutine; 				/// <summary>Feedback's Coroutine.</summary>
	private Text _text; 								/// <summary>Text's Component.</summary>

	/// <summary>Gets alphaChangeSpeed property.</summary>
	public float alphaChangeSpeed { get { return _alphaChangeSpeed; } }

	/// <summary>Gets and Sets feedbackDuration property.</summary>
	public float feedbackDuration
	{
		get { return _feedbackDuration; }
		set { _feedbackDuration = value; }
	}

	/// <summary>Gets and Sets text Component.</summary>
	public Text text
	{ 
		get
		{
			if(_text == null)
			{
				_text = GetComponent<Text>();
			}
			return _text;
		}
	}

	public void StartFeedbackCoroutine(string _message, float _duration)
	{
		gameObject.SetActive(true);
		text.text = _message;
		feedbackDuration = _duration;
		this.StartCoroutine(FeedbackCoroutine(), ref feedbackCoroutine);
	}

	private IEnumerator FeedbackCoroutine()
	{
		Color destinyColor = text.color;
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			destinyColor.a = Extensions.Remap(Mathf.Sin(Time.time * alphaChangeSpeed), -1.0f, 1.0f, 0.0f, 1.0f);
			text.color = destinyColor;
			n += (Time.deltaTime / feedbackDuration);
			yield return null;
		}

		gameObject.SetActive(false);
		//Destroy(gameObject);
	}
}
}
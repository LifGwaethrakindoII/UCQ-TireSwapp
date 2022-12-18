using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ulyssess;

public static class AsynchronousSceneLoader
{
	private static AsyncOperation operation; 		/// Asynchronous Operation.
	private static Coroutine loadingOperation; 		/// Loading Operation.
	private static IEnumerator<float> progress; 	/// Current's Progress.

	public static bool loading { get { return operation != null; } }

	public static float sceneProgress { get { return progress.Current; } }

	public static void LoadScene(this MonoBehaviour _mono, string _scene, float? _minimumWait = null, bool _allowSceneActivation = false)
	{
		operation = SceneManager.LoadSceneAsync(_scene);
		operation.allowSceneActivation = _allowSceneActivation;
		progress = LoadSceneCoroutine(_scene, _minimumWait, _allowSceneActivation);
		_mono.StartCoroutine(progress);
	}

	private static IEnumerator<float> LoadSceneCoroutine(string _scene, float? _minimumWait, bool _allowSceneActivation = false)
	{
		float wait = 0.0f;

		while(((_minimumWait.HasValue && wait < _minimumWait.Value) || !_minimumWait.HasValue) && operation.progress < 0.9f)
		{
			wait += Time.deltaTime;
			yield return !_minimumWait.HasValue ? operation.progress : Mathf.Min(operation.progress, wait);
		}

		operation.allowSceneActivation = true;
		operation = null;
	}
}

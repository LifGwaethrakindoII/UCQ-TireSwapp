using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public GameObject _refSun;

    public float num;

    public void ChangeRotationSun()
    {
        _refSun.transform.Rotate( new Vector3(150,0,0)
            , Space.World);

    }

    private void Awake()
    {
        ChangeRotationSun();
    }

}

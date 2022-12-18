using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour {

    public Transform _target;


    public float _velocidad;

    private void Update()
    {
        Vector3 direccion = _target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(direccion);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _velocidad * Time.deltaTime);
    }
}

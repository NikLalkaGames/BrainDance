using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayCast : MonoBehaviour
{
    private RaycastHit _hit;
    public Camera _camera;

    void Start()
    {

    }

    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out _hit);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        //if (_hit.collider.TryGetComponent(out MeshRenderer mesh))
        //{
        // mesh.enabled = true;
        //}
    }
}

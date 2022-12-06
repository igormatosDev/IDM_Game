using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerPosition;

    private void Awake()
    {
        playerPosition = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        var camera = Camera.main;
        var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        var vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        if (vcam != null)
        {
            float orthoSize = 25 * Screen.height / Screen.width * 0.5f;
            vcam.m_Lens.OrthographicSize = orthoSize;
        }
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, transform.position.z);
    }
}

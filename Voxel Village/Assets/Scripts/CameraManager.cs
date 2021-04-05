using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camL1;
    public Camera camR1;
    public Camera camL2;
    public Camera camR2;

    public Camera mainCamera;

    public float cameraFOV;

    private bool is360 = false;

    // Start is called before the first frame update
    void Start()
    {
        camL1.rect = new Rect(0.25f, 0.0f, 0.25f, 0.25f);
        camR1.rect = new Rect(0.5f, 0.0f, 0.25f, 0.25f);
        camL2.rect = new Rect(0.0f, 0.0f, 0.25f, 0.25f);
        camR2.rect = new Rect(0.75f, 0.0f, 0.25f, 0.25f);
        mainCamera.rect = new Rect(0.0f, 0.25f, 1.0f, 0.75f);

        camL1.fieldOfView = cameraFOV;
        camR1.fieldOfView = cameraFOV;
        camL2.fieldOfView = cameraFOV;
        camR2.fieldOfView = cameraFOV;

        Disable360();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle360()
    {
        if(is360)
        {
            Disable360();
        }
        else
        {
            Enable360();
        }
        is360 = !is360;
    }

    public void Enable360()
    {
        camL1.gameObject.SetActive(true);
        camR1.gameObject.SetActive(true);
        camL2.gameObject.SetActive(true);
        camR2.gameObject.SetActive(true);

        mainCamera.rect = new Rect(0.0f, 0.25f, 1.0f, 0.75f);
    }

    public void Disable360()
    {
        camL1.gameObject.SetActive(false);
        camR1.gameObject.SetActive(false);
        camL2.gameObject.SetActive(false);
        camR2.gameObject.SetActive(false);

        mainCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
    }
}

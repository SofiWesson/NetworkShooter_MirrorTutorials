using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class SpawnCameraSetUp : NetworkBehaviour
{
    public GameObject casualCamPrefab;
    public GameObject aimCamPrefab;
    public Transform camBase;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
            return;

        GameObject casualCam = Instantiate(casualCamPrefab);
        casualCam.GetComponent<CinemachineVirtualCamera>().Follow = camBase;
        casualCam.transform.parent = transform;
        casualCam.SetActive(true);
        GameObject aimCam = Instantiate(aimCamPrefab);
        aimCam.GetComponent<CinemachineVirtualCamera>().Follow = camBase;
        aimCam.transform.parent = transform;
        GetComponent<AimOverrideControllerTPS>().SetAimCam(aimCam.GetComponent<CinemachineVirtualCamera>());
    }
}

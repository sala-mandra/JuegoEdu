using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    [SerializeField] private GameObject _arSessionGO;
    [SerializeField] private GameObject _arCameraOriginGO;

    private void Start()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            StartCoroutine(WaitForCameraPermission());
        }
        else
        {
            EnableObjectAR();
        }
#endif
    }

    private IEnumerator WaitForCameraPermission()
    {
#if UNITY_ANDROID
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }
        EnableObjectAR();
#endif
    }

    private void EnableObjectAR()
    {
        _arSessionGO.SetActive(true);
        _arCameraOriginGO.SetActive(true);
        
        var session = _arSessionGO.GetComponent<ARSession>();
        if (session != null)
        {
            session.Reset();
        }
    }
}
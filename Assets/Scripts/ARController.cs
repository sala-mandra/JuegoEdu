using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    [SerializeField] private GameObject _arSessionGO;
    [SerializeField] private GameObject _arCameraOriginGO;

    private IEnumerator Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }
#endif

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(InitializeARSession());

        _arSessionGO.SetActive(true);
        _arCameraOriginGO.SetActive(true);

        yield return new WaitForSeconds(1);

        GameController.Instance.StartGame();
    }

    private IEnumerator InitializeARSession()
    {
        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            Debug.Log("AR Session requires installation. Attempting to install...");
            yield return ARSession.Install();

            if (ARSession.state != ARSessionState.Ready)
            {
                Debug.LogError("AR installation failed. AR is not available.");
                yield break;
            }
        }
        else if (ARSession.state != ARSessionState.Ready)
        {
            Debug.Log("AR is not available on this device.");
            yield break;
        }

        Debug.Log("AR Session is ready.");
    }
}
using UnityEngine;
using UnityEngine.Video;

public class VideoSequenceController : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoOriginLaw;
    [SerializeField] private VideoPlayer _videoPresentation;
    [SerializeField] private GameObject _panelMenuSpiral;
    [SerializeField] private GameObject _containerStartGamePanels;

    private VideoPlayer _vp;
    
    private void Start()
    {
        _vp = _videoOriginLaw;
        _videoOriginLaw.loopPointReached += OnFirstVideoEnd;
        _videoPresentation.loopPointReached += OnSecondVideoEnd;
    }
    
    public void SkipVideo()
    {
        if (_videoOriginLaw.isPlaying)
        {
            _videoOriginLaw.Stop();
            OnFirstVideoEnd(_videoOriginLaw);
        }
        else if (_videoPresentation.isPlaying)
        {
            _videoPresentation.Stop();
            OnSecondVideoEnd(_videoPresentation);
        }
    }
    
    private void OnFirstVideoEnd(VideoPlayer video1)
    {
        _vp = _videoPresentation;
        _videoPresentation.Play();
    }

    private void OnSecondVideoEnd(VideoPlayer video2)
    {
        _containerStartGamePanels.SetActive(false);
        _panelMenuSpiral.SetActive(true);
    }
}
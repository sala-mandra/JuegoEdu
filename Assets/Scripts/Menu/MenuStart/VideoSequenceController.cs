using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSequenceController : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoOriginLaw;
    [SerializeField] private VideoPlayer _videoPresentation;
    [SerializeField] private GameObject _panelMenuSpiral;
    [SerializeField] private GameObject _containerStartGamePanels;
    [SerializeField] private GameObject _audioSourceMenu;
    [SerializeField] private GameObject _viewOriginLaw;
    [SerializeField] private GameObject _viewPresentation;

    private void Start()
    {
        _videoOriginLaw.loopPointReached += OnFirstVideoEnd;
        _videoPresentation.loopPointReached += OnSecondVideoEnd;
    }

    public void StartFirstVideo()
    {
        _viewOriginLaw.SetActive(true);
        _videoOriginLaw.Play();
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
        _viewOriginLaw.SetActive(false);
        _viewPresentation.SetActive(true);
        _videoPresentation.Play();
    }

    private void OnSecondVideoEnd(VideoPlayer video2)
    {
        _viewPresentation.SetActive(false);
        _containerStartGamePanels.SetActive(false);
        _panelMenuSpiral.SetActive(true);
        _audioSourceMenu.SetActive(true);
    }
}
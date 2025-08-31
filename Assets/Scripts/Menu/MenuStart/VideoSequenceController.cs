using System;
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
    [SerializeField] private GameObject _blackOverlay;

    private void Start()
    {
        _videoOriginLaw.loopPointReached += OnFirstVideoEnd;
        _videoPresentation.loopPointReached += OnSecondVideoEnd;

        _videoOriginLaw.prepareCompleted += OnVideoPrepared;
        _videoPresentation.prepareCompleted += OnVideoPrepared;
    }

    public void StartFirstVideo()
    {
        _viewOriginLaw.SetActive(true);
        _blackOverlay.SetActive(true);
        _videoOriginLaw.Prepare();
    }
    
    private void OnVideoPrepared(VideoPlayer vp)
    {
        if (_blackOverlay != null)
            _blackOverlay.SetActive(false);

        vp.Play();
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
        _blackOverlay.SetActive(true);
        _videoPresentation.Prepare();
        //_videoPresentation.Play();
    }

    private void OnSecondVideoEnd(VideoPlayer video2)
    {
        _viewPresentation.SetActive(false);
        _containerStartGamePanels.SetActive(false);
        _panelMenuSpiral.SetActive(true);
        _audioSourceMenu.SetActive(true);
    }

    private void OnDisable()
    {
        _videoOriginLaw.loopPointReached -= OnFirstVideoEnd;
        _videoPresentation.loopPointReached -= OnSecondVideoEnd;
    }
}
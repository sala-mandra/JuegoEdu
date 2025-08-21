using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoSequenceController : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoOriginLaw;
    [SerializeField] private VideoPlayer _videoPresentation;
    [SerializeField] private GameObject _panelMenuSpiral;
    [SerializeField] private GameObject _containerStartGamePanels;

    private void Start()
    {
        _videoOriginLaw.loopPointReached += OnFirstVideoEnd;
        _videoPresentation.loopPointReached += OnSecondVideoEnd;
    }
    
    private void OnFirstVideoEnd(VideoPlayer video1)
    {
        _videoPresentation.Play();
    }

    private void OnSecondVideoEnd(VideoPlayer video2)
    {
        _containerStartGamePanels.SetActive(false);
        _panelMenuSpiral.SetActive(true);
    }
}
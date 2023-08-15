using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialCanvasHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    [TextArea]public string[] sentences;
    [SerializeField]private int _currentIndex = 0;
    private bool _finishedMoving = false;
    private bool _finishedZooming = false;
    private bool _finishedRotating = false;
    private bool _movedUp = false;
    private bool _movedDown = false;
    private bool _movedLeft = false;
    private bool _movedRight = false;
    private bool _zoomApplied = false;
    private bool _RotationApplied = false;

    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;


    // Start is called before the first frame update
    void Start()
    {
        text.text = sentences[_currentIndex];
        gameEventChannel.OnTowerPlaced.AddListener(HandleTowerPlaced);
        gameEventChannel.OnGameWaveStatusChange.AddListener(HandleWaveChange);
    }

    private void HandleWaveChange(bool status,int waveIndex)
    {
        if (status)
            MoveToNextSentence();
    }

    private void HandleTowerPlaced(TowerDataSO arg0)
    {
        MoveToNextSentence();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) _movedUp = true;
        if (Input.GetKeyDown(KeyCode.S)) _movedDown = true;
        if (Input.GetKeyDown(KeyCode.A)) _movedLeft = true;
        if (Input.GetKeyDown(KeyCode.D)) _movedRight = true;

        if (Input.mouseScrollDelta.y > 0) _zoomApplied = true;
        if (Input.GetMouseButtonDown(2)) _RotationApplied = true;

        if (_movedUp && _movedDown && _movedLeft && _movedRight && !_finishedMoving)
        {
            MoveToNextSentence();
            _finishedMoving = true;
        }
        if (_zoomApplied && !_finishedZooming)
        {
            MoveToNextSentence();
            _finishedZooming = true;
        }
        if (_RotationApplied && !_finishedRotating)
        {
            MoveToNextSentence();
            _finishedRotating = true;
        }

    }

    private void MoveToNextSentence()
    {
        if(_currentIndex < sentences.Length - 1)
        {
            _currentIndex++;
            text.text = sentences[_currentIndex];


        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

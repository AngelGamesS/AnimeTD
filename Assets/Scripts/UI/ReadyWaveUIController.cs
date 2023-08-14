using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReadyWaveUIController : MonoBehaviour
{

    private VisualElement _nextWaveBTN;
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _nextWaveBTN = root.Q<VisualElement>("NextWaveBTN");
        _nextWaveBTN.AddManipulator(new Clickable(evt => NextWaveBTN_onClick()));

        gameEventChannel.OnGameWaveStatusChange.AddListener(HandleWaveChange);

    }

    private void NextWaveBTN_onClick()
    {
        GameManager.Instance.ChangeInWave(true);
        _nextWaveBTN.SetEnabled(false);
        _nextWaveBTN.style.opacity = 0;
    }

    private void HandleWaveChange(bool status)
    {
        _nextWaveBTN.SetEnabled(!status);
        if(!status)
            _nextWaveBTN.style.opacity = 1;
    }
}

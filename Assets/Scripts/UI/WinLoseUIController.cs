using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WinLoseUIController : MonoBehaviour
{
    private VisualElement _continueBTN;
    public int nextSceneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _continueBTN = root.Q<VisualElement>("ContinueBTN");
        _continueBTN.AddManipulator(new Clickable(evt => NextWaveBTN_onClick()));

    }

    private void NextWaveBTN_onClick()
    {
        MySceneManager.Instance.MoveToScene(nextSceneIndex);
    }
}

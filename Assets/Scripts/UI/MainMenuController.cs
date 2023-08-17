using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public SaveSystem saveSystem;
    private VisualElement _playBtn;
    private VisualElement _exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        saveSystem.DeleteSaveFile();
        var root = GetComponent<UIDocument>().rootVisualElement;
        _playBtn = root.Q<VisualElement>("PlayBTN");
        _exitBtn = root.Q<VisualElement>("Exit");


        _playBtn.AddManipulator(new Clickable(evt => HandlePlay()));
        _exitBtn.AddManipulator(new Clickable(evt => HandleExit()));

    }

    private void HandleExit()
    {
        Application.Quit();
    }

    private void HandlePlay()
    {
        MySceneManager.Instance.MoveToScene(1);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardPanelController : MonoBehaviour
{
    ScrollView sv;
    private void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        sv = root.Q<ScrollView>("ScrollView");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            sv.AddToClassList("moveUp");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniMapCotroller : MonoBehaviour
{
    public GameObject miniMapCamera;
    private VisualElement miniMap;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        miniMap = root.Q<VisualElement>("MiniMap");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (miniMapCamera.activeSelf)
            {
                miniMapCamera.SetActive(false);
                miniMap.style.opacity = 0;
            }
            else
            {
                miniMapCamera.SetActive(true);
                miniMap.style.opacity = 1;
            }
        }
    }
}

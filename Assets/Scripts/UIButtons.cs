using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{

    [SerializeField] Button viewSceneBTN;
    [SerializeField] Button exitBTN;
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        viewSceneBTN.onClick.AddListener(ViewSceneListener);
        exitBTN.onClick.AddListener(ExitListener);
    }

    private void ViewSceneListener()
    {
        canvas.SetActive(false);
    }

    private void ExitListener()
    {
        Application.Quit();
    }
}

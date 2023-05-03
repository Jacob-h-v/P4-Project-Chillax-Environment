using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{

    [SerializeField] Button viewSceneBTN;
    [SerializeField] Button exitBTN;
    [SerializeField] GameObject canvas;

    // Update is called once per frame
    void Update()
    {
        viewSceneBTN.onClick.AddListener(ViewSceneListener);
        exitBTN.onClick.AddListener(ExitListener);
    }

    // Closes the volume control canvas when View Scene button is clicked
    private void ViewSceneListener()
    {
        canvas.SetActive(false);
    }

    // Closes the application when Exit button is clicked
    private void ExitListener()
    {
        Application.Quit();
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
     *  Å×½ºÆ®
     */

    private string Teststring;
    


    public Button TestButton;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        TestButton.onClick.AddListener(InputAction);
    }

    public void InputAction()
    {
        string name = "TestName1";
        string key = "PlayerName";
        EventBus<string>.PublishAction(key, name);

     

    }

     


    // Update is called once per frame
    void Update()
    {
    }


}

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoSingleton<GameManager>   
{
    /*
     *  Å×½ºÆ®
     */


    #region otherManager
   public  EnemyManager enemyManager { get; private set; }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        enemyManager = new EnemyManager(this);
        
    }

    void Start()
    {
       
      
    }

   
     


    // Update is called once per frame
    void Update()
    {
    }


}




using Attacker;
using Mover;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    GameObject enemyOb;
    


    public void Spawn(UnitData data, Vector3 pos, Transform parent =null)
    {
        var go = Instantiate(data,pos,Quaternion.identity,parent);
        var enemy = GetComponent<Enemy>()?.AddComponent<Enemy>();
       


        switch (data.attackType)
        {
            case AttackType.Melee:

                go.GetComponent<MeleeAttacker>()?.AddComponent<MeleeAttacker>();
                break;
            
        }


        switch (data.moveType)
        {
            case MoverType.Rigidbody:
                go.GetComponent<Rigidbody>()?.AddComponent<Rigidbody>();
                go.GetComponent<RbMover>()?.AddComponent<RbMover>(); //없으면 넣어
                break;
        }


        switch (data.brainType)
        {
            case BrainType.AIMonster:
              
                break ;
            case BrainType.Fsm:
                go.GetComponent<ChaseBrainFSM>()?.AddComponent<ChaseBrainFSM>();
                break;
        }

       // return go;

    }



    //스폰은 해당 펙토리 기능을 가져가서 그저 생성만 하도록 구현할예정

    
}

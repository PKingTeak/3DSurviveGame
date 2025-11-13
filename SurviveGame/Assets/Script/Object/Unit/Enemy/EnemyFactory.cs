
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
    


    public Enemy Spawn(UnitData data, Vector3 pos, Transform parent =null)
    {

        if (data == null)
        {
            Debug.Log($"{data}null입니다.");
        }

        var prefab = data.prfabs;
        var go = Instantiate(prefab, pos, Quaternion.identity, parent);
        
        var enemy = go.GetComponent<Enemy>() ?? go.AddComponent<Enemy>();
        enemy.Init(data);
        

        switch (data.attackType)
        {
            case AttackType.Melee:

                var atk = go.GetComponent<MeleeAttacker>() ??  go.AddComponent<MeleeAttacker>();
                break;
            
        }


        switch (data.moveType)
        {
            case MoverType.Rigidbody:
               var rigid =  go.GetComponent<Rigidbody>() ?? go.AddComponent<Rigidbody>();
                // 회전 고정
                rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


                var RMover =  go.GetComponent<RbMover>() ?? go.AddComponent<RbMover>(); //없으면 넣어  //그러면 해당 값을 변수로 받아서 사용해야하나?
                RMover.Speed = data.moveSpeed;
                break;
        }


        switch (data.brainType)
        {
            case BrainType.AIMonster:
               //네비매쉬
                break ;
            case BrainType.Fsm:
                var chaseBrain = go.GetComponent<ChaseBrainFSM>() ?? go.AddComponent<ChaseBrainFSM>();
                break;
        }


        return enemy;
       
    }



    //스폰은 해당 펙토리 기능을 가져가서 그저 생성만 하도록 구현할예정

    
}

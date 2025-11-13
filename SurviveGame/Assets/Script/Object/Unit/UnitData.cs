using System;
using System.Collections;
using UnityEngine;



[Serializable]
[CreateAssetMenu(menuName = "Units/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject prfabs;

    [Header("State")]
    public string name;
    public int maxHp;
    public int power;
    public int defense;
    public float jumpForce;


    [Header("Behaviors")]
    public BrainType brainType; //유닛의 인공지능 타입
    public MoverType moveType; //이동방식
    public AttackType attackType; //공격 방식

    [Header("Move")]
    public float moveSpeed;


}


public enum BrainType { Player, AIMonster, Fsm, None}
public enum MoverType { CharacterController, Rigidbody, NavMesh , None/*just transform*/ }
public enum AttackType { Melee, Ranged, None }







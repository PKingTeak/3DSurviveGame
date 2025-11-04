using System;
using System.Collections;
using UnityEngine;



[Serializable]
[CreateAssetMenu(menuName = "Units/UnitData")]
public class UnitData : ScriptableObject
{
    public string name;
    public int maxHp;
    public int power;
    public int defense;
    public float moveSpeed;
    public float jumpForce;

    public BrainType brainType; //유닛의 인공지능 타입
    public MoverType moveType; //이동방식
    public AttackType attackType; //공격 방식



}


public enum BrainType { Player, AIMonster, None}
public enum MoverType { CharacterController, Rigidbody, NavMesh }
public enum AttackType { Melee, Ranged, None }







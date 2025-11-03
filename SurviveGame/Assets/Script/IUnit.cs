using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class UnitBaseStats
{
    public string name;
    public int maxHp;
    public int power;
    public int defense;
    public float moveSpeed;
}


[CreateAssetMenu(menuName = "Units/UnitData")]
public class UnitData : ScriptableObject
{
    public UnitBaseStats baseStats;
    public BrainType brainType; //유닛의 인공지능 타입
    public MoverType moveType; //이동방식
    public AttackType attackType;
}

public enum BrainType { Player, AIMonster, None}
public enum MoverType { CharacterController, Rigidbody, NavMesh }
public enum AttackType { Melee, Ranged, None }


public class Unit : MonoBehaviour
{ 
    protected Unit()
    {
        
        unitData.name = "None";
        unitData.brainType = BrainType.None;
        unitData.moveType = MoverType.NavMesh;
        unitData.attackType = AttackType.None;
    }

    #region UnitInfo
    public UnitData unitData = new UnitData();
    #endregion

    #region Stats
    protected UnitBaseStats unitBaseStats;
    #endregion
    protected void Die()
    {
        Debug.Log($"{unitBaseStats.name}이(가) 사망했습니다.");
        Destroy(this.gameObject); //나중에 풀링 + 이펙트 추가
    }

    protected void Spawn(Vector3 postition)
    { 
        // 생성 로직 만들꺼임
    }




}




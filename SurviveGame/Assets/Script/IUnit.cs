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
    #region UnitInfo
    protected UnitData unitData;
    #endregion

    #region Stats
    protected string unitName { get; set; }
    protected int unitHp { get; set; }
    protected int unitMp { get; set; }

    protected int unitDefense { get; set; }
    protected int unitPower { get; set; } //스탯은 일단 하나로 통일
    protected int unitLevel { get; set; }
    #endregion
    protected void Die()
    {
        Debug.Log($"{unitName}이(가) 사망했습니다.");
        Destroy(this.gameObject); //나중에 풀링 + 이펙트 추가
    }

    protected void Spawn(Vector3 postition)
    { 
        // 생성 로직 만들꺼임
    }




}




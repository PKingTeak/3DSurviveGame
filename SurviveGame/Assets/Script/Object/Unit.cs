using System;
using Unity;
using UnityEditor;
using UnityEngine;

public class Unit : MonoBehaviour
{
    

  

    #region UnitInfo
    [Header("UnitData")]
    [SerializeField]
    private UnitData unitData;
    //unitBaseState -> UnitData 내부에 있음
    #endregion

    //프로퍼티 사용

    public string Name { get => unitData.name;}  
    public float MoveSpeed { get => unitData.moveSpeed; }
    public float JumpForce {  get => unitData.jumpForce; }



  
    protected void Die()
    {
        Debug.Log($"{unitData.name}이(가) 사망했습니다.");
        Destroy(this.gameObject); //나중에 풀링 + 이펙트 추가
    }

    protected void Spawn(Vector3 postition)
    {
        // 생성 로직 만들꺼임
    }




}
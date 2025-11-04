using UnityEngine;


public interface IAttacker // 공격하는 유닛
{
    bool canAttack { get; }
    void Attack(Unit target, int damage);
    // 제너릭으로 오브젝트도 데미지가 생긴다면 리펙토링 하면된다.

}

public interface IMover 
{
    float Speed { get; set; }
    void Move(Vector3 direction);
}


public interface  IBrain
{
    void Think();
    
}






using UnityEngine;
using Attacker;
using Brain;
using Mover;
using System;


public class Enemy : Unit
{

    public IMover imover { get; private set; }
    public IAttacker iattacker { get; private set; }
    public IBrain ibrain { get; private set; }


    public event Action<Enemy> IsDead;

    private bool dead;

    private EnemyManager mg;
    public void SettingManager(EnemyManager manager)
    {
        mg = manager;
    }


    public override void Init(UnitData data)
    {
        base.Init(data);

        imover = GetComponent<IMover>();
        ibrain = GetComponent<IBrain>();
        iattacker = GetComponent<IAttacker>();
    }


    private void Update()
    {
        ibrain.Think(); //생각하는 로직 아직 미구현
    }

    public void DIe()
    {
        if (dead) return;
        dead = true;
        IsDead?.Invoke(this);
        gameObject.SetActive(false); //나중에 풀링 할때 사용할 예정

    }






}

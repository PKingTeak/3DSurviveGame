using UnityEngine;
using Attacker;
using Brain;
using Mover;


public class Enemy : Unit
{
    public IMover imover { get; private set; }
    public IAttacker iattacker { get; private set; }
    public IBrain ibrain { get; private set; } 



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






}

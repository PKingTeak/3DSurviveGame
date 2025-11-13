using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class EnemyManager
{
    public EnemyManager(GameManager _gm)
    {
        gm = _gm;
    }



    GameManager gm;

    private readonly HashSet<Unit> _alive = new();

    //매니저는 단순 해당 유닛들이 살아있는지만 관리
    private int _currentWave;
    private int _totalThisWave;
    public int Alive => _alive.Count;
    public int Total => _totalThisWave;
    //근데 구지 waveIndex를 몰라도 되는거 아닌가?
    


   


    void Awake()
    {
        QueryBus<int>.Register(EventTopics.CurrentWave, () => _currentWave);
        QueryBus<int>.Register(EventTopics.AliveUnitCount, () => Alive);
        QueryBus<int>.Register(EventTopics.WaveTotal, () => Total);
    }

    public void OnWaveStart(int waveIndex, int total)
    {
        _alive.Clear();
        _currentWave = waveIndex;
        _totalThisWave = total;


        EventBus<int>.PublishAction(EventTopics.WaveStarted, _currentWave);
        EventBus<int>.PublishAction(EventTopics.WaveTotalSet, _totalThisWave );
        EventBus<int>.PublishAction(EventTopics.AliveCountChanged, Alive);

        
    }

    public void OnSpawned(Enemy enemy)
    {
        if (enemy == null) return;
        if (_alive.Add(enemy))
        {
            enemy.IsDead += OnDied;
            EventBus<int>.PublishAction(EventTopics.AliveCountChanged, Alive);
        }
    }

    public void OnDied(Enemy enemy)
    {
        if (enemy == null) return;
        if (_alive.Remove(enemy))
        {
            enemy.IsDead -= OnDied;
            EventBus<int>.PublishAction(EventTopics.AliveCountChanged, Alive);

            if (_alive.Count == 0)
            { 
            EventBus<int>.PublishAction(EventTopics.WaveClear, _currentWave);
            }
        }
    }


}

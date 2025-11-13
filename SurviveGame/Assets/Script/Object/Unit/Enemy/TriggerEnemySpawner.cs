using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Internal;
using System.Transactions;




public class TriggerEnemySpawner : MonoBehaviour
{

    [Serializable]
    public struct SpawnEntry
    {
        public UnitData data;
        public int count;
    }


    [SerializeField]
    private List<SpawnEntry> currentWave = new();

    // private Dictionary<int, List<Tuple<UnitData, int>>> waveUnitDic = new() ; //웨이브 , 생성유닛, 유닛수
    //딕셔너리가 비어있음 지금 상태

    [SerializeField]
    private List<Vector3> spawnPos = new();

    [SerializeField]
    private Collider triggerCol;

    [SerializeField]
    private int wave = 1;


    private int unitCount;
    private int total;
    private int maxWaveUnitCount;





    [SerializeField] private EnemyFactory enemyFactory; //직접 넣어주기 // 나중에 리펙토링
    private EnemyManager enemyManager;

    public void Awake()
    {
        enemyManager = GameManager.Instance.enemyManager;


    }







    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerCol) triggerCol.enabled = false;

        Debug.Log("플레이어가 접촉했습니다.");
        StartCoroutine(SpawnWave());

    }




    private IEnumerator SpawnWave()
    {


        if (currentWave == null || currentWave.Count == 0)
        {
            yield break;
        }

        total = 0;
        unitCount = 0;

        foreach (var pair in currentWave)
        {
            total += Mathf.Max(0, pair.count);

        }

        foreach (var pair in currentWave)
        {
            Debug.Log($"{pair.data} , {pair.count}");
        }

        enemyManager.OnWaveStart(1, total);

        foreach (var entry in currentWave)
        {
            UnitData data = entry.data;
            int count = Mathf.Max(0,entry.count);

            for (int i = 0; i < count; i++)
            {
                int spawnIndex = UnityEngine.Random.Range(0, spawnPos.Count);
                Vector3 pos = spawnPos.Count > 0 ? spawnPos[spawnIndex] : transform.position;

               var enemy = enemyFactory.Spawn(data, pos, null);

                if (enemy != null)
                {
                    enemyManager.OnSpawned(enemy);
                    unitCount++;
                    Debug.Log($"유닛 생성 {enemy.name}");
                }
                else
                {
                    Debug.Log("유닛 미생성");
                }
                //딜레이
               // 딜레이 원할때 yield return null;
            }


            yield return new WaitUntil(() => unitCount  >= total);

            
        }

    }


    /*
     *
트리거 작동하면 웨이브 생성하고 막기 

필요 변수 
웨이브 , 유닛(정보) , 유닛갯수, 이벤트액션

몬스터 생성 위치 
    스폰 위치 생성기 둘까 아니면 위치로 잡을까
    모든 정보를 가지고 직접 생성 or 생성된 애를 그냥 배치만 할까?
     */
}

using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint = new Vector3();
    [SerializeField] GameObject TestUnit;
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Instantiate(TestUnit, spawnPoint, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

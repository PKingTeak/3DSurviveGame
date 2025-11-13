using UnityEngine;

public static class EventTopics
{

    //웨이브 관련 카운트
    public const string WaveStarted = "WaveStarted";        // EventBus<int> (waveIndex)
    public const string WaveTotalSet = "WaveTotalSet";       // EventBus<int> (total)
    public const string AliveCountChanged = "AliveCountChanged";  // EventBus<int> (alive)
    public const string WaveClear = "WaveClear";


    //질의(현재 상태)
    public const string AliveUnitCount = "AliveUnitCount";
    public const string CurrentWave = "CurrentWave";
    public const string WaveTotal = "WaveTotal";

    

}

using System.Collections.Generic;
using UnityEngine;

public interface IPlayerProvider
{ 
    IReadOnlyList<Transform> Players { get; }
    Transform GetClosest(Vector3 fromPos);
    Transform GetClosest(Vector3 fromPos, float maxRadius);
    Transform GetByID(int id);
}

public class PlayerRegistry : MonoBehaviour , IPlayerProvider
{
    private readonly List<Transform> players = new();
    public IReadOnlyList<Transform> Players => players;


    public void Register(Transform transform)
    {
        if (transform && !players.Contains(transform))
        {
            players.Add(transform); 
        }
    }

    public void UnRegister(Transform transform)
    {
        players.Remove(transform);
    }

    public Transform GetClosest(Vector3 fromPos)
    {
        Transform best = null;
        float bestSqr = float.PositiveInfinity;//짧은걸 저장

        for (int i = 0; i < players.Count; i++)
        {
            var p = players[i];
            if (!p)
            {
                continue;
            }

            float distance = (p.position - fromPos).sqrMagnitude;
            if (distance < bestSqr)
            {
                bestSqr = distance;
                best = p;
            }


        }
        return best; 
        //제일 가까운 플레이어를 리턴할꺼임
    }
    public Transform GetClosest(Vector3 fromPos, float maxRadius)
    {
        Transform best = null;
        float bestSqr = maxRadius * maxRadius; ;

        for (int i = 0; i < players.Count; i++)
        {
            var p = players[i];
            if (!p)
            {
                continue;
            }

            float distance = (p.position - fromPos).sqrMagnitude;
            if (distance < bestSqr)
            {
                bestSqr = distance;
                best = p;
            }


        }

        return best;

    }


    public Transform GetByID(int ID)
    {
        return null;        
    }

    
}

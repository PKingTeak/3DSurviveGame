using Unity.VisualScripting;
using UnityEngine;

public class Player : Unit
{
 
    public void GetUnitState()
    {
        string name = unitBaseStats.name;
        Debug.Log($"{unitBaseStats.name}");
    }

}

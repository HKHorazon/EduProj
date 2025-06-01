using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [field:SerializeField] public GameMap gameMap {  get; private set; }

    private static GameManager mInstance = null;
    public static GameManager Instance
    {
        get
        {
            return mInstance;
        }
    }

    void Start()
    {
        mInstance = this;
    }
}

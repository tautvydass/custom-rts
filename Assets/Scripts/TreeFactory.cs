using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Tree Factory", menuName = "RTS/Map/Tree Factory")]
public class TreeFactory : ScriptableObject
{
    private Func<GameObject, GameObject> Instantiate;

    public List<GameObject> treePrefabs;

    public void SetupInstantiateCall(Func<GameObject, GameObject> instantiate)
    {
        Instantiate = instantiate;
    }

    public GameObject CreateTree()
    {
        return Instantiate(treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Count - 1)]);
    }
}

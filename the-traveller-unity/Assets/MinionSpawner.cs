using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public Transform minionParentObject;
    public Transform playerTarget;
    public GameObject minionPrefab;
    public List<Transform> leftSpawnPoints;
    public List<Transform> rightSpawnPoints;
    public List<GameObject> spawnedMinions;
    [SerializeField] float minionSpawnRate;
    [SerializeField] int maxMinionsAlive;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMinions", minionSpawnRate, minionSpawnRate);
    }

    /// <summary>
    /// Spawns 1 enemy on left and 1 on right 
    /// </summary>
    void SpawnMinions()
    {
        CleanMinionList();
        SpawnMinionAtLocation(leftSpawnPoints[Random.Range(0, leftSpawnPoints.Count)].position);
        SpawnMinionAtLocation(rightSpawnPoints[Random.Range(0, leftSpawnPoints.Count)].position);
    }

    void SpawnMinionAtLocation(Vector3 positionToSpawn)
    {
        if (spawnedMinions.Count >= maxMinionsAlive) return;
        GameObject newMinion = Instantiate(minionPrefab, positionToSpawn, Quaternion.identity);
        newMinion.transform.parent = minionParentObject;
        newMinion.GetComponent<MinionController>().SetPathTarget(playerTarget);
        spawnedMinions.Add(newMinion);
    }

    void CleanMinionList()
    {
        for (int i = spawnedMinions.Count - 1; i > -1; i--)
        {
            if (spawnedMinions[i] == null)
            {
                spawnedMinions.RemoveAt(i);
            }
        }
    }
}

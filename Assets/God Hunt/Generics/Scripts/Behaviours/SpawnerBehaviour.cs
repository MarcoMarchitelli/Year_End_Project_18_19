using UnityEngine;

public class SpawnerBehaviour : BaseBehaviour
{
    public SpawnData[] spawnData;

    public void Spawn(int _index)
    {
        for (int i = 0; i < spawnData.Length; i++)
        {
            if (i == _index)
            {
                SpawnData s = spawnData[i];
                //if (s.spawnAsChild)
                //    Instantiate(s.prefab, s.spawnPoint);
                //else
                {
                    Instantiate(s.prefab, s.spawnPoint.position, s.useSpawnRotation ? s.spawnPoint.rotation : Quaternion.identity);
                }
            }
        }
    }

    public void SpawnAll()
    {
        for (int i = 0; i < spawnData.Length; i++)
        {
            SpawnData s = spawnData[i];
            //if (s.spawnAsChild)
            //    Instantiate(s.prefab, s.spawnPoint);
            //else
            {
                Instantiate(s.prefab, s.spawnPoint.position, s.useSpawnRotation ? s.spawnPoint.rotation : Quaternion.identity);
            }
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public GameObject prefab;
    public Transform spawnPoint;
    //public bool spawnAsChild;
    [Tooltip("If true spawns with the same rotation as the spawnPoint.")]
    public bool useSpawnRotation;
}
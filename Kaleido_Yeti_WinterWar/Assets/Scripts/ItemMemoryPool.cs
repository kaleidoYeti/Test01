using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject itemSpawnPointPrefab;
    [SerializeField]
    private float itemSpawnTime = 10;

    private MemoryPool spawnPointMemoryPool;

    private int numberOfItemsSpawnedAtOnce = 4;

    private Vector2Int mapSize = new Vector2Int(100, 100);

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(itemSpawnPointPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        while (true)
        {
            for (int i = 0; i < numberOfItemsSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivatePoolItem();
                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f), 1, Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f));

                StartCoroutine("CheckIfHadItem", item);
            }

            yield return new WaitForSeconds(itemSpawnTime);
        }
    }

    private IEnumerator CheckIfHadItem()
    {
        //������ ���� ��� -> Collider / Input
        //ó������ -> UI����

        //������ �� �԰� 10�� �帥 ���
        //spawnPointMemoryPool.DeactivatePoolItem(point);

        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMemoryPool : MonoBehaviour
{
    [SerializeField]
    private float range;

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject itemSpawnPointPrefab;
    [SerializeField]
    private float itemSpawnTime = 10;

    private MemoryPool spawnPointMemoryPool;

    private int numberOfItemsSpawnedAtOnce = 4;
    private float sec = 10.0f;

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

    private IEnumerator CheckIfHadItem(GameObject point)
    {
        float t = 0.0f;
        while (t < sec)
        {
            t += Time.deltaTime;

            //아이템 먹은 경우 (Collider 충돌하며/거리가 가까우며 특정 키(tab이나 E) 눌렀을 경우)
            if (CheckItem() && Input.GetKeyDown(KeyCode.E))
            {
                //랜덤으로 아이템 종류(0~5) 중 하나 고르고, 
                //Item item;
                int a = Random.Range(0, 6);
                //화면에 그림 띄우면서 item 관리
                ItemInfoAppear();
            }
            yield return null;
        }

        //아이템 안 먹고 10초 흐른 경우
        spawnPointMemoryPool.DeactivatePoolItem(point);

    }

    private bool CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                return true;
            }
        }
        return false;
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
    }
}

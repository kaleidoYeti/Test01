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

            //������ ���� ��� (Collider �浹�ϸ�/�Ÿ��� ������ Ư�� Ű(tab�̳� E) ������ ���)
            if (CheckItem() && Input.GetKeyDown(KeyCode.E))
            {
                //�������� ������ ����(0~5) �� �ϳ� ����, 
                //Item item;
                int a = Random.Range(0, 6);
                //ȭ�鿡 �׸� ���鼭 item ����
                ItemInfoAppear();
            }
            yield return null;
        }

        //������ �� �԰� 10�� �帥 ���
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

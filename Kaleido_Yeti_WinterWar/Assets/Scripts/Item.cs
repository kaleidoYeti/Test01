using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName { Heal = 0, SnowBallMaker, Booster, Goggles, Skeleton, Boom }
/*Item
 * Heal             (HP ȸ��)
 * SnowBallMaker    (������ ���� �ð� ����)
 * Booster          (�̵� �ӵ� ����)
 * Goggles          (�þ� Ȯ�� = ���� ���� ���� ����)
 * Skeleton         (����Ű �ݴ��? �Ǵ� �̵� �ӵ� ����)
 * Boom             (HP ����)
 */

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemName itemName;

    public Sprite itemImage;
    public GameObject itemPrefab;

}


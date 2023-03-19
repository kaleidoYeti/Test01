using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName { Heal = 0, SnowBallMaker, Booster, Goggles, Skeleton, Boom }
/*Item
 * Heal             (HP 회복)
 * SnowBallMaker    (눈덩이 생성 시간 단축)
 * Booster          (이동 속도 증가)
 * Goggles          (시야 확보 = 적의 공격 궤적 보임)
 * Skeleton         (방향키 반대로? 또는 이동 속도 저하)
 * Boom             (HP 감소)
 */

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemName itemName;

    public Sprite itemImage;
    public GameObject itemPrefab;

}


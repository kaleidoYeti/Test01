using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipFire;

    [Header("Grenade")]
    [SerializeField]
    private GameObject grenadePrefab;
    [SerializeField]
    private Transform grenadeSpawnPoint;

    private void OnEnable()
    {
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public void Awake()
    {
        base.Setup();

        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type==0 && isAttack==false && weaponSetting.currentAmmo > 0)
        {
            StartCoroutine("OnAttack");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {

    }

    public override void StartReload()
    {

    }


    private IEnumerator OnAttack(int type)
    {
        isAttack = true;

        //animator.Play("Fire", -1, 0);
        
        PlaySound(audioClipFire);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            //if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            if (audioSource.isPlaying == false)
            {
                isAttack = false;
                yield break;
            }

            yield return null;
        }
    }

    //애니메이션 이벤트 함수 (애니메이션이 있어야 제대로 작동함)
    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Random.rotation);
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward);

        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
}

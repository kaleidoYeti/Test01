using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipHit;

    [SerializeField]
    private WeaponKnifeCollider weaponKnifeCollider;

    private void OnEnable()
    {
        isAttack = false;
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
        if (isAttack) return;

        if (weaponSetting.isAutomaticAttack)
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else
        {
            StartCoroutine("OnAttack", type);
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
        StopCoroutine("OnAttackLoop");
    }

    public override void StartReload()
    {

    }

    private IEnumerator OnAttackLoop(int type)
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", type);
        }
    }

    private IEnumerator OnAttack(int type)
    {
        isAttack = true;

        //animator.SetFloat("attackType", type);
        //animator.Play("Fire", -1, 0);

        PlaySound(audioClipHit);

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
    public void StartWeaponKnifeCollider()
    {
        weaponKnifeCollider.StartCollider(weaponSetting.damage);
    }
}

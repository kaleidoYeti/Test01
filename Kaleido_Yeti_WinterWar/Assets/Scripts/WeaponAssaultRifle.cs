using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent: UnityEngine.Events.UnityEvent<int> { }
*/

//public class WeaponAssaultRifle : MonoBehaviour
public class WeaponAssaultRifle : WeaponBase
{
    /*
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();
    */

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint;
    [SerializeField]
    private Transform bulletSpawnPoint;


    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip audioClipFire;
    [SerializeField]
    private AudioClip audioClipReload;

    /*
    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting;
    */

    [Header("Aim UI")]
    [SerializeField]
    private Image imageAim;

    /*
    private float lastAttackTime = 0;
    private bool isReload = false;
    private bool isAttack = false;
    */
    private bool isModeChange = false;
    //private float defaultModeFOV = 60;
    //private float aimModeFOV = 30;

    /*
    private AudioSource audioSource;
    //private PlayerAnimatorController animator;
    */
    private CasingMemoryPool casingMemoryPool;
    private ImpactMemoryPool impactMemoryPool;
    private Camera mainCamera;

    /*
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;
    */

    private void Awake()
    {
        base.Setup();

        /*audioSource = GetComponent<AudioSource>();
        //animator = GetComponentInParent<PlayerAnimatorController>();*/
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;

        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;      //
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);

        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (isReload) return;
        if (isModeChange) return;

        if (type == 0)  //좌클릭
        {
            if (weaponSetting.isAutomaticAttack)
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
        else            //우클릭
        {
            if (isAttack) return;

            //StartCoroutine("OnModeChange");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public override void StartReload()
    {
        if (isReload || weaponSetting.currentMagazine <= 0) return;
        StopWeaponAction();
        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            //if (animator.MoveSpeed > 0.5f) return;

            lastAttackTime = Time.time;

            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }

            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            //string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            //animator.Play(animation, -1, 0);
            //if (animator.AimModeIs == false)
            StartCoroutine("OnMuzzleFlashEffect");

            PlaySound(audioClipFire);

            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            TwoStepRaycast();
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        //animator.OnReload();
        PlaySound(audioClipReload);

        while (true)
        {
            //if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            if (audioSource.isPlaying == false)
            {
                isReload = false;
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red); //

        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
            if (hit.transform.CompareTag("ImpactEnemy"))
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(weaponSetting.damage);
            }
            else if (hit.transform.CompareTag("InteractionObject"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    /*
    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        animator.AimModeIs = !animator.AimModeIs;
        imageAim.enabled = !imageAim.enabled;

        float start = mainCamera.fieldOfView;
        float end = animator.AimModeIs == ture ? aimModeFOV : defaultModeFOV;
        
        isModeChange = true;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }
        isModeChange = false;

    }
    */

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }

    /*
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
    */
}

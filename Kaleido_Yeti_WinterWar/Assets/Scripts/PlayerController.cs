using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;
    [SerializeField]
    private KeyCode keyCodeUseItem = KeyCode.Tab;
    
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;

    private RotateToMouse rotateToMouse;
    private MovementCharacterController movement;
    private Status status;
    private AudioSource audioSource;

    //private WeaponAssaultRifle weapon;
    private WeaponBase weapon;
    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        audioSource = GetComponent<AudioSource>();
        /*weapon = GetComponentInChildren<WeaponAssaultRifle>();*/
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
        UpdateUseItem();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x!=0 || z != 0)
        {
            bool isRun = false;
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            //weapon.Animator.MoveSpeed = isRun == true ? 1 : 0.5f
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            movement.MoveSpeed = 0;
            //weapon.Animator.MoveSpeed = 0;

            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }

    private void UpdateWeaponAction()
    {
        //좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        //우클릭
        if (Input.GetMouseButtonDown(1))
        {
            weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }

        //R키
        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }

    private void UpdateUseItem()
    {
        if (Input.GetKeyDown(keyCodeUseItem))
        {
            //첫번째 아이템 사용
        }
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);
        if (isDie)
        {
            Debug.Log("GameOver");
            //씬 전환
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
}

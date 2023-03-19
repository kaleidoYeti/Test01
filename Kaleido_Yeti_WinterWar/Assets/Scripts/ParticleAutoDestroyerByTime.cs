using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyerByTime : MonoBehaviour
{
    //���� ���� ���� �巳�� ��ġ ��, �޸� Ǯ ����

    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!particle.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}

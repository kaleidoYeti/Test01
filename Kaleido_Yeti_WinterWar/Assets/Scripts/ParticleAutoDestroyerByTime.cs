using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyerByTime : MonoBehaviour
{
    //많은 수의 폭발 드럼통 배치 시, 메모리 풀 적용

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

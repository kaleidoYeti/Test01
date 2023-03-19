using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 4;
    private MeshRenderer meshRenderer;

    [SerializeField]
    private float moveDistance = 0.2f;
    [SerializeField]
    private float pingpongSpeed = 0.5f;
    [SerializeField]
    private float rotateSpeed = 50;

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while (true)
        {
            InfiniteLoopDetector.Run();
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            Vector3 position = transform.position;
            position.y = Mathf.Lerp(y, y + moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("OnFadeEffect");
    }

    private void OnDisable()
    {
        StopCoroutine("OnFadeEffect");
    }

    private IEnumerator OnFadeEffect()
    {
        //돌아가는 효과?
        //yield return new WaitForSeconds(6.0f);

        while (true)
        {
            InfiniteLoopDetector.Run();
            Color color = meshRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * fadeSpeed, 1));
            meshRenderer.material.color = color;

            yield return null;
        }
    }
}

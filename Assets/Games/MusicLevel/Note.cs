using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public MusicLevelManager musicLevelManager;
    public float speed = 1.0f;
    public float bottomY = -5.0f;

    public bool isHandled = false;

    public Transform JudgeLine;
    public float HandleDistance = 2f;
    public float PerfectDistance = 1.5f;
    public float WowDistance = 0.5f;

    public GameObject ExplosionPrefab;

    private bool isMissed = false;

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        if (transform.position.y < bottomY && !isMissed)
        {
            musicLevelManager.tipSprite = musicLevelManager.missSprite;
            Debug.Log("Miss");
            musicLevelManager.BloodBar.fillAmount -= 0.25f;
            isMissed = true;
            Destroy(gameObject);
        }

        if (isHandled)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        // 确保Note未被处理且在处理距离范围内
        if (isHandled || Mathf.Abs(transform.position.y - JudgeLine.position.y) > HandleDistance)
        {
            return;
        }

        GameObject Explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(Explosion, 1.0f);

        isHandled = true;
        Debug.Log("Clicked on Note");
        Judge();
    }

    void Judge()
    {
        float distance = Mathf.Abs(transform.position.y - JudgeLine.position.y);
        if (distance < WowDistance)
        {
            musicLevelManager.score += 300;
            musicLevelManager.tipSprite = musicLevelManager.perfectSprite;
            Debug.Log("Wow");
        }
        else if (distance < PerfectDistance)
        {
            musicLevelManager.score += 200;
            musicLevelManager.tipSprite = musicLevelManager.goodSprite;
            Debug.Log("Perfect");
        }
        else
        {
            musicLevelManager.score += 100;
            musicLevelManager.tipSprite = musicLevelManager.hitSprite;
            Debug.Log("Great");
        }
    }
}
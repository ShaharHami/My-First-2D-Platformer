using System.Collections.Generic;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform startingPlatform;
    [SerializeField] private List<Transform> platforms;
    private Transform player;
    private Transform platform;
    private const float PLAYER_DISTANCE_SPAWN_PLATFORM = 50f;
    private float spriteWidth = 0f;
    private float spriteHeight = 0f;
    private Transform lastRightPlatform;
    private Transform lastLeftPlatform;
    private bool searchingForPlayer = false;
    void Awake()
    {
        lastRightPlatform = SpawnLevelPart(SpawnPosition(startingPlatform, 1));
        lastLeftPlatform = SpawnLevelPart(SpawnPosition(startingPlatform, -1));
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Spawn();
        AstarPath.active.Scan();
    }
    void Update()
    {
        if (player != null)
        {
            Spawn();
        }
        else
        {
            StartCoroutine(SearchForPlayer());
        }
    }

    private void Spawn()
    {
        if (Mathf.Abs(lastRightPlatform.position.x - player.position.x) < PLAYER_DISTANCE_SPAWN_PLATFORM)
        {
            lastRightPlatform = SpawnLevelPart(SpawnPosition(lastRightPlatform, 1));
        }

        // TODO: Find out why this is causing performance issues

        if (Mathf.Abs(lastLeftPlatform.position.x - player.position.x) < PLAYER_DISTANCE_SPAWN_PLATFORM)
        {
            lastLeftPlatform = SpawnLevelPart(SpawnPosition(lastLeftPlatform, -1));
        }
    }

    private Vector3 SpawnPosition(Transform previousPart, int dir)
    {
        platform = RandomPlatform();
        SpriteRenderer sRenderer = platform.GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
        spriteHeight = sRenderer.sprite.bounds.size.y;
        float OffsetX = 0f;
        float rnd = Random.Range(0.1f, spriteWidth * 2);
        if (dir == 1)
        {
            OffsetX = previousPart.position.x + (rnd + spriteWidth);
        }
        else if (dir == -1)
        {
            OffsetX = previousPart.position.x - (rnd + spriteWidth);
        }
        float OffsetY = Random.Range(-3f, 3f) - (spriteHeight / 2);
        Vector3 spawnPos = new Vector3(OffsetX, OffsetY, 0);
        return spawnPos;
    }
    private Transform SpawnLevelPart(Vector3 position)
    {
        Transform levelPartTransform = Instantiate(platform, position, Quaternion.identity);
        return levelPartTransform;
    }
    private Transform RandomPlatform()
    {
        return platforms[Random.Range(0, platforms.ToArray().Length)];
    }
    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchingForPlayer = false;
            player = sResult.transform;
            yield return false;
        }
    }
}
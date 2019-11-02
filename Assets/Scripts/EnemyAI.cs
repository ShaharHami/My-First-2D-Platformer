﻿using UnityEngine;
using Pathfinding;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Path path;
    public float speed = 300f;
    public ForceMode2D fMode;
    // [HideInInspector]
    public bool pathIsEnded = false;
    public float nextWayPointDistance = 3;
    private int currentWayPoint = 0;
    private bool searchingForPlayer = false;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine (SearchForPlayer());
            }
            return;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }
    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds (0.5f);
            StartCoroutine (SearchForPlayer());
        } else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine (UpdatePath());
            yield return false;
        }
    }
    IEnumerator UpdatePath()
    {
        if (target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
        else
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine (SearchForPlayer());
            }
            return;
        }
        //TODO: Always look at player
        if (path == null)
        {
            return;
        }
        if(currentWayPoint >= path.vectorPath.Count)
        {
            if(pathIsEnded)
            {
                return;
            }
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //Find direction to next waypoint
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce (dir, fMode);
        float dist = Vector3.Distance (transform.position, path.vectorPath[currentWayPoint]);
        if (dist < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }
}

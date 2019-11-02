using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsFollow : MonoBehaviour
{
    [SerializeField] Transform whatToFollow;
    [SerializeField] bool freezeX = false;
    [SerializeField] bool freezeY = false;
    private float followX;
    private float setX;
    private float followY;
    private float setY;
    private float setZ;
    // Start is called before the first frame update
    void Start()
    {
        setX = transform.position.x;
        setY = transform.position.y;
        setZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (freezeX)
        {
            followX = setX;
        }
        else
        {
            followX = whatToFollow.position.x;
        }
        if (freezeY)
        {
            followY = setY;
        }
        else
        {
            followY = whatToFollow.position.y;
        }
        transform.position = new Vector3(followX, followY, setZ);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlepointObj : MonoBehaviour
{
    //public GameObject networkManager;
    //public string owner;
    //public string name;

    public GameObject obj;
   // public GameObject startPoint;
    public GameObject idlePoint;
    public CapsuleCollider CapsuleCollider;

    public List<Transform> idlepoints;
    public Transform targetIdlepoint;
    private int targetIdlepointIndex = 0;
    private int lastIdlepointIndex;
    private float minDistance = 0.1f;


    private float movementSpeed = 1f;
    private float rotationSpeed = 8f;

    private void Awake()
    {
        CapsuleCollider = obj.GetComponentInChildren<CapsuleCollider>();
        //idlepoints = idlePoint.GetComponent<IdlepointController>().idlepoints;
        lastIdlepointIndex = idlepoints.Count - 1;
        targetIdlepoint = idlepoints[targetIdlepointIndex];
    }
    

    private void FixedUpdate()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetIdlepoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);
        float distance = Vector3.Distance(transform.position, targetIdlepoint.position);
       CheckDistanceToIdlepoint(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetIdlepoint.position, movementStep);
    }

    public void CheckDistanceToIdlepoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            //int random = Random.Range(0, 12);
            targetIdlepointIndex++;
            UpdateTargetIdlepoint();
        }
    }

    public void UpdateTargetIdlepoint()
    {
        if (targetIdlepointIndex > lastIdlepointIndex)
        {
            targetIdlepointIndex = 0;
        }
        targetIdlepoint = idlepoints[targetIdlepointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        NPC npc = other.gameObject.GetComponent<NPC>();
        if (npc != null)
        {
            npc.Idle = true;
            //npc.idleTime = ;
            //Debug.Log("HP" + npc.health);
           // Debug.Log("멈춰! " + npc.name);
        }
    }
}

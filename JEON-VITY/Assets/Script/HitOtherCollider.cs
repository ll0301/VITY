using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOtherCollider : MonoBehaviour
{
    //GameObject somebody;
    public int damage=100;


    private void OnTriggerEnter(Collider other)
    {
        Player somebody = other.gameObject.GetComponent<Player>();
        NPC npc = other.gameObject.GetComponent<NPC>();
        //var somebody = GameObject.FindWithTag("enemy").GetComponent<Player>();

        if (somebody != null)
        { 
            somebody.health -= damage;
        }
        if (npc != null)
        {
            npc.health -= damage;
            Debug.Log("HP" + npc.health);
            Debug.Log("Hit " + npc.name);
        }
    }
}

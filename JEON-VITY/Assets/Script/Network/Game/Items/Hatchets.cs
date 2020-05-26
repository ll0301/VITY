using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchets : MonoBehaviour
{
    public GameObject hatchet;
    public bool flying;
    public int damage = 100;
    public string owner;


    private void Update()
    {
        if (flying)
        {
            transform.Rotate(new Vector3(5000f, 0f, 0f)*Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        OtherPlayer somebody = other.gameObject.GetComponent<OtherPlayer>();
        NPC npc = other.gameObject.GetComponent<NPC>();
        Player player = other.gameObject.GetComponent<Player>();

        if (other.tag.Equals("Ground"))
        {
            flying = false;
            //Debug.Log("땅에떨어짐 ");
        }


        if (flying)
        {
            if (player != null)
            {
                if (!player.names.Equals(owner))
                {
                    player.health -= damage;
                    player.hatchet2.SetActive(true);
                    Destroy(hatchet);
                }
            }
            //Debug.Log("날아가다 맞음");
            if (somebody != null)
            {
                if (!somebody.names.Equals(owner))
                {
                    somebody.health -= damage;
                    somebody.hatchet2.SetActive(true);
                    Destroy(hatchet);
                }

            }

            if (npc != null)
            {
                npc.health -= damage;
                npc.hatchet2.SetActive(true);
                Destroy(hatchet);
                Debug.Log("HP" + npc.health);
                Debug.Log("Hit " + npc.name);
            }
        }

    }

}

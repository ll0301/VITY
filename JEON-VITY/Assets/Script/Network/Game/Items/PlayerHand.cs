using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public GameObject character;
    public GameObject Hatchet;
    public GameObject HatchetDrop;
    public GameObject handCollider;
    public bool grabItem;
    public string currentItem;

    public bool player;

    float itemThrowingForce = 1000f;

    //Vector3 vec;
    private void Awake()
    {

        grabItem = false;
        currentItem = "";
        handCollider.SetActive(false);

        //if (character.name.Equals("Character(Clone)"))
        //    player = true;
        //else
        //    player = false;

        //Debug.Log(player);
    }

    // 아이템 이 손에 닿으면 
    private void OnTriggerEnter(Collider other)
    {

            if (other.tag.Equals("Hatchet"))
            {
             // Debug.Log("도끼 잡았다!");
                Destroy(other.gameObject);
                Hatchet.SetActive(true);
                handCollider.SetActive(false);
                currentItem = "Hatchet";
                grabItem = true;
                character.GetComponent<Player>().currentItem = currentItem;
                character.GetComponent<Player>().grabItem = grabItem;
          //  Debug.Log(character.GetComponent<Player>().currentItem);
           // Debug.Log(character.GetComponent<Player>().grabItem);
        }
     }

    // 아이템 버리기 
    public void DropItem(string item)
    {
        if (item.Equals("Hatchet"))
        {
            GameObject Hatc = Instantiate(HatchetDrop, Hatchet.transform.position, Quaternion.identity);
            Hatchet.SetActive(false);
            handCollider.SetActive(false);
            Hatc.GetComponent<Rigidbody>().AddForce(character.transform.localRotation * Vector3.forward * 50);
            grabItem = false;
            currentItem = "";
            character.GetComponent<Player>().currentItem = currentItem;
            character.GetComponent<Player>().grabItem = grabItem;
           // Debug.Log(character.GetComponent<Player>().currentItem);
           // Debug.Log(character.GetComponent<Player>().grabItem);
        }
    }

    //아이템던지기 
    public void ThrowItem(string names)
    {
        if (currentItem.Equals("Hatchet"))
        {
            Hatchet.SetActive(false);
            handCollider.SetActive(false);
            grabItem = false;
            currentItem = "";
            character.GetComponent<Player>().currentItem = currentItem;
            character.GetComponent<Player>().grabItem = grabItem;
            //Debug.Log(character.GetComponent<Player>().currentItem);
            //Debug.Log(character.GetComponent<Player>().grabItem);
            GameObject Hatc = Instantiate(HatchetDrop, Hatchet.transform.position, Quaternion.identity);
            Hatc.GetComponent<Hatchets>().flying = true;
            Hatc.GetComponent<Hatchets>().owner = names;
            //if (player)
            //Hatc.GetComponent<Rigidbody>().AddForce(character.transform.forward * itemThrowingForce);
            //else
            Hatc.GetComponent<Rigidbody>().AddForce(character.transform.localRotation * Vector3.forward * itemThrowingForce);
        }
    }

}

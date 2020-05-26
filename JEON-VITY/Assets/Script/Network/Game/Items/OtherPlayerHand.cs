using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerHand : MonoBehaviour
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
        handCollider.SetActive(false);
        grabItem = false;
        currentItem = "";
      //  handCollider.SetActive(false);

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
            Debug.Log("다른 플레이어 도끼 잡았다");
            Destroy(other.gameObject);
            Hatchet.SetActive(true);
            handCollider.SetActive(false);
            character.GetComponent<OtherPlayer>().currentItem = "Hatchet";
            character.GetComponent<OtherPlayer>().grabItem = true;
        }
        // currentItem = "Hatchet";
        // grabItem = true;
        //Debug.Log(character.GetComponent<OtherPlayer>().currentItem);
        //Debug.Log(character.GetComponent<OtherPlayer>().grabItem);
    }

    // 아이템 버리기 
    public void DropItem()
    {
        Debug.Log("다른플레이어버림");
        GameObject Hatc = Instantiate(HatchetDrop, Hatchet.transform.position, Quaternion.identity);
        Hatchet.SetActive(false);
        handCollider.SetActive(false);
        Hatc.GetComponent<Rigidbody>().AddForce(character.transform.localRotation * Vector3.forward * 50);
        //grabItem = false;
        //currentItem = "";
        //character.GetComponent<OtherPlayer>().currentItem = currentItem;
        //character.GetComponent<OtherPlayer>().grabItem = grabItem;
        //Debug.Log(character.GetComponent<OtherPlayer>().currentItem);
        //Debug.Log(character.GetComponent<OtherPlayer>().grabItem);
    }

    //아이템던지기 
    public void ThrowItem(string names)
    {

            Debug.Log("다른플레이어 던짐");
            Hatchet.SetActive(false);
            handCollider.SetActive(false);
        // grabItem = false;
        //currentItem = "";
        //character.GetComponent<OtherPlayer>().currentItem = currentItem;
        //character.GetComponent<OtherPlayer>().grabItem = grabItem;
        //Debug.Log(character.GetComponent<OtherPlayer>().currentItem);
        //Debug.Log(character.GetComponent<OtherPlayer>().grabItem);
        GameObject Hatc = Instantiate(HatchetDrop, Hatchet.transform.position, Quaternion.identity);
            Hatc.GetComponent<Hatchets>().flying = true;
            Hatc.GetComponent<Hatchets>().owner = names;
            //if (player)
            //Hatc.GetComponent<Rigidbody>().AddForce(character.transform.forward * itemThrowingForce);
            //else
            Hatc.GetComponent<Rigidbody>().AddForce(character.transform.localRotation * Vector3.forward * itemThrowingForce);
        
    }
}

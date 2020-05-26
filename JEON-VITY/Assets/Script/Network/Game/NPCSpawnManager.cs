using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    public GameObject NPC; //npc 
    public GameObject spawnA; // 게임시작 포지션 
    public GameObject spawnB; //
    public GameObject spawnC; // 
    public GameObject spawnD;

    public string myPoint;
    public string otherPoint;

    public void NPCSpawn(string owner, string position, int group, bool isOwner)
    {
        //개인전일때 
        if (group == 2)
        {
            if (position.Equals("A"))
            {
                //spawnA.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "C" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "C";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("B"))
            {
                //  spawnB.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;

                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "D" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "D";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("C"))
            {
                //    spawnC.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }

                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "A" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "A";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("D"))
            {
                // spawnD.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프

                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "B" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "B";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
            }
        }

        // 단체전일때 
        if (group == 4)
        {
            if (position.Equals("A"))
            {
                //spawnA.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("B"))
            {
                //  spawnB.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;

                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("C"))
            {
                //    spawnC.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("D"))
            {
                // spawnD.SetActive(true);  // 포인트가 생성되고 
                myPoint = position;
                // for 루프

                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void OtherNPCSpawn(string owner, string position, int group, bool isOwner)
    {
        //개인전일때
        if (group == 2)
        {
            if (position.Equals("A"))
            {
                //spawnA.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "C" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "C";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("B"))
            {
                //spawnB.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "D" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "D";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("C"))
            {
                //spawnC.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "A" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "A";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("D"))
            {
                //    spawnD.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
                // for 루프
                for (int i = 0; i < 5; i++)
                {
                    NPC.name = "B" + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = "B";
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
            }
        }
        // 단체전일때 
        if (group == 4)
        {
            if (position.Equals("A"))
            {
                //spawnA.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnA.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z += i;
                    spawnA.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnA.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("B"))
            {
                //spawnB.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnB.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z += i;
                    spawnB.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnB.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("C"))
            {
                //spawnC.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnC.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x -= i;
                    vector.z -= i;
                    spawnC.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnC.transform.position, Quaternion.identity);
                }
            }
            else if (position.Equals("D"))
            {
                //    spawnD.SetActive(true);  // 포인트가 생성되고 
                otherPoint = position;
                // for 루프
                for (int i = 0; i < 4; i++)
                {
                    NPC.name = position + i;
                    NPC.GetComponent<NPC>().names = NPC.name;
                    NPC.GetComponent<NPC>().ownerName = owner;
                    NPC.GetComponent<NPC>().position = position;
                    NPC.GetComponent<NPC>().isOwner = isOwner;
                    Vector3 vector; //새 변수 만듬
                    vector = spawnD.transform.position; //새 변수에 현재 위치 넣어줌
                    vector.x += i;
                    vector.z -= i;
                    spawnD.transform.position = vector; //현재 위치에 변수를 넣어줌
                    Instantiate(NPC, spawnD.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
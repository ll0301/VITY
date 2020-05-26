using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveManager : MonoBehaviour
{
    protected Animator Animator;

    public int choice;

    private void Awake()
    {
        choice = 0;
        Animator = GetComponentInChildren<Animator>();
    }

    public void CharacterMoveRight()
    {
        choice++;
        if (choice == 4)
            choice = 0;

        Animator.SetInteger("Choice", choice);
        //Debug.Log(choice);
    }

    public void ChracterMoveLeft()
    {
        if (choice > 0)
            choice--;
        else if (choice == 3)
            choice = 0;
        else if (choice == 0)
            choice = 3;

        Animator.SetInteger("Choice", choice);
        //Debug.Log(choice);
    }
}

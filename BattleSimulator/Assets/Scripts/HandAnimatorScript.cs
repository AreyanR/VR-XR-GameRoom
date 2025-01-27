using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimatorScript : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    private Animator anim;

    private void start()
    {
        anim = GetComponent<Animator>();
    }

    private void update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();
        anim.SetFloat("Trigger", triggerValue);
        anim.SetFloat("Grip", gripValue);
    }


}

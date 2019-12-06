using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMotor))]
public class Unit : Interacteble
{
    UnitMotor motor;
    public Interacteble focus;

    public LayerMask movmentMask;

    private void Awake()
    {
        entity = true;
        motor = GetComponent<UnitMotor>();
        focus = null;
    }

    public void Command(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            
            //Set focus
            Interacteble interactable = hit.collider.GetComponent<Interacteble>();
            if(interactable != null)
            {
                SetFocus(interactable);
            } else
            {
                RemoveFocus();
                //Move entity to pos
                if (Physics.Raycast(ray, out hit, 100, movmentMask))
                    motor.MoveToPoint(hit.point);


            }
        }
    }

    void SetFocus(Interacteble newFocus)
    {
        if( newFocus != focus)
        {
            if(focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);

        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}

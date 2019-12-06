using UnityEngine;

public class Interacteble : MonoBehaviour
{
    public float radius = 3f;
    public string testName = "working?";
    public bool entity = false;

    public Transform interactionTransform;

    private bool isFocus;
    private Transform unit;

    bool hasInteracted = false;

    public virtual void Interact()
    {
        //To be overitten
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(unit.position, interactionTransform.position);
            if( distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform unitTransform)
    {
        isFocus = true;
        unit = unitTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        unit = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}

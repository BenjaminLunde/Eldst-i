using UnityEngine;


[CreateAssetMenu(fileName = "Undefined Target", menuName = "Focus/Target")]
public class Target : ScriptableObject
{
    new public string name = "Undefined";
    public string description = "This object has no description";
    public Sprite icon = null;
    
}

using UnityEngine;

public class binary_right_ItemAbility : MonoBehaviour
{
    [SerializeField] ItemAbility ability;
    public ItemAbility Ability => ability;
}
public enum ItemAbility { Double_Jump, High_Jump, None }
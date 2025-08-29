using System.Collections;
using UnityEngine;

public class binary_right_ItemRespawn : MonoBehaviour
{
    public void UseItem()
    {
        ActiveComponent(false);
    }
    public void Respawn()
    {
        ActiveComponent(true);
    }
    void ActiveComponent(bool option)
    {
        GetComponent<MeshRenderer>().enabled = option;
        GetComponent<Collider>().enabled = option;
    }
}

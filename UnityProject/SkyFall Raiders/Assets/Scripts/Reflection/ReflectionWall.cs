using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionWall : MonoBehaviour
{
    [SerializeField] Vector2 normal;
    public Vector3 normalV { get { return normal; } }
}

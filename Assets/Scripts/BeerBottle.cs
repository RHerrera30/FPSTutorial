using UnityEngine;
using System.Collections.Generic;

public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody> allParts = new();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }
    }
}

using UnityEngine;
using System.Linq;

// Example script using Hexagon
public class Example : MonoBehaviour
{
    void Start()
    {
        Debug.Log("0 " + Hexath.SnapNumberToStep(0.1f, 1));
        Debug.Log("0 " + Hexath.SnapNumberToStep(-0.1f, 1));
        Debug.Log("0 " + Hexath.SnapNumberToStep(-0.2f, 2));
        Debug.Log("0 " + Hexath.SnapNumberToStep(-0.9f, 2));
        Debug.Log("-2 " + Hexath.SnapNumberToStep(-1.2f, 2));
        Debug.Log("102 " + Hexath.SnapNumberToStep(101, 3));
        Debug.Log("-1 " + Hexath.SnapNumberToStep(-0.8f, 1));
        Debug.Log("50 " + Hexath.SnapNumberToStep(25, 50));
    }
}
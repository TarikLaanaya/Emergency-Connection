using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngles : MonoBehaviour
{
    public Transform circle1;
    public Transform circle2;
    public Transform circle3;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 point1 = circle1.position;
        Vector2 point2 = circle2.position;
        Vector2 point3 = circle3.position;

        Vector2 direction = point2 - point3;
        float sign = (direction.y >= 0) ? 1 : -1;

        //Debug.Log(AngleBetweenTwoLines(point1, point2, point3, point2));

        if (Vector2.Distance(point1, point2) < 0.1)
            print("close");
    }

    public static float AngleBetweenTwoLines(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
    {
        float angle1 = Mathf.Atan2(line1End.y - line1Start.y, line1End.x - line1Start.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(line2End.y - line2Start.y, line2End.x - line2Start.x) * Mathf.Rad2Deg;
        float angleBetween = Mathf.DeltaAngle(angle1, angle2);

        return angleBetween;
    }
}

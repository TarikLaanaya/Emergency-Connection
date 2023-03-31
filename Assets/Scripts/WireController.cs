using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WireController : MonoBehaviour
{
    public LineRenderer wire;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject wireSpawner;
    private bool canFingerPress;

    private Vector3 screenPosition;
    private Vector2 touchWorldPosition;
    [SerializeField] private float touchMovementSpeed = 10.0f;

    private Vector3 previousWirePosition;
    private Vector2 lastRayHitLocation;
    private PolygonCollider2D polygonCollider;
    private Vector2 closestPoint;

    private Vector2 wrappedObjectPosition;
    private List<Vector2> wrappedObjectPositionList = new List<Vector2>();
    [SerializeField] private float distanceFromOrigin = 0.02f;

    [SerializeField] private float angleReleasePoint = 160;
    private bool isAnglePositive;
    
    void Start()
    {
        previousWirePosition = wireSpawner.transform.position;
        wire.SetPosition(0, previousWirePosition);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
            screenPosition.z = Camera.main.nearClipPlane;
            touchWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            if (canFingerPress)
            {
                wire.SetPosition(wire.positionCount-1, Vector3.Lerp(wire.GetPosition(wire.positionCount-1), touchWorldPosition, touchMovementSpeed * Time.deltaTime));
                RaycastHit2D hitConnect = Physics2D.Raycast(previousWirePosition, (wire.GetPosition(wire.positionCount-1) - previousWirePosition), Vector2.Distance(wire.GetPosition(wire.positionCount-1), previousWirePosition));
                if (hitConnect.transform != null && hitConnect.transform.tag == "Wrappable")
                {   
                    polygonCollider = hitConnect.transform.GetComponent<PolygonCollider2D>();
                    lastRayHitLocation = hitConnect.point;
                    wrappedObjectPositionList.Add(hitConnect.transform.position);
                    wrappedObjectPosition = wrappedObjectPositionList[wrappedObjectPositionList.Count-1];
                    FindNearestPoint();
                }
            }
            
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out hit))
            {
                if (hit.collider != null)
                {
                    if(hit.collider.tag == "Spawner")
                    {
                        canFingerPress = true;
                    }
                    
                    if(hit.collider.tag == "Danger")
                    {
                        wire.positionCount = 2;
                        wire.SetPosition(0,wireSpawner.transform.position);
                        wire.SetPosition(1,wireSpawner.transform.position);
                        previousWirePosition = wireSpawner.transform.position;
                        wrappedObjectPositionList.Clear();
                        canFingerPress = false;
                    }
                }
            }
        }
        else
        {
            wire.positionCount = 2;
            wire.SetPosition(0,wireSpawner.transform.position);
            wire.SetPosition(1,wireSpawner.transform.position);
            previousWirePosition = wireSpawner.transform.position;
            wrappedObjectPositionList.Clear();
            canFingerPress = false;
        }

        //Angle Release

        if (wire.positionCount > 2)
        {
            float currentAngle = CurrentAngle();

            if(Vector2.Distance(wire.GetPosition(wire.positionCount - 3), wire.GetPosition(wire.positionCount - 2)) < 0.1)
                RemovePoint();

            if (isAnglePositive)
            {
                if(currentAngle < -angleReleasePoint && currentAngle > -179.5)
                    RemovePoint();
            }
            else
            {
                if(currentAngle > angleReleasePoint && currentAngle < 179.5)
                    RemovePoint();
            }
        }
    }

    private void RemovePoint()
    {
        Vector3 originalWirePosition = wire.GetPosition(wire.positionCount - 1);
        wrappedObjectPositionList.Remove(wrappedObjectPositionList[wrappedObjectPositionList.Count - 1]);

        if (wrappedObjectPositionList.Count > 0)
        {
            wrappedObjectPosition = wrappedObjectPositionList[wrappedObjectPositionList.Count - 1];
            previousWirePosition = wire.GetPosition(wire.positionCount - 3) + (new Vector3(wrappedObjectPosition.x, wrappedObjectPosition.y, wire.GetPosition(wire.positionCount - 3).z) - wire.GetPosition(wire.positionCount - 3)).normalized * -distanceFromOrigin;
        }
        else
        {
            previousWirePosition = wire.GetPosition(wire.positionCount - 3);
        }

        wire.positionCount--;
        wire.SetPosition(wire.positionCount - 1, originalWirePosition);

        if(wire.positionCount > 2)
            isAnglePositive = CurrentAngle() > 0 ? true : false;
    }

    private float CurrentAngle()
    {
        float currentAngle = AngleBetweenTwoLines(wire.GetPosition(wire.positionCount-2), wire.GetPosition(wire.positionCount-1), wire.GetPosition(wire.positionCount-2), wire.GetPosition(wire.positionCount-3));

        return currentAngle;
    }

    public static float AngleBetweenTwoLines(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
    {
        float angle1 = Mathf.Atan2(line1End.y - line1Start.y, line1End.x - line1Start.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(line2End.y - line2Start.y, line2End.x - line2Start.x) * Mathf.Rad2Deg;
        float angleBetween = Mathf.DeltaAngle(angle1, angle2);

        return angleBetween;
    }

    void FindNearestPoint()
    {
        float closestPointDistance = 5000;
        
        for (int i = 0; i < polygonCollider.points.Length; i++)
        {
            Vector2 pointInWorldSpace = polygonCollider.transform.TransformPoint(polygonCollider.points[i]);

            if (Vector2.Distance(pointInWorldSpace, lastRayHitLocation) < closestPointDistance)
            {
                closestPointDistance = Vector2.Distance(pointInWorldSpace, lastRayHitLocation);
                closestPoint = pointInWorldSpace; 
            }

            //Debug.Log(pointInWorldSpace);
        }

        Vector3 originalWirePosition = wire.GetPosition(wire.positionCount-1);
        wire.positionCount++;

        wire.SetPosition(wire.positionCount-2, (closestPoint));
        wire.SetPosition(wire.positionCount-1, originalWirePosition);
        previousWirePosition = closestPoint + (wrappedObjectPosition - closestPoint).normalized * -distanceFromOrigin;

        isAnglePositive = CurrentAngle() > 0 ? true : false;
        
        //print(CurrentAngle());
    }
}
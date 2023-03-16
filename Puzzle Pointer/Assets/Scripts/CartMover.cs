using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CartMove : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField, Range(0f, 5f)] float speed = 1f;
    [SerializeField] private float rotationDamp = 5f;
    void Start()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    void FindPath()
    {
        path.Clear();

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Path");

        foreach (GameObject waypoint in waypoints)
        {
            path.Add(waypoint.GetComponent<Waypoint>());
        }
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator FollowPath()
    {
        foreach (Waypoint waypoint in path)
        {
            // Establish our startPosition
            Vector3 startPosition = transform.position;

            // Find our endPosition
            Vector3 endPosition = waypoint.transform.position;

            // A percentage value to check how far we've come
            float travelPercent = 0f;

            
            
            // While this percentage is below 1f, move towards the endPosition (smoothly!)
            while (travelPercent < 1f)
            {
                transform.up= Vector3.Lerp(transform.up, (endPosition - transform.position), travelPercent/rotationDamp);
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}

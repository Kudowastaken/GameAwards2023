using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Dumt namn men tidspress
public class LevelStartEffect : MonoBehaviour
{
    [SerializeField] Transform[] affectedTransforms = new Transform[0];

    [SerializeField] float startPositionRadius = 15f;
    [SerializeField] float startPositionHeightIncrease;
    [SerializeField] float startPositionXAxis;

    [SerializeField] float minDuration, maxDuration;
    [SerializeField] AnimationCurve movementCurve;

    AnimationEvents animationEvents;
    
    void Start()
    {
        animationEvents = FindObjectOfType<AnimationEvents>();

        for (int i = 0; i < affectedTransforms.Length; i++)
        {
            StartCoroutine(MoveObjectToPosition(affectedTransforms[i], Random.Range(minDuration, maxDuration)));
        }
    }

    IEnumerator MoveObjectToPosition(Transform objTransform, float movementDuration)
    {
        Vector2 targetPosition = objTransform.position;
        Vector2 startPosition = objTransform.position = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * (Vector2.right * startPositionRadius);

        startPosition = new Vector2(startPosition.x + startPositionXAxis, startPosition.y + startPositionHeightIncrease);

        foreach (Collider2D objCollider in objTransform.GetComponentsInChildren<Collider2D>())
        {
            objCollider.enabled = false;
        }

        objTransform.position = startPosition;

        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            const float buildDelay = 2.1f;
            yield return new WaitForSeconds(buildDelay);
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / movementDuration;
            
            objTransform.position = Vector2.LerpUnclamped(startPosition, targetPosition, movementCurve.Evaluate(t));

            yield return new WaitForEndOfFrame();
        }

        objTransform.position = targetPosition;
        foreach (Collider2D objCollider in objTransform.GetComponentsInChildren<Collider2D>())
        {
            objCollider.enabled = true;
        }
        animationEvents.hasLoadedScene = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector2.zero, startPositionRadius);
    }
}

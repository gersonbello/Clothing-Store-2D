using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveToPosition : MonoBehaviour
{
    public float speed;
    public Vector2[] positions;

    public void StartMove(int posID)
    {
        StopAllCoroutines();
        StartCoroutine(Move(positions[posID]));
    }

    private IEnumerator Move(Vector2 targetPosition)
    {
        while (Vector2.Distance((Vector2)transform.localPosition, targetPosition) > .01f)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, speed * Time.fixedDeltaTime);
            yield return null;
        }
    }
}

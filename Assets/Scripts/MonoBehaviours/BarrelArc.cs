using System.Collections;
using UnityEngine;

public class BarrelArc : MonoBehaviour
{
    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        var startPosition = transform.position;
        var percentComplete = 0.0f;
        Animator animator = GetComponent<Animator>();

        yield return new WaitForSeconds(0.2f);
        while (percentComplete < 1.0f)
        {
            percentComplete += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete);
            animator.SetBool("isMoving", true);
            yield return null;
        }
        animator.SetBool("isMoving", false);
        gameObject.SetActive(false);
    }

}

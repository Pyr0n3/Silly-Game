using UnityEngine;
using System.Collections;

public class MovingPlatformInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public GameObject button;
    public GameObject movingWall;
    public float wallMoveDistance = 5f; // Distance to move the wall right
    public float moveSpeed = 2f; // Speed of the wall movement

    private bool isWallMoving = false; // To prevent multiple triggers

    void Update()
    {
        // Raycast to check if player is looking at the button within interaction distance
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject == button && Input.GetKeyDown(KeyCode.E) && !isWallMoving)
            {
                StartCoroutine(MoveWallLeftAndRight());
            }
        }
    }

    private IEnumerator MoveWallLeftAndRight()
    {
        isWallMoving = true;

        // Move the wall to the right
        Vector3 targetPositionRight = movingWall.transform.position + Vector3.right * wallMoveDistance;
        while (Vector3.Distance(movingWall.transform.position, targetPositionRight) > 0.01f)
        {
            movingWall.transform.position = Vector3.MoveTowards(movingWall.transform.position, targetPositionRight, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Move the wall back to the left
        Vector3 targetPositionLeft = movingWall.transform.position - Vector3.right * wallMoveDistance;
        while (Vector3.Distance(movingWall.transform.position, targetPositionLeft) > 0.01f)
        {
            movingWall.transform.position = Vector3.MoveTowards(movingWall.transform.position, targetPositionLeft, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isWallMoving = false;
    }

    private void OnDrawGizmos()
    {
        // Visualize the interaction distance in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);
    }
}

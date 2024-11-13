using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public GameObject button;
    public GameObject movingWall;
    public float wallMoveDistance = 5f; // Distance to move the wall up
    public float moveSpeed = 2f; // Speed of the wall movement

    private bool isWallMoving = false; // To prevent multiple triggers

    void Update()
    {
        // Raycast to check if player is looking at the button within interaction distance
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject == button && Input.GetKeyDown(KeyCode.E) && !isWallMoving)
            {
                StartCoroutine(MoveWallUpAndDown());
            }
        }
    }

    private IEnumerator MoveWallUpAndDown()
    {
        isWallMoving = true;

        // Move the wall up
        Vector3 targetPositionUp = movingWall.transform.position + Vector3.up * wallMoveDistance;
        while (Vector3.Distance(movingWall.transform.position, targetPositionUp) > 0.01f)
        {
            movingWall.transform.position = Vector3.MoveTowards(movingWall.transform.position, targetPositionUp, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Move the wall back down
        Vector3 targetPositionDown = movingWall.transform.position - Vector3.up * wallMoveDistance;
        while (Vector3.Distance(movingWall.transform.position, targetPositionDown) > 0.01f)
        {
            movingWall.transform.position = Vector3.MoveTowards(movingWall.transform.position, targetPositionDown, moveSpeed * Time.deltaTime);
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

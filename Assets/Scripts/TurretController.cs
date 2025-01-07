using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform turret; // Assign the turret object in the Inspector
    public Camera mainCamera; // Assign the main camera in the Inspector

    void Update()
    {
        RotateTurretToMouse();
    }

    void RotateTurretToMouse()
    {
        // Raycast to get mouse position in the world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Look at the mouse hit point
            Vector3 targetPosition = hit.point;
            targetPosition.y = turret.position.y; // Keep the turret rotation flat
            turret.LookAt(targetPosition);
        }
    }
}

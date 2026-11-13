using UnityEngine;
using UnityEngine.InputSystem;

public class DroneShooter : MonoBehaviour
{
    [SerializeField] InputActionReference fireAction;
    [SerializeField] Projectile missilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireCooldown = 0.5f;
    [SerializeField] float maxAimDistance = 200f;
    [SerializeField] AudioSource fireAudio;
    [SerializeField] ParticleSystem muzzleFlash;

    Camera cam;
    float nextFireTime;

    void Awake()
    {
        cam = Camera.main;
    }

    void OnEnable()
    {
        fireAction.action.performed += OnFire;
        fireAction.action.Enable();
    }

    void OnDisable()
    {
        fireAction.action.performed -= OnFire;
    }

    void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireCooldown;
        Instantiate(missilePrefab, muzzle.position, GetAimRotation());

        if (muzzleFlash != null) muzzleFlash.Play();
        if (fireAudio != null) fireAudio.Play();
    }

    Quaternion GetAimRotation()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        float skip = Vector3.Distance(ray.origin, muzzle.position);
        Vector3 start = ray.origin + ray.direction * skip;

        Vector3 targetPoint = ray.origin + ray.direction * maxAimDistance;

        if (Physics.Raycast(start, ray.direction, out RaycastHit hit, maxAimDistance, ~0, QueryTriggerInteraction.Ignore))
            targetPoint = hit.point;

        Vector3 toTarget = targetPoint - muzzle.position;

        if (toTarget.sqrMagnitude < 1f)
            return muzzle.rotation;

        return Quaternion.LookRotation(toTarget);
    }
}
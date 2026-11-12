using UnityEngine;
using UnityEngine.InputSystem;

public class DroneShooter : MonoBehaviour
{
    [SerializeField] InputActionReference fireAction;
    [SerializeField] Projectile missilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireCooldown = 0.5f;
    [SerializeField] AudioSource fireAudio;
    [SerializeField] ParticleSystem muzzleFlash;

    float nextFireTime;

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
        Instantiate(missilePrefab, muzzle.position, muzzle.rotation);

        if (muzzleFlash != null) muzzleFlash.Play();
        if (fireAudio != null) fireAudio.Play();
    }
}
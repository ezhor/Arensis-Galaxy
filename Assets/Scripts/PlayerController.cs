using UnityEngine;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private float thrusterForce;
    [SerializeField]
    private float rotationSpeed;
#pragma warning restore 0649

    private ParticleSystem fireParticles;
    private Rigidbody2D rb;

    private void Awake()
    {
        fireParticles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!fireParticles.isEmitting)
            {
                fireParticles.Play();
            }
        }
        else
        {
            fireParticles.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * thrusterForce, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
        }
    }
}

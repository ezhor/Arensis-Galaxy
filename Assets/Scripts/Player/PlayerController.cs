using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float thrusterForce;
    [SerializeField]
    private float rotationSpeed;
    private Rigidbody2D _rb;
    private PlayerVisuals _playerVisuals;


    private void Awake()
    {
        _playerVisuals = GetComponent<PlayerVisuals>();
        _rb = GetComponent<Rigidbody2D>();
        GetComponent<PlayerVisuals>().SetPlayerVisuals(GameController.PlayerInfo.Name, GameController.PlayerInfo.Color);
    }
    
    

    private void Update()
    {
        _playerVisuals.EmitFireParticles = Input.GetKey(KeyCode.UpArrow);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _rb.AddForce(transform.up * thrusterForce, ForceMode2D.Impulse);
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
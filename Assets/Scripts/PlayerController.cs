using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Control del jugador
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    private Vector3 _moveVector;

    // Caracter�sticas dle jugador
    private float speed = 3;
    public Rigidbody playerRB;

    // Variables de estado
    private bool isOnGround;

    // Variable de estado estoy cerca de un arbol
    public bool isTouchTree = false;

    // Variable de madera
    public int wood;

    // Variable valor de madera para instanciar.
    [SerializeField] private int valueWood = 5;

    // Variable para almacenar el prefab de la caja.
    public GameObject prefabCubeWood;

    // Variable para detectar un obstaculo
    public bool isTouchObstacle;
    public LayerMask obstacleMask;
    public Transform check;

    public Animator animator;

    private UIManager UIManager;
    private AudioManager audioManager;

    public GameObject winScene;

    // Update is called once per frame

    private void Start()
    {
        Time.timeScale = 1f;
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        _moveVector = Vector3.zero;
        _moveVector.x = horizontalInput * speed * Time.deltaTime;
        _moveVector.z = verticalInput * speed * Time.deltaTime;

        //Movement + rotation code from -> https://youtu.be/HVTQ-VJCbws

        Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, speed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        playerRB.MovePosition(playerRB.position + _moveVector);

        isTouchObstacle = GameObject.Find("Check Raycast").GetComponent<ConfirmationCube>().auxConfirmation;

        if (wood >= valueWood && Input.GetKeyDown(KeyCode.Mouse1) && !isTouchObstacle)
        {
            wood -= 5;
            Vector3 diretionCube = this.transform.position + transform.forward + new Vector3(0f, 0.3f, 0f);
            Instantiate(prefabCubeWood, diretionCube, prefabCubeWood.transform.rotation);
        }

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRB.AddForce(Vector3.up * 450, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump");
            audioManager.PlayJumpSound();
        }

        if (transform.position.y < -5f)
        {
            UIManager.gameOverCanva.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = true;
            animator.SetTrigger("onGround");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            winScene.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
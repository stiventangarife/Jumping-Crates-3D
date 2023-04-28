using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject canvasObj;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject gameOverCanva;
    public GameObject instructionsCanva;
    public GameObject playerCanva;
    public TextMeshProUGUI woodPoints;
    public TextMeshProUGUI instructionsText;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(InstructionsChanger());
    }
    void Update()
    {
        //Actualiza el texto para llevar conteo de cantidad de recursos
        woodPoints.text = "Resources: " + playerController.wood.ToString();

        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateGameState();            
        }
    }


    IEnumerator InstructionsChanger()
    {
        instructionsText.text = "Use W,A,S,D to move";
        yield return new WaitForSeconds(3);
        instructionsText.text = "LeftClick close to a tree to chop it";
        yield return new WaitForSeconds(3);
        instructionsText.text = "Use SpaceBar to jump";
        yield return new WaitForSeconds(3);
        instructionsCanva.SetActive(false);
        yield return new WaitForSeconds(10);
        instructionsCanva.SetActive(true);
        instructionsText.text = "Try RightClick to create a wooden box and reach the top";
        yield return new WaitForSeconds(10);
        Destroy(instructionsCanva);
    }  

    public void UpdateGameState()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            playerCanva.SetActive(false);
            canvasObj.SetActive(true);
            pauseButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            playerCanva.SetActive(true);
            canvasObj.SetActive(false);
            pauseButton.SetActive(true);
        }
    }
}

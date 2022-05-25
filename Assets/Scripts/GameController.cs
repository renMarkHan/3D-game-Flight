using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameController : MonoBehaviour
{
    public GameObject obstacle;

    private ArrayList obstacles;

    public TextMeshProUGUI scoreText;
    public GameObject loseText;
    public GameObject winText;
    // Use this for initialization
    void Start()
    {
        winText.SetActive(false);
        loseText.SetActive(false);
        obstacles = new ArrayList();
        CreateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Obstacles remaining: " + obstacles.Count;

        //to be completed
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void CreateObstacles()
    {
        //you should create at least 3 obstacles with different rotations 
        //and in different positions
        //note: we are adding them to the ArrayList created above
        
        
        //Instantiate can take the transform (position vector) 
        //and rotation quaternion as parameters

        obstacles.Add(
            Instantiate(obstacle, new Vector3(4, 4, 4), Quaternion.identity)
        );
        obstacles.Add(
            Instantiate(obstacle, new Vector3(-3, 7, 9), Quaternion.AngleAxis(30f, Vector3.up))
        );

        //Quaternion.AngleAxis creates a Quaternion object that is defined by 
        //the number of degrees of rotation around a provided axis. 
        //Below we provide the up axis (or y-axis)
        obstacles.Add(
            Instantiate(obstacle, new Vector3(-10, 8, 6), Quaternion.AngleAxis(45f, Vector3.up))
        );
    }

    //This method is to be called by the FlyingCraft when it successfully
    //makes it through a gap. The first parameter is the obstacle,
    //the second parameter is the FlyingCraft game object.
    public void Score(GameObject obstacle, GameObject sender)
    {
        //check if the obstacle is in the list
        //i.e., has not yet been passed through
        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
        }

        if (obstacles.Count == 0)
        {
            Win();
            sender.SendMessage("Stop");
        }
    }

    public void Win()
    {
        winText.SetActive(true);
    }

    public void GameOver()
    {
        loseText.SetActive(true);
    }
}

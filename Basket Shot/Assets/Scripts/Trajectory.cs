using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : Singleton<Trajectory>
{
    [SerializeField] private GameObject obstacles;
    [SerializeField] private int maxIterations;

    Scene currentScene;
    Scene predictionScene;

    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;

    List<GameObject> dummyObstacles = new List<GameObject>();

    LineRenderer lineRenderer;
    GameObject dummy;

    [SerializeField] private GameObject ballTrajectory;
    private List<GameObject> ballsTrajectory = new List<GameObject>();

    void Start()
    {
        Physics.autoSimulation = false;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();

        copyAllObstacles();

        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    public void copyAllObstacles()
    {
        foreach (Transform t in obstacles.transform)
        {
            if (t.gameObject.GetComponent<Collider2D>() != null)
            {
                GameObject fakeT = Instantiate(t.gameObject);
                fakeT.transform.position = t.position;
                fakeT.transform.rotation = t.rotation;
                SpriteRenderer fakeR = fakeT.GetComponent<SpriteRenderer>();
                if (fakeR)
                {
                    fakeR.enabled = false;
                }
                SceneManager.MoveGameObjectToScene(fakeT, predictionScene);
                dummyObstacles.Add(fakeT);
            }
        }
    }

    void killAllObstacles()
    {
        foreach (var o in dummyObstacles)
        {
            Destroy(o);
        }
        dummyObstacles.Clear();
    }

    public void predict(GameObject subject, Vector3 currentPosition, Vector3 force)
    {
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(subject);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
            }

            dummy.transform.position = currentPosition;
            dummy.GetComponent<Rigidbody2D>().velocity = force;

            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = maxIterations;

            for (int i = 0; i < maxIterations; i++)
            {
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                lineRenderer.SetPosition(i, dummy.transform.position);
                
                if (i % 5 == 0 || i == 0)
                {
                    if (i >= maxIterations / 3)
                    {
                        float per = 400f;
                        GameObject ball = Instantiate(ballTrajectory, lineRenderer.GetPosition(i), Quaternion.identity);
                        ball.transform.localScale = new Vector3(ball.transform.localScale.x - (float)i / per, ball.transform.localScale.y - (float)i / per, ball.transform.localScale.z - (float)i / per);
                        ballsTrajectory.Add(ball);
                    }
                    else
                    {
                        ballsTrajectory.Add(Instantiate(ballTrajectory, lineRenderer.GetPosition(i), Quaternion.identity));
                    }
                }
            }

            Destroy(dummy);
        }
    }

    public void RemBalls()
    {
        foreach (GameObject g in ballsTrajectory)
        {
            Destroy(g.gameObject);
        }
        ballsTrajectory.Clear();
    }

    void OnDestroy()
    {
        killAllObstacles();
    }
}

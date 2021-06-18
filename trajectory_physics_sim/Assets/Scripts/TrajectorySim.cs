using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// REFERENCE: https://github.com/ToughNutToCrack/TrajectoryPrediction
public class TrajectorySim : MonoBehaviour
{

    private Scene currentScene;
    private Scene predictionScene;
    private PhysicsScene currentPhysicsScene;
    private PhysicsScene predictionPhysicsScene;

    public int MAX_SIMULATION_ITERATIONS = 100;

    private LineRenderer lineRenderer;
    private Dictionary<string, GameObject> simulationObjects;

    GameObject simulationSubject;
    // Start is called before the first frame update
    public void Load()
    {
        simulationObjects = new Dictionary<string, GameObject>();
        InitComponents();
    }

    public void CreateObjectInSimWorld(GameObject newObject, string objectName)
    {
        GameObject newSimObj = Instantiate(newObject, newObject.transform.position, newObject.transform.rotation);
        simulationObjects.Add(objectName, newSimObj);
        SceneManager. MoveGameObjectToScene(newSimObj, predictionScene);

        EnsureDisableScriptOnSimObject(objectName, newSimObj);
    }

    public void UpdateCannonTransform(Transform newTransform)
    {
        GameObject cannonSim;
        simulationObjects.TryGetValue("cannon", out cannonSim);
        cannonSim.transform.position = newTransform.position;
        cannonSim.transform.rotation = newTransform.rotation;
    }

    public void PredictPhysics(GameObject subject, Vector3 currentPosition, Vector3 force)
    {        
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            EnsureSimulationSubjectExists(ref simulationSubject, subject);

            simulationSubject.transform.position = currentPosition;
            simulationSubject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            DrawSimulatedTrajectory(ref simulationSubject);
        }
    }

    private void EnsureDisableScriptOnSimObject(string objName, GameObject simObj)
    {
        if (objName == "cannon")
        {
            simObj.GetComponent<CannonController>().enabled = false;
        }
    }

    private void DrawSimulatedTrajectory(ref GameObject simSubject)
    {
        lineRenderer.positionCount = MAX_SIMULATION_ITERATIONS;
        for (int i = 0; i < MAX_SIMULATION_ITERATIONS; i++)
        {
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, simSubject.transform.position);
        }
        Destroy(simSubject);
    }

    // If simulation subject doesn't exist in the physics world yet
    // THEN create it
    private void EnsureSimulationSubjectExists(ref GameObject simSubject, GameObject subject)
    {
        if (simSubject == null)
        {
            simSubject = Instantiate(subject);
            SceneManager.MoveGameObjectToScene(simSubject, predictionScene);
        }
    }

    private void InitComponents()
    {
        Physics.autoSimulation = false;
        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("PredictionScene", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();

        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

}

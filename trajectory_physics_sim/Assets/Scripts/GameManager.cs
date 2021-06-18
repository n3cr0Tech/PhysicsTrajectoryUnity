using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TrajectorySim trajectorySim;
    public GameObject FloorPrefab;
    public GameObject CannonPrefab;
    public GameObject BasicObjectsContainer;

    private Dictionary<string, GameObject> environmentObjects;
    private CannonController cannonScript;

    void Start()
    {
        environmentObjects = new Dictionary<string, GameObject>();
        trajectorySim.Load();
        CreateInitialObjects();
    }

    private void CreateInitialObjects()
    {
        GameObject floor = Instantiate(FloorPrefab, new Vector3(0, 0, 0), transform.rotation);
        environmentObjects.Add("floor", floor);
        trajectorySim.CreateObjectInSimWorld(floor, "floor");

        GameObject cannon = Instantiate(CannonPrefab, new Vector3(0, 10, 0), transform.rotation);
        cannonScript = cannon.GetComponent<CannonController>();
        cannonScript.Load(trajectorySim);
        environmentObjects.Add("cannon", cannon);
        trajectorySim.CreateObjectInSimWorld(cannon, "cannon");

    }

    // Update is called once per frame
    void Update()
    {
        EnsureCannonExistsForInteractions();  
    }

    private void EnsureCannonExistsForInteractions()
    {
        if (cannonScript != null)
        {
            HandleCannonShootInput();
            HandleCannonRotateInput();
        }
    }

    private void HandleCannonShootInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            cannonScript.Shoot();
        }
    }

    private void HandleCannonRotateInput()
    {
        float vertInput = Input.GetAxis("Vertical");
        float horInput = Input.GetAxis("Horizontal");
        cannonScript.UpdateTransform(horInput, vertInput);
        trajectorySim.UpdateCannonTransform(cannonScript.gameObject.transform);
    }

}

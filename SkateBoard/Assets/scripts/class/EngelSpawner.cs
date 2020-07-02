using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngelSpawner : MonoBehaviour
{
    private GameObject cityObstaclesParent,parkObstaclesParent;
    [SerializeField]
    private GameObject[] cityObstaclePrefabs,parkObstaclePrefabs;

    private List<GameObject> cityObstacles = new List<GameObject>();
    private List<GameObject> parkObstacles = new List<GameObject>();

    private Transform playerTransform;
    
    [SerializeField]
    private string environment = "City";

    [SerializeField]
    private int stage = 1;
    
    [SerializeField]
    private GameObject L_lastObstacle, R_lastObstacle;

    private GameObject empty;

    public float cityBeginningPoint, cityEndingPoint, parkBeginningPoint, parkEndingPoint;

    #region Singleton
    public static EngelSpawner instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(!GameObject.Find("CityObstacles"))
        {
            cityObstaclesParent = new GameObject("CityObstacles");
        }
        else
        {
            cityObstaclesParent = GameObject.Find("CityObstacles");
        }
        if(!GameObject.Find("ParkObstacles"))
        {
            parkObstaclesParent = new GameObject("ParkObstacles");
        }
        else
        {
            parkObstaclesParent = GameObject.Find("ParkObstacles");
        }

        playerTransform = GameObject.Find("player").transform;
        empty = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
            if (L_lastObstacle.transform.position.z < playerTransform.position.z + 50f)
            {
                SpawnObstacle("Left");
            }
            if (R_lastObstacle.transform.position.z < playerTransform.position.z + 50f)
            {
                SpawnObstacle("Right");
            }
    }

    private void SpawnObstacle(string direction)
    {
        string env;
        GameObject objToSpawn = empty;
        Vector3 spawnPoint;
        if (direction == "Left")
            spawnPoint = L_lastObstacle.transform.position + new Vector3(0f, 0f, L_lastObstacle.GetComponent<MeshRenderer>().bounds.extents.z + Random.Range(50f, 100f));
        else
            spawnPoint = R_lastObstacle.transform.position + new Vector3(0f, 0f, R_lastObstacle.GetComponent<MeshRenderer>().bounds.extents.z + Random.Range(50f, 100f));
        if (spawnPoint.z < cityEndingPoint && spawnPoint.z > cityBeginningPoint)
        {
            env = "City";
        }
        if (spawnPoint.z < parkEndingPoint && spawnPoint.z > parkBeginningPoint)
        {
            env = "Park";
        }
        else
        {
            env = "City";
        }
        if (env == "City")
        {
            int a = Random.Range(0, cityObstaclePrefabs.Length);
            
            switch (a)
            {
                case 0:
                    objToSpawn = GetObstacleFromCityPool("Bank");
                    break;
                case 1:
                    objToSpawn = GetObstacleFromCityPool("Box");
                    break;
                case 2:
                    objToSpawn = GetObstacleFromCityPool("Ramp");
                    break;
            }
        }
        else if (env == "Park")
        {
            int a = Random.Range(0, parkObstaclePrefabs.Length);
           
            switch(a)
            {
                case 0:
                    objToSpawn = GetObstacleFromParkPool("Bank");
                    break;
                case 1:
                    objToSpawn = GetObstacleFromParkPool("Platform");
                    break;
                case 2:
                    objToSpawn = GetObstacleFromParkPool("Ramp");
                    break;
;           }
        }
        if (env == "City")
        {
            if (direction == "Left")
            {
                objToSpawn.transform.position = new Vector3(1, 0, L_lastObstacle.transform.position.z + Random.Range(50f, 100f));
                if (Mathf.Abs(R_lastObstacle.transform.position.z - objToSpawn.transform.position.z) < 25f)
                    objToSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(R_lastObstacle.transform.position.z - objToSpawn.transform.position.z));
                L_lastObstacle = objToSpawn;
            }
            if (direction == "Right")
            {
                objToSpawn.transform.position = new Vector3(-1, 0, R_lastObstacle.transform.position.z + Random.Range(50f, 100f));

                if (Mathf.Abs(L_lastObstacle.transform.position.z - objToSpawn.transform.position.z) < 25f)
                    objToSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(L_lastObstacle.transform.position.z - objToSpawn.transform.position.z));
                R_lastObstacle = objToSpawn;
            }
        }
        if(env == "Park")
        {
            if (direction == "Left")
            {
                objToSpawn.transform.position = new Vector3(1, 0, L_lastObstacle.transform.position.z + Random.Range(50f, 100f));
                if (Mathf.Abs(R_lastObstacle.transform.position.z - objToSpawn.transform.position.z) < 25f)
                    objToSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(R_lastObstacle.transform.position.z - objToSpawn.transform.position.z));
                L_lastObstacle = objToSpawn;
            }
            if (direction == "Right")
            {
                objToSpawn.transform.position = new Vector3(-1, 0, R_lastObstacle.transform.position.z + Random.Range(50f, 100f));

                if (Mathf.Abs(L_lastObstacle.transform.position.z - objToSpawn.transform.position.z) < 25f)
                    objToSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(L_lastObstacle.transform.position.z - objToSpawn.transform.position.z));
                R_lastObstacle = objToSpawn;
            }


        }
    }


    //private void CitySpawnObstacle(string Direction)
    //{
    //    GameObject objtoSpawn;
    //    string obstacleType;
    //    int a = Random.Range(0, cityObstaclePrefabs.Length);
    //    switch(a)
    //    {
    //        case 0: obstacleType = "Bank";
    //            break;
    //        case 1: obstacleType = "Box";
    //            break;
    //        case 2: obstacleType = "Ramp";
    //            break;
    //        default: obstacleType = "Bank";
    //            break;
    //    }


    //    if (Direction == "Left")
    //    {
    //        objtoSpawn = GetObstacleFromCityPool(obstacleType);
    //        objtoSpawn.transform.position = new Vector3(1, 0, L_lastObstacle.transform.position.z + Random.Range(50f,100f));
    //        if (Mathf.Abs(R_lastObstacle.transform.position.z - objtoSpawn.transform.position.z) < 25f)
    //            objtoSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(R_lastObstacle.transform.position.z - objtoSpawn.transform.position.z));
    //        L_lastObstacle = objtoSpawn;
    //    }
    //    if(Direction == "Right")
    //    {

    //        objtoSpawn = GetObstacleFromCityPool(obstacleType);
    //        objtoSpawn.transform.position = new Vector3(-1, 0, R_lastObstacle.transform.position.z + Random.Range(50f,100f));

    //        if (Mathf.Abs(L_lastObstacle.transform.position.z - objtoSpawn.transform.position.z) < 25f)
    //            objtoSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(L_lastObstacle.transform.position.z - objtoSpawn.transform.position.z));
    //        R_lastObstacle = objtoSpawn;
    //    }
    //}
    //private void ParkSpawnObstacle(string Direction)
    //{
    //    GameObject objtoSpawn;
    //    string obstacleType;
    //    int a = Random.Range(0, parkObstaclePrefabs.Length);
    //    switch (a)
    //    {
    //        case 0:
    //            obstacleType = "Bank";
    //            break;
    //        case 1:
    //            obstacleType = "Platform";
    //            break;
    //        case 2:
    //            obstacleType = "Ramp";
    //            break;
    //        default:
    //            obstacleType = "Bank";
    //            break;
    //    }


    //    if (Direction == "Left")
    //    {
    //        objtoSpawn = GetObstacleFromParkPool(obstacleType);
    //        objtoSpawn.transform.position = new Vector3(1, 0, L_lastObstacle.transform.position.z + Random.Range(50f, 100f));
    //        if (Mathf.Abs(R_lastObstacle.transform.position.z - objtoSpawn.transform.position.z) < 25f)
    //            objtoSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(R_lastObstacle.transform.position.z - objtoSpawn.transform.position.z));
    //        L_lastObstacle = objtoSpawn;
    //    }
    //    if (Direction == "Right")
    //    {

    //        objtoSpawn = GetObstacleFromParkPool(obstacleType);
    //        objtoSpawn.transform.position = new Vector3(-1, 0, R_lastObstacle.transform.position.z + Random.Range(50f, 100f));

    //        if (Mathf.Abs(L_lastObstacle.transform.position.z - objtoSpawn.transform.position.z) < 25f)
    //            objtoSpawn.transform.position += Vector3.forward * (25f - Mathf.Abs(L_lastObstacle.transform.position.z - objtoSpawn.transform.position.z));
    //        R_lastObstacle = objtoSpawn;
    //    }

    //}

    private GameObject GetObstacleFromCityPool(string obstacleType)
    {
        GameObject objtoSpawn;
        switch (obstacleType)
        {
            case "Bank":
                objtoSpawn = cityObstaclePrefabs[0];
                break;
            case "Box":
                objtoSpawn = cityObstaclePrefabs[1];
                break;
            case "Ramp":
                objtoSpawn = cityObstaclePrefabs[2];
                break;
            default:
                objtoSpawn = cityObstaclePrefabs[0];
                break;
        }


        //for (int i = 0; i < cityObstacles.Count; i++)
        //{
        //    if (cityObstacles[i].activeInHierarchy == false)
        //    {
        //        cityObstacles[i].SetActive(true);
        //        return cityObstacles[i];
        //    }
        //}
        GameObject temp = Instantiate<GameObject>(objtoSpawn, cityObstaclesParent.transform);
        cityObstacles.Add(temp);
        return temp;
    }
    private GameObject GetObstacleFromParkPool(string obstacleType)
    {
        GameObject objtoSpawn;
        switch (obstacleType)
        {
            case "Bank":
                objtoSpawn = parkObstaclePrefabs[0];
                break;
            case "Platform":
                objtoSpawn = parkObstaclePrefabs[1];
                break;
            case "Ramp":
                objtoSpawn = parkObstaclePrefabs[2];
                break;
            default:
                objtoSpawn = parkObstaclePrefabs[0];
                break;
        }


        for (int i = 0; i < parkObstacles.Count; i++)
        {
            if (parkObstacles[i].activeInHierarchy == false)
            {
                parkObstacles[i].SetActive(true);
                return parkObstacles[i];
            }
        }
        GameObject temp = Instantiate<GameObject>(objtoSpawn, parkObstaclesParent.transform);
        parkObstacles.Add(temp);
        return temp;
    }

    private IEnumerator ClearPool()
    {
        while(true)
        {
                for (int i = 0; i < cityObstacles.Count; i++)
                {
                    if (cityObstacles[i].transform.position.z < playerTransform.position.z - 10f)
                    {
                        cityObstacles[i].SetActive(false);
                    }
                }
                for (int i = 0; i < parkObstacles.Count; i++)
                {
                    if(parkObstacles[i].transform.position.z < playerTransform.position.z - 10)
                    {
                        parkObstacles[i].SetActive(false);
                    }
                }
            
            yield return new WaitForSeconds(5f);
        }
    }
}

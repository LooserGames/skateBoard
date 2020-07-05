using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    private float sightDistance = 150f;
    public GameObject cityRoadPrefab,parkRoadPrefab;
    [SerializeField]
    public GameObject[] citySideObjects,parkSideObjects;
    public GameObject L_lastSideObject,R_lastSideObject, LastRoad;

    [SerializeField]
    private Material[] CityBuildingMaterials;
    

    public Transform PlayerTransform;

    public GameObject emptyObj; // getting rid of null refence exception in methods

    private GameObject citySideObjParent , cityRoadsParent,parkSideObjParent,parkRoadsParent;

    [Header("Test")][SerializeField]private float minBuildingSpacing=1f, maxBuildingSpacing=5f;

    private List<GameObject> cityMasterPool = new List<GameObject>(); // 
    private List<GameObject> parkMasterPool = new List<GameObject>(); // Master pools for deactivating objects in hierarchy easier

    private List<GameObject> cityBuildingPool = new List<GameObject>();
    private List<GameObject> cityRoadPool = new List<GameObject>();

    private List<GameObject> parkSeesawPool = new List<GameObject>();
    private List<GameObject> parkSlidePool = new List<GameObject>();
    private List<GameObject> parkRoadPool = new List<GameObject>();
    

    private List<GameObject> treePool = new List<GameObject>();

    public string environment = "City";
    private float cityBeginningPoint, cityEndingPoint, parkBeginningPoint, parkEndingPoint;
    public bool city = true, park = false;


    #region Singleton
    public static BuildingSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region SpawnParentObj
        GameObject cityParent;
        if(!GameObject.Find("CityObjects"))
        {
            cityParent = new GameObject("CityObjects");
        }
        else
        {
            cityParent = GameObject.Find("CityObjects");
        }
        GameObject parkParent;
        if(!GameObject.Find("ParkObjects"))
        {
            parkParent = new GameObject("ParkObjects");
        }
        else
        {
            parkParent = GameObject.Find("ParkObjects");
        }
        if (!GameObject.Find("CitySideObjects"))
        {
            citySideObjParent = new GameObject("CitySideObjects");
            citySideObjParent.transform.parent = cityParent.transform;
        }
        else
        {
            citySideObjParent = GameObject.Find("CitySideObjects");
            citySideObjParent.transform.parent = cityParent.transform;
        }
        if(!GameObject.Find("ParkSideObjects"))
        {
            parkSideObjParent = new GameObject("ParkSideObjects");
            parkSideObjParent.transform.parent = parkParent.transform;
        }
        else
        {
            parkSideObjParent = GameObject.Find("ParkSideObjects");
            parkSideObjParent.transform.parent = parkParent.transform;
        }
        if(!GameObject.Find("CityRoads"))
        {
            cityRoadsParent = new GameObject("CityRoads");
            cityRoadsParent.transform.parent = cityParent.transform;
        }
        else
        {
            cityRoadsParent = GameObject.Find("CityRoads");
            cityRoadsParent.transform.parent = cityParent.transform;
        }
        if(!GameObject.Find("ParkRoads"))
        {
            parkRoadsParent = new GameObject("ParkRoads");
            parkRoadsParent.transform.parent = parkParent.transform;
        }
        else
        {
            parkRoadsParent = GameObject.Find("ParkRoads");
            parkRoadsParent.transform.parent = parkParent.transform;
        }
        #endregion
       
        SetBeginningPoint(0);
        if (LastRoad == null)
            SpawnRoad();

        PlayerTransform = GameObject.Find("player").transform;
        StartCoroutine(ClearPool());

        emptyObj = new GameObject("Empty"); // getting rid of null refence exception in methods

    }

    // Update is called once per frame
    void Update()
    {
        if(L_lastSideObject!= null)
        {
            if(L_lastSideObject.transform.position.z < sightDistance + PlayerTransform.position.z)
            {
                SpawnObject("Left");
            }  
        } //Checking if should place building
        if(R_lastSideObject != null)
        {
            if(R_lastSideObject.transform.position.z < sightDistance + PlayerTransform.position.z)
            {
                SpawnObject("Right");
            }
        }

        if(LastRoad != null) //Checking if should place road
        {
            if(LastRoad.transform.position.z < sightDistance + PlayerTransform.position.z)
            {
                SpawnRoad();
            }
        }
        
    }

    private void SpawnObject(string direction)
    {
        string env;
        GameObject objToSpawn = emptyObj;
        Vector3 spawnPoint;
        if (direction == "Left")
            spawnPoint = L_lastSideObject.transform.position + new Vector3(0f, 0f, L_lastSideObject.GetComponent<MeshRenderer>().bounds.extents.z + Random.Range(minBuildingSpacing, maxBuildingSpacing));
        else
            spawnPoint = R_lastSideObject.transform.position + new Vector3(0f, 0f, R_lastSideObject.GetComponent<MeshRenderer>().bounds.extents.z + Random.Range(minBuildingSpacing, maxBuildingSpacing));
        if(spawnPoint.z < cityEndingPoint && spawnPoint.z > cityBeginningPoint)
        {
            env = "City";
        }
        if(spawnPoint.z < parkEndingPoint && spawnPoint.z > parkBeginningPoint)
        {
            env = "Park";
        }
        else
        {
            env = "City";
        }

        bool canChangeMaterial = false;
        if (env == "City")
        {
            int a = Random.Range(0, citySideObjects.Length);
            switch (a)
            {
                case 0:
                    objToSpawn = GetSideObjectFromPool("Building",env);
                    canChangeMaterial = true;
                    break;
                case 1:
                    objToSpawn = GetSideObjectFromPool("Tree",env);
                    canChangeMaterial = false;
                    break;
                default:
                    objToSpawn = GetSideObjectFromPool("Building",env);
                    canChangeMaterial = true;
                    break;
            }
        }
        else if (env == "Park")
        {
            int a = Random.Range(0, parkSideObjects.Length * 20);
            if(a >= 0 && a < 5)
            {
                objToSpawn = GetSideObjectFromPool("Seesaw",env);
                canChangeMaterial = false;
            }
            if (a >= 5 && a < 55)
            {
                objToSpawn = GetSideObjectFromPool("Tree",env);
                canChangeMaterial = false;
            }
            if (a >= 55 && a < 60)
            {
                objToSpawn = GetSideObjectFromPool("Slide",env);
                canChangeMaterial = false;
            }
        }
        if (direction == "Left")
        {
            objToSpawn.transform.localScale = new Vector3(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(1, 1));
            objToSpawn.transform.position =  spawnPoint + new Vector3(0,0,objToSpawn.GetComponent<MeshRenderer>().bounds.extents.z);
            objToSpawn.transform.rotation = L_lastSideObject.transform.rotation;
            if(canChangeMaterial)
            objToSpawn.GetComponent<MeshRenderer>().sharedMaterial = GetMaterialFromBuildingPalette();
            L_lastSideObject = objToSpawn;

        }
        if (direction == "Right")
        {
            objToSpawn.transform.localScale = new Vector3(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(1, 1));
            objToSpawn.transform.position = spawnPoint + new Vector3(0f, 0f, objToSpawn.GetComponent<MeshRenderer>().bounds.extents.z);
            objToSpawn.transform.rotation = R_lastSideObject.transform.rotation;
            if(canChangeMaterial)
            objToSpawn.GetComponent<MeshRenderer>().sharedMaterial = GetMaterialFromBuildingPalette();
            R_lastSideObject = objToSpawn;
        }
        
    }

    private void SpawnRoad()
    {
        Vector3 spawnpoint = LastRoad.transform.position + new Vector3(0f, 0f, LastRoad.GetComponent<MeshRenderer>().bounds.extents.z);
        string env = "City";
        if (spawnpoint.z > cityEndingPoint && spawnpoint.z > parkEndingPoint)
        {
            if (cityEndingPoint > parkEndingPoint)
            {
                park = true;
                city = false;
                SetBeginningPoint(cityEndingPoint);
            }
            else
            {
                SetBeginningPoint(parkEndingPoint);
                city = true;
                park = false;
            }
        }
        if (spawnpoint.z >= cityBeginningPoint && spawnpoint.z < cityEndingPoint)
        { env = "City"; }
        if (spawnpoint.z >= parkBeginningPoint && spawnpoint.z < parkEndingPoint)
        { env = "Park"; }

        GameObject objToSpawn = GetRoadFromPool(env);
        objToSpawn.transform.position = LastRoad.transform.position + new Vector3(0f, 0f, LastRoad.GetComponent<MeshRenderer>().bounds.extents.z + objToSpawn.GetComponent<MeshRenderer>().bounds.extents.z);
        LastRoad = objToSpawn;
    }

    private GameObject GetSideObjectFromPool(string objectType,string env)
    {
        if (env == "City")
        {
            if (objectType == "Building")
            {
                for (int i = 0; i < cityBuildingPool.Count; i++)
                {
                    if (cityBuildingPool[i].activeInHierarchy == false)
                    {
                        cityBuildingPool[i].SetActive(true);
                        return cityBuildingPool[i];
                    }
                }
                GameObject temp = Instantiate(citySideObjects[0], citySideObjParent.transform);
                cityBuildingPool.Add(temp);
                cityMasterPool.Add(temp);
                return temp;
            }
            if (objectType == "Tree")
            {
                for (int i = 0; i < treePool.Count; i++)
                {
                    if (treePool[i].activeInHierarchy == false)
                    {
                        treePool[i].SetActive(true);
                        return treePool[i];
                    }
                }
                GameObject temp = Instantiate(citySideObjects[1], citySideObjParent.transform);
                treePool.Add(temp);
                cityMasterPool.Add(temp);
                return temp;
            }
        }
        else if(env == "Park")
        {
            if(objectType == "Seesaw")
            {
                for (int i = 0; i < parkSeesawPool.Count; i++)
                {
                    if(parkSeesawPool[i].activeInHierarchy == false)
                    {
                        parkSeesawPool[i].SetActive(true);
                        return parkSeesawPool[i];
                    }
                }
                GameObject temp = Instantiate(parkSideObjects[0], parkSideObjParent.transform);
                parkSeesawPool.Add(temp);
                parkMasterPool.Add(temp);
                return temp;
            }
            if (objectType == "Tree")
            {
                    for (int i = 0; i < treePool.Count; i++)
                    {
                        if (treePool[i].activeInHierarchy == false)
                        {
                            treePool[i].SetActive(true);
                            return treePool[i];
                        }
                    }
                    GameObject temp = Instantiate(parkSideObjects[1], parkSideObjParent.transform);
                    treePool.Add(temp);
                    parkMasterPool.Add(temp);
                    return temp;
            }
            if(objectType == "Slide")
            {
                for (int i = 0; i < parkSlidePool.Count; i++)
                {
                    if(parkSlidePool[i].activeInHierarchy == false)
                    {
                        parkSlidePool[i].SetActive(true);
                        return parkSlidePool[i];
                    }
                }
                GameObject temp = Instantiate(parkSideObjects[2], parkSideObjParent.transform);
                parkSlidePool.Add(temp);
                parkMasterPool.Add(temp);
                return temp;
            }
        }
        return null;
    }

    private GameObject GetRoadFromPool(string env)
    {
        if (env == "City")
        {
            for (int i = 0; i < cityRoadPool.Count; i++)
            {
                if (cityRoadPool[i].activeInHierarchy == false)
                {
                    cityRoadPool[i].SetActive(true);
                    
                    return cityRoadPool[i];
                }
            }
            GameObject temp = Instantiate(cityRoadPrefab, cityRoadsParent.transform);
            cityRoadPool.Add(temp);
            cityMasterPool.Add(temp);
            return temp;
        }
        if(env == "Park")
        {
            for (int i = 0; i < parkRoadPool.Count; i++)
            {
                if(parkRoadPool[i].activeInHierarchy == false)
                {
                    parkRoadPool[i].SetActive(true);
                    return parkRoadPool[i];
                }
            }
            GameObject temp = Instantiate(parkRoadPrefab, parkRoadsParent.transform);
            parkRoadPool.Add(temp);
            parkMasterPool.Add(temp);
            return temp;
        }
        return null;
    }

    private Material GetMaterialFromBuildingPalette()
    {
        int a = Random.Range(0, CityBuildingMaterials.Length);
        return CityBuildingMaterials[a];
    }


    private IEnumerator ClearPool()
    {
        while (true)
        {
            //for (int i = 0; i < cityBuildingPool.Count; i++)
            //{
            //    if(cityBuildingPool[i].transform.position.z < PlayerTransform.position.z - 10f)
            //    {
            //        cityBuildingPool[i].SetActive(false);
            //    }
            //}
            //for (int i = 0; i < treePool.Count; i++)
            //{
            //    if (treePool[i].transform.position.z < PlayerTransform.position.z - 10f)
            //    {
            //        treePool[i].SetActive(false);
            //    }
            //}
            //for (int i = 0; i < cityRoadPool.Count; i++)
            //{
            //    if (cityRoadPool[i].transform.position.z < PlayerTransform.position.z - 50f)
            //    {
            //        cityRoadPool[i].SetActive(false);
            //    }
            //}
            for (int i = 0; i < cityMasterPool.Count; i++)
            {
                if(cityMasterPool[i].transform.position.z < PlayerTransform.position.z - 50f)
                {
                    cityMasterPool[i].SetActive(false);
                }

            }
            for (int i = 0; i < parkMasterPool.Count; i++)
            {
                if(parkMasterPool[i].transform.position.z < PlayerTransform.position.z -50f)
                {
                    parkMasterPool[i].SetActive(false);
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void SetBeginningPoint(float zposition)
    {
        if(city)
        {
            city = false;
            cityBeginningPoint = zposition;
            cityEndingPoint = zposition + cityRoadPrefab.GetComponent<MeshRenderer>().bounds.extents.z * 2 * 8;
            parkBeginningPoint = cityEndingPoint;
            EngelSpawner.instance.cityBeginningPoint = cityBeginningPoint;
            EngelSpawner.instance.cityEndingPoint = cityEndingPoint;
            EngelSpawner.instance.parkBeginningPoint = parkBeginningPoint;
        }
        if(park)
        {
            park = false;
            parkBeginningPoint = zposition;
            parkEndingPoint = zposition + cityRoadPrefab.GetComponent<MeshRenderer>().bounds.extents.z * 2 * 4;
            cityBeginningPoint = parkEndingPoint;
            EngelSpawner.instance.parkBeginningPoint = parkBeginningPoint;
            EngelSpawner.instance.parkEndingPoint = parkEndingPoint;
            EngelSpawner.instance.cityBeginningPoint = cityBeginningPoint;
        }
    }
}
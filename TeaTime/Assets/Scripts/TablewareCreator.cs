using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablewareCreator : Singleton<TablewareCreator>
{
    public GameObject TablewarePrefab;
    public int InitialTablewareCount;

    private List<GameObject> tableware = new List<GameObject>();

    public void Start()
    {
        for(int i = 0; i < InitialTablewareCount; i++)
        {
            InstantiateAndAddNewTableware();
        }
    }

    public GameObject GetTableware()
    {
        foreach(GameObject instance in tableware)
        {
            if (!instance.activeSelf) return instance;
        }
        InstantiateAndAddNewTableware();
        return tableware[tableware.Count - 1];
    }

    public void Reset()
    {
        foreach (GameObject instance in tableware)
        {
            instance.gameObject.SetActive(false);
        }
    }

    private void InstantiateAndAddNewTableware()
    {
        GameObject newTablewareObject = Instantiate(TablewarePrefab);
        Tableware newTableware = newTablewareObject.GetComponentInChildren<Tableware>();
        newTableware.OnDestroyed += (delay) => 
            StartCoroutine(WaitForDeactivateTablewareInstance(newTablewareObject, newTableware, delay));
        newTablewareObject.transform.SetParent(transform);
        newTablewareObject.transform.localPosition = Vector3.zero;
        newTablewareObject.SetActive(false);
        tableware.Add(newTablewareObject);
    }

    private IEnumerator WaitForDeactivateTablewareInstance(GameObject tablewareObject, Tableware tableWare, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateTablewareInstance(tablewareObject, tableWare);
    }
	
    private void DeactivateTablewareInstance(GameObject tablewareObject, Tableware tableWare)
    {
        tablewareObject.SetActive(false);
        tableWare.MainObject.SetActive(true);
        tableWare.ParticleSystem.SetActive(false);
    }
}

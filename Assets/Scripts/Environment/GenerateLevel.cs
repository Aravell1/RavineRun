using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] section;
    public GameObject[] decoObjects;
    public GameObject[] obstacles;
    public GameObject[] collectibles;
    public List<GameObject> createdSections;


    private int secNum;
    private float zPos;
    private float sectionLength;
    //private bool creatingSection = false;

    private void Start()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
        {
            sectionLength = LevelBoundary.baseFloor.transform.localScale.z;

            GenerateSection();
            GenerateSection();
            GenerateSection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
        {
            if (createdSections[0].transform.position.z < -50)
            {
                GenerateSection();
                Destroy(createdSections[0]);
                createdSections.RemoveAt(0);
            }
        }
    }

    public void GenerateSection()
    {
        zPos = createdSections[^1].transform.position.z + sectionLength;
        secNum = Random.Range(0, section.Length);
        AddCreatedSection(Instantiate(section[secNum], new Vector3(0, 0, zPos), Quaternion.identity));

    }

    public void AddCreatedSection(GameObject section)
    {
        createdSections.Add(section);
    }
}

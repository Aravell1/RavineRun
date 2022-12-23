using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectType type;
    public GenerateLevel levelManager;

    public enum ObjectType
    {
        Decorative,
        Obstacle,
        Collectible
    }

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<GenerateLevel>();

        if (type == ObjectType.Decorative)
        {
            int rndObject = Random.Range(0, levelManager.decoObjects.Length);
            GameObject newObject = Instantiate(levelManager.decoObjects[rndObject], 
                transform.position, 
                Quaternion.Euler(transform.rotation.x, Random.Range(-180f, 180f), transform.rotation.z));
            newObject.transform.parent = gameObject.transform;
        }
        else if (type == ObjectType.Collectible)
        {
            int rndObject = Random.Range(0, levelManager.collectibles.Length);
            GameObject newObject = Instantiate(levelManager.collectibles[rndObject],
                new Vector3(Random.Range(LevelBoundary.leftSide + 1f, LevelBoundary.rightSide - 1f), transform.position.y, transform.position.z), 
                transform.rotation);
            newObject.transform.parent = gameObject.transform;
            
        }
        else if (type == ObjectType.Obstacle)
        {
            int rndObject = Random.Range(0, levelManager.obstacles.Length);
            if (rndObject > 1)
            {
                GameObject newObject = Instantiate(levelManager.obstacles[rndObject],
                    new Vector3(Random.Range(LevelBoundary.leftSide + 1f, LevelBoundary.rightSide - 1f), transform.position.y, transform.position.z),
                    Quaternion.Euler(transform.rotation.x, Random.Range(-180f, 180f), transform.rotation.z));
                newObject.transform.parent = gameObject.transform;
            }
            else
            {
                GameObject newObject = Instantiate(levelManager.obstacles[rndObject],
                    transform.position,
                    transform.rotation);
                newObject.transform.parent = gameObject.transform;
            }
        }
    }

}

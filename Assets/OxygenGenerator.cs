using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SAME AS ASTEROID GENERATOR BUT FOR O2 TANKS
public class OxygenGenerator : MonoBehaviour
{
    public Transform oxygenTankPrefab;
    public int fieldRadius = 200;
    public int tankCount = 20;
    public float minTankRadius = 0.5f;
    public float maxTankRadius = 5f;

    void Start()
    {
        for (int i = 0; i < tankCount; i++)
        {
            Transform temp = Instantiate(oxygenTankPrefab, Random.insideUnitSphere * fieldRadius, Random.rotation);
            temp.localScale = temp.localScale * Random.Range(minTankRadius, maxTankRadius);

        }
    }


}
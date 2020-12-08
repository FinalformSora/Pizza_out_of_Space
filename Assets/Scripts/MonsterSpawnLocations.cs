using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnLocations
{
    private List<Vector3> upperLocations;
    private List<Vector3> lowerLocations;

    public MonsterSpawnLocations()
    {
        upperLocations = new List<Vector3>();
        lowerLocations = new List<Vector3>();

        upperLocations.Add(new Vector3(-63, 21, -10));
        upperLocations.Add(new Vector3(-40, 21, 53));
        upperLocations.Add(new Vector3(-80, 21, 90));
        upperLocations.Add(new Vector3(42, 21, 0));
        upperLocations.Add(new Vector3(42, 21, 62));
        upperLocations.Add(new Vector3(-61.5f, 21, 87));
        upperLocations.Add(new Vector3(-80, 21, 76));
        upperLocations.Add(new Vector3(-93, 21, 57));
        upperLocations.Add(new Vector3(45, 21, 71));
        upperLocations.Add(new Vector3(-94, 21, 26));

        lowerLocations.Add(new Vector3(-78, 1, 88));
        lowerLocations.Add(new Vector3(0, 1, 13));
        lowerLocations.Add(new Vector3(90, 1, 15));
        lowerLocations.Add(new Vector3(70, 1, -30));
        lowerLocations.Add(new Vector3(0, 1, -30));
        lowerLocations.Add(new Vector3(-35, 1, -30));
        lowerLocations.Add(new Vector3(-80, 1, -10));
        lowerLocations.Add(new Vector3(13, 1, 90));
        lowerLocations.Add(new Vector3(94, 1, 60));
    }

    public List<Vector3> getUpper()
    {
        return upperLocations;
    }

    public List<Vector3> getLower()
    {
        return lowerLocations;
    }
}

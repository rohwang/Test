using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapDataSO : ScriptableObject
{

    public SingleData[] dataList;

    public string GetRandomScene()
    {
        int size = dataList.Length;

        return dataList[Random.Range(0, size)].name;
    }

    public string GetSceneName(int index)
    {
        for (int i = 0; i < dataList.Length; i++)
        {
            if (dataList[i].id == index)
            {
                return dataList[i].name;
            }
        }

        return null;
    }

}

[System.Serializable]
public struct SingleData
{
    public int id;
    public string name;
}

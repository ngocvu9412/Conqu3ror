using UnityEngine;

[CreateAssetMenu(fileName = "PointInfosDatabase", menuName = "PointInfos/PointInfos database")]
public class PointInfosDatabase : ScriptableObject
{
    public PointInfo[] PointInfos;

    public int PointInfosCount
    {
        get { return PointInfos.Length; }
    }

    public PointInfo GetPointInfos(int index)
    {
        return PointInfos[index];
    }
    public int GetIndex(PointInfo pointinf)
    {
        return System.Array.IndexOf(PointInfos, pointinf);
    }
}

public static class PointInforDatabaseManager
{
    private static PointInfosDatabase _pointinfoDB;

    // Truy cập CharactersDatabase thông qua class tĩnh
    public static PointInfosDatabase GetDatabase()
    {
        if (_pointinfoDB == null)
        {
            _pointinfoDB = Resources.Load<PointInfosDatabase>("Database/PointInfosDatabase");
            if (_pointinfoDB == null)
            {
                Debug.LogError("CharactersDatabase instance not found in Resources.");
            }
        }
        return _pointinfoDB;
    }
}

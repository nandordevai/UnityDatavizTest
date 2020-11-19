using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Plotter : MonoBehaviour
{
    public string dataFile;
    public GameObject PointPrefab;
    public GameObject PointHolder;

    float lookSpeed = 3;
    Vector2 rotation = Vector2.zero;
    List<Dictionary<string, object>> pointList;

    List<Dictionary<string, object>> Load()
    {
        TextAsset data = Resources.Load(dataFile) as TextAsset;
        var lines = data.text.Split('\n');
        var header = lines[0].Split(',');
        var rows = new List<Dictionary<string, object>>();
        foreach (var line in lines.Skip(1))
        {
            var values = line.Split(',');
            var entry = new Dictionary<string, object>();
            for (var i = 0; i < header.Length; i++)
            {
                string tempValue = (string)values[i];
                object value = tempValue;
                int n;
                float f;
                if (float.TryParse(tempValue, out f))
                    value = f;
                else if (int.TryParse(tempValue, out n))
                    value = n;
                entry[header[i]] = value;
            }
            rows.Add(entry);
        }
        return rows;
    }

    void Start() {
        pointList = Load();
        for (var i = 0; i < pointList.Count; i++)
        {
            float x = Convert.ToSingle(pointList[i]["id"]) / 10;
            float y = Convert.ToSingle(pointList[i]["Sepal.Length"]) * 2 - 7;
            float z = Convert.ToSingle(pointList[i]["Sepal.Width"]);
            GameObject p = Instantiate(PointPrefab, new Vector3(x, y , z), Quaternion.identity);
            p.transform.parent = PointHolder.transform;
            p.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);
        }
    }

    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        Camera.main.transform.localRotation = Quaternion.Euler(
            rotation.x * lookSpeed,
            rotation.y * lookSpeed,
            0
        );
    }
}

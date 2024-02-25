using UnityEngine;
using ChartAndGraph;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class GenerateLR : MonoBehaviour
{
    public GraphChart chart;
    public GameObject controller;

   /* public Material lineMaterial, poObscuredIntMaterial, fillMaterial;
    public ObscuredDouble lineThickness = 2.0, poObscuredIntSize = 5.0;
    public ObscuredBool stertchFill = false;*/
    

    private void Start()
    {
        /*var lineTiling = new MaterialTiling(true, 20);
        chart.DataSource.AddCategory("Temperature", lineMaterial, lineThickness, lineTiling, fillMaterial, stertchFill, poObscuredIntMaterial, poObscuredIntSize);*/
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("Temperature");
        for(ObscuredInt i = 0;i<= ObscuredPrefs.GetInt("Year");i++)
        {
            if (chart.DataSource.HasCategory("Temperature"))
            {
                Debug.Log("Succeeded");
                chart.DataSource.AddPointToCategory("Temperature", i+2020, ObscuredPrefs.GetFloat("IncreasedTemp" + i));
                
            }
        }
        chart.DataSource.EndBatch();
    }

}
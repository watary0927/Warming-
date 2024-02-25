﻿using UnityEngine;
using ChartAndGraph;
using System.Collections;

public class GraphChartFeed : MonoBehaviour
{
	void Start ()
    {
        GraphChartBase graph = GetComponent<GraphChartBase>();
        if (graph != null)
        {
            graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("Player 1");
            graph.DataSource.ClearAndMakeBezierCurve("Player 2");
            
            for (int i = 0; i <5; i++)
            {
                graph.DataSource.AddPointToCategory("Player 1",i,Random.value*10f + 20f);
                if (i == 0)
                    graph.DataSource.SetCurveInitialPoint("Player 2",i, Random.value * 10f + 10f);
                else
                    graph.DataSource.AddLinearCurveToCategory("Player 2", 
                                                                    new DoubleVector2(i , Random.value * 10f + 10f));
            }
            graph.DataSource.MakeCurveCategorySmooth("Player 2");
            graph.DataSource.EndBatch();
        }
       // StartCoroutine(ClearAll());
    }

    IEnumerator ClearAll()
    {
        yield return new WaitForSeconds(5f);
        GraphChartBase graph = GetComponent<GraphChartBase>();

        graph.DataSource.Clear();
    }
}

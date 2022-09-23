using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [SerializeField] private int grid_width, grid_height;
    private enum Shape {circle, square, poligon };
    //[SerializeField] private Shape grid_shape = Shape.circle;
    [SerializeField] private RuleTile ruleTilePrefab;

    void Start()
    {
        generateGrid();
        
    }

    void generateGrid()
    {
        for (int i = 0; i < grid_width; i++)
        {
            for (int j = 0; j < grid_height; j++)
            {
                var spawned_title = Instantiate(ruleTilePrefab, new Vector3(i, j, -10), Quaternion.identity);
                spawned_title.name = $"Tile ({i},{j})";
                print(spawned_title.name);
            }
        }
    }
}

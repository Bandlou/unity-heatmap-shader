using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bdl.Utils.Grid
{
    public class Grid<TGridObject>
    {
        // EVENTS
        public delegate void GridValueChangedAction(int x, int y);
        public event GridValueChangedAction OnGridValueChanged;

        // PROPERTIES
        public int Width { get => width; }
        public int Height { get => height; }
        public float CellSize { get => cellSize; }

        // PRIVATE FIELDS
        private int width;
        private int height;
        private float cellSize;
        private Vector3 origin;
        private TGridObject[,] gridArray;

        // CONSTRUCTOR
        public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); ++x)
                for (int y = 0; y < gridArray.GetLength(1); ++y)
                    gridArray[x, y] = createGridObject(this, x, y);

            bool debug = true;
            if (debug)
            {
                for (int x = 0; x < gridArray.GetLength(0); ++x)
                {
                    for (int y = 0; y < gridArray.GetLength(1); ++y)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(gridArray.GetLength(0), 0), GetWorldPosition(gridArray.GetLength(0), gridArray.GetLength(1)), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(0, gridArray.GetLength(1)), GetWorldPosition(gridArray.GetLength(0), gridArray.GetLength(1)), Color.white, 100f);
            }
        }

        // PUBLIC METHODS

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(x, y);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (IsXYValid(x, y))
            {
                gridArray[x, y] = value;
                OnGridValueChanged?.Invoke(x, y);
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (IsXYValid(x, y))
                return gridArray[x, y];
            else
                return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetGridObject(x, y);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + origin;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition.x - origin.x) / cellSize);
            y = Mathf.FloorToInt((worldPosition.y - origin.y) / cellSize);
        }

        public bool IsXYValid(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}

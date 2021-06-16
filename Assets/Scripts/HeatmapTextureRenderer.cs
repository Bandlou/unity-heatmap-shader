using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bdl.Utils.Grid;

namespace Heatmap
{
    public class HeatmapTextureRenderer : MonoBehaviour
    {
        // PUBLIC FIELDS
        public Vector2Int gridSize = new Vector2Int(64, 32);
        public float cellSize = .25f;
        public int cellTypeCount = 28;
        public int cellTypeRowCount = 10;
        public int cellTypeColumnCount = 3;
        public float drawingAccuracy = .1f;
        public float heatSpeed = .05f;
        public int heatSpread = 32;
        public float coolingSpeed = .0015f;
        public new Camera camera;
        public Material material;

        // PRIVATE FIELDS
        private Grid<float> grid;
        private MeshFilter meshFilter;
        private ComputeBuffer gridValuesBuffer;

        // LIFECYCLE

        private void Awake()
        {
            grid = new Grid<float>(gridSize.x, gridSize.y, cellSize, Vector3.zero, (g, x, y) => 0);
            meshFilter = GetComponent<MeshFilter>();

            DrawGrid(grid);
        }

        private void Start()
        {
            camera.transform.position = new Vector3(grid.Width * grid.CellSize * .5f, grid.Height * grid.CellSize * .5f, camera.transform.position.z);

            material.SetInt("_GridWidth", grid.Width);
            material.SetInt("_GridHeight", grid.Height);
            material.SetInt("_CellTypeCount", cellTypeCount);
            material.SetInt("_CellTypeRowCount", cellTypeRowCount);
            material.SetInt("_CellTypeColumnCount", cellTypeColumnCount);
            material.SetFloat("_DrawingAccuracy", drawingAccuracy);
            UpdateMaterialGridValues();
        }

        private void Update()
        {
            // Camera movement
            var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            camera.transform.position += movement;
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            camera.orthographicSize += zoom;

            // User heating
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(mouseWorldPosition, out int heatX, out int heatY);
            for (int xOffset = -heatSpread; xOffset <= heatSpread; ++xOffset)
            {
                for (int yOffset = -heatSpread; yOffset <= heatSpread; ++yOffset)
                {
                    float ratio = 1f / (Mathf.Pow(xOffset, 2) + Mathf.Pow(yOffset, 2) + 1);
                    float heatedValue = Mathf.Clamp01(grid.GetGridObject(heatX + xOffset, heatY + yOffset) + heatSpeed * ratio);
                    grid.SetGridObject(heatX + xOffset, heatY + yOffset, heatedValue);
                }
            }

            // Base cooling
            for (int x = 0; x < grid.Width; ++x)
            {
                for (int y = 0; y < grid.Height; ++y)
                {
                    float cooledValue = Mathf.Clamp(grid.GetGridObject(x, y) - coolingSpeed, 0, 100);
                    grid.SetGridObject(x, y, cooledValue);
                }
            }

            // Graphical update
            UpdateMaterialGridValues();
        }

        private void OnDestroy()
        {
            gridValuesBuffer?.Release();
        }

        // PRIVATE METHODS

        private void DrawGrid(Grid<float> grid)
        {
            meshFilter.mesh = new Mesh
            {
                vertices = new Vector3[4]
                {
                    new Vector3(0, 0),
                    new Vector3(0, grid.Height * grid.CellSize),
                    new Vector3(grid.Width * grid.CellSize, 0),
                    new Vector3(grid.Width * grid.CellSize, grid.Height * grid.CellSize)
                },
                uv = new Vector2[4] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) },
                triangles = new int[6] { 0, 1, 2, 2, 1, 3 }
            };
        }

        private void UpdateMaterialGridValues()
        {
            // Get grid values
            var gridValues = new float[grid.Width * grid.Height];
            for (int i = 0; i < gridValues.Length; i++)
                gridValues[i] = grid.GetGridObject(i % grid.Width, i / grid.Width);

            // Set material buffer from grid values
            int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(float));
            gridValuesBuffer?.Release();
            gridValuesBuffer = new ComputeBuffer(grid.Width * grid.Height, stride);
            gridValuesBuffer.SetData(gridValues);
            material.SetBuffer("_GridValuesBuffer", gridValuesBuffer);
        }
    }
}

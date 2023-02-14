using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos = true;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private Node[,] grid;
    public TerrainType[] walkableRegions;

    private LayerMask walkableMask;
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    [SerializeField]
    private int blurPenaltyMultipliyer = 3;

    [SerializeField]
    private int obstacleMovementPenalty = 5;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType terrain in walkableRegions)
        {
            //Stack the value of the layer mask with a bit array of their values
            // 1000000000 ---> 512 And that would be layer mask number 9, Road.
            // 10000000000 ---> 1024 And that would be layer mask number 10, Grass.
            walkableMask.value += terrain.terrainMask.value;
            //Save the value of the layer mask on a dictionary as well as it's penalty to acces it later.
            walkableRegionsDictionary.Add((int)Mathf.Log(terrain.terrainMask.value, 2), terrain.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize()
    {
        return gridSizeX * gridSizeY;
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Set the starting worldPoint of the grid, on the bottom left corner of the former grid, and check for non walkable tiles
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                //Raycast to check the movement penalty layer value

                //Throw a ray to the ground from the node position
                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                //Check which layer the ray hits
                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    //Look for the movement penalty applied to that layer in the dictionary and add it to the movement penalty
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                //We want the players to not get closer to obstacles, even if they are the closest route.
                if (!walkable)
                {
                    movementPenalty += obstacleMovementPenalty;
                }

                //Then, add a node each tile
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(blurPenaltyMultipliyer);
    }

    //????¿¿¿¿//
    private void BlurPenaltyMap(int blurSize)
    {
        //The size of the kernel square to blur
        int kernelSize = blurSize * 2 + 1;
        //How many squares there are between the center of the kernel and it's border
        int kernelExtents = (kernelSize - 1) / 2;

        //Two arrays to cover the grid bluring the numbers and store those values
        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        //Ni puta idea de que pasa aquí socio
        //Se recorren los kernels avanzando por el mapa que tenemos, pero no se que pasa dentro

        //Vertical Pass
        for (int y = 0; y < gridSizeY; y++)
        {
            //Kernel pass
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);

                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            //Advance column
            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        //Horizontal Pass
        for (int x = 0; x < gridSizeX; x++)
        {
            //Kernel Pass
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(x, 0, kernelExtents);

                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            //Adavance row and add the total penalties to the blurred total
            for (int y = 1; y < gridSizeX; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];

                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));

                grid[x, y].movementPenalty = blurredPenalty;
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //Buscamos el valor porcentual de donde se encuentra en la grid
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        //Lo convertimos a porcentaje
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //Lo pasamos a coordenadas de x e y de la grid
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNeightbours(Node node)
    {
        List<Node> neightbours = new List<Node>();

        //We check all the adjacent nodes of the current one we have to get a list of possibles moves
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //If x and y are 0 these means we are on our current node, thats not a neightbour, so we skip it
                if (x == 0 && y == 0)
                    continue;

                //Then we get the position from the node on the grid
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //Check if it's on the grid and not outside
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //If it's not outside then we add that node to the list
                    neightbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neightbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}

[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenalty;
}
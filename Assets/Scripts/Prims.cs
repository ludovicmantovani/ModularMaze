using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Generator
{
    public class Prims : Maze
    {
        #region Variables
        #endregion

        #region BuiltIn Methods
        #endregion

        #region Custom Methods
        public override void GenerateMap()
        {
            int x = 2;
            int z = 2;

            _map[x, z] = BlocType.CORRIDOR;

            List<MapLocation> walls = new List<MapLocation>();
            walls.Add(new MapLocation(x + 1, z));
            walls.Add(new MapLocation(x - 1, z));
            walls.Add(new MapLocation(x, z + 1));
            walls.Add(new MapLocation(x, z - 1));

            int countLoops = 0;
            while (walls.Count > 0 && countLoops <= 5000)
            {
                int randomWallIndex = Random.Range(0, walls.Count);
                x = walls[randomWallIndex].X;
                z = walls[randomWallIndex].Z;
                walls.RemoveAt(randomWallIndex);


                if (CountSquareNeighbours(x, z, BlocType.CORRIDOR) == 1)
                {
                    _map[x, z] = BlocType.CORRIDOR;
                    walls.Add(new MapLocation(x + 1, z));
                    walls.Add(new MapLocation(x - 1, z));
                    walls.Add(new MapLocation(x, z + 1));
                    walls.Add(new MapLocation(x, z - 1));
                }

                countLoops++;
            }
        }
        #endregion
    }
}
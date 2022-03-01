using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Generator
{
    public class Wilsons : Maze
    {
        #region Variables
        private List<MapLocation> _directions = new List<MapLocation>()
        {
            new MapLocation(1, 0),
            new MapLocation(0, 1),
            new MapLocation(-1, 0),
            new MapLocation(0, -1)
        };

        private List<MapLocation> _available = new List<MapLocation>();
        #endregion

        #region BuiltIn Methods
        #endregion

        #region Custom Methods
        public override void GenerateMap()
        {
            int startingX = Random.Range(2, _width - 1);
            int startingZ = Random.Range(2, _depth - 1);

            _map[startingX, startingZ] = BlocType.START_POINT;


            int test = 10;

            while (GetAvailableBlocs() > 1)
            {
                RandomWalk();
                test--;
            }

            for (int z = 0; z < _depth; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_map[x, z] == BlocType.START_POINT)
                    {
                        _map[x, z] = BlocType.CORRIDOR;
                    }
                }
            }

            _map[startingX, startingZ] = BlocType.START_POINT;

        }

        private int GetAvailableBlocs()
        {
            _available.Clear();
            for (int z = 1; z < _depth - 1; z++)
            {
                for (int x = 1; x < _width - 1; x++)
                {
                    if (CountSquareMazeNeighbours(x, z) == 0)
                    {
                        _available.Add(new MapLocation(x, z));
                    }
                }
            }
            return _available.Count;
        }


        private int CountSquareMazeNeighbours(int x, int z)
        {
            int count = 0;
            for (int d = 0; d < _directions.Count; d++)
            {
                int nx = x + _directions[d].X;
                int nz = z + _directions[d].Z;
                if (_map[nx, nz] == BlocType.START_POINT)
                {
                    count++;
                }
            }

            return count;
        }

        private void RandomWalk()
        {
            List<MapLocation> currentWalk = new List<MapLocation>();
            int randomStartIndex = Random.Range(0, _available.Count);
            int cx = _available[randomStartIndex].X;
            int cz = _available[randomStartIndex].Z;

            currentWalk.Add(new MapLocation(cx, cz));

            int countLoops = 0;
            bool isValisPath = false;

            while (cx > 0 && cx < _width - 1
                && cz > 0 && cz < _depth - 1
                && countLoops < 5000
                && !isValisPath)
            {
                _map[cx, cz] = BlocType.CORRIDOR;

                if (CountSquareMazeNeighbours(cx, cz) > 1)
                    break;

                int randomDirectionIndex = Random.Range(0, _directions.Count);

                int nextX = cx + _directions[randomDirectionIndex].X;
                int nextZ = cz + _directions[randomDirectionIndex].Z;
                if (CountSquareNeighbours(nextX, nextZ, BlocType.CORRIDOR) < 2)
                {
                    cx = nextX;
                    cz = nextZ;
                    currentWalk.Add(new MapLocation(cx, cz));
                }

                isValisPath = CountSquareMazeNeighbours(cx, cz) == 1;

                countLoops++;
            }

            if (isValisPath)
            {
                _map[cx, cz] = BlocType.CORRIDOR;
                Debug.Log("PathFound");

                foreach (MapLocation mapLocation in currentWalk)
                {
                    _map[mapLocation.X, mapLocation.Z] = BlocType.START_POINT;
                }
                currentWalk.Clear();
            }
            else
            {
                foreach (MapLocation mapLocation in currentWalk)
                {
                    _map[mapLocation.X, mapLocation.Z] = BlocType.WALL;
                }
                currentWalk.Clear();
            }
        }

        #endregion
    }
}
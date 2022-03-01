using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Generator
{
    public class Recursive : Maze
    {
        #region Variables
        private List<MapLocation> _directions = new List<MapLocation>()
        {
            new MapLocation(1, 0),
            new MapLocation(0, 1),
            new MapLocation(-1, 0),
            new MapLocation(0, -1)
        };
        #endregion

        public override void GenerateMap()
        {
            Generate(
                Random.Range(2, _width - 1),
                Random.Range(2, _depth - 1));
        }

        void Generate(int x, int z)
        {
            if (CountSquareNeighbours(x, z, BlocType.CORRIDOR) >= 2)
                return;

            _map[x, z] = BlocType.CORRIDOR;

            _directions.Shuffle();
            Generate(x + _directions[0].X, z + _directions[0].Z);
            Generate(x + _directions[1].X, z + _directions[1].Z);
            Generate(x + _directions[2].X, z + _directions[2].Z);
            Generate(x + _directions[3].X, z + _directions[3].Z);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Generator
{
    public class Crawler : Maze
    {
        #region Variables
        #endregion

        #region BuiltIn Methods
        #endregion

        #region Custom Methods
        public override void GenerateMap()
        {

            for (int i = 0; i < 3; i++)
            {
                CrawlH();
            }

            for (int i = 0; i < 2; i++)
            {
                CrawlV();
            }

        }


        void CrawlV()
        {
            bool done = false;
            int x = Random.Range(1,_width - 1);
            int z = 1;

            int last_x = x;
            int last_z = z;

            _map[x, z] = BlocType.START_POINT;

            while (!done)
            {
                if (_map[x, z] == BlocType.WALL)
                {
                    _map[x, z] = BlocType.CORRIDOR;
                }

                last_x = x;
                last_z = z;
                if (Random.Range(0, 100) < 50)
                {
                    x += Random.Range(-1, 2);
                }
                else
                {
                    z += Random.Range(0, 2);
                }

                if (x < 1 || x >= _width -1 ||
                    z < 1 || z >= _depth -1)
                {
                    done = true;
                }
                else
                {
                    done = false;
                }
            }
            _map[last_x, last_z] = BlocType.END_POINT;
        }


        void CrawlH()
        {
            bool done = false;
            int x = 1;
            int z = Random.Range(1, _depth - 1);

            int last_x = x;
            int last_z = z;

            _map[x, z] = BlocType.START_POINT;

            while (!done)
            {
                if (_map[x, z] == BlocType.WALL)
                {
                    _map[x, z] = BlocType.CORRIDOR;
                }

                last_x = x;
                last_z = z;
                if (Random.Range(1, 100) < 50)
                {
                    x += Random.Range(0, 2);
                }
                else
                {
                    z += Random.Range(-1, 2);
                }

                if (x < 1 || x >= _width -1 ||
                    z < 1 || z >= _depth -1)
                {
                    done = true;
                }
                else
                {
                    done = false;
                }
            }
            _map[last_x, last_z] = BlocType.END_POINT;
        }
        #endregion
    }
}
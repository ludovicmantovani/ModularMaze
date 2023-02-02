using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Generator
{
    public enum BlocType
    {
        CORRIDOR,
        WALL,
        START_POINT,
        END_POINT,
        ALL_TYPE
    }

/*    public enum DeadEndType
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }*/

    public class MapLocation
    {
        private int _x;
        public int X
        {
            get { return _x; }
        }

        private int _z;
        public int Z
        {
            get { return _z; }
        }

        public MapLocation(int x, int z)
        {
            _x = x;
            _z = z;
        }

    }

    public class Maze : MonoBehaviour
    {
        #region Variables
        /*        [SerializeField] private GameObject _wallPrefab;
                [SerializeField] private GameObject _startPointPrefab;
                [SerializeField] private GameObject _endPointPrefab;
                [SerializeField] private GameObject _floorPrefab;*/


        [Header("Maze Settings")]
        [SerializeField] protected int _width = 30; //x length
        public int Width
        {
            get{
                return _width;
            }
        }
        [SerializeField] protected int _depth = 30; //z length
        public int Depth
        {
            get
            {
                return _depth;
            }
        }
        [SerializeField] private MimiMap _minimap;
        [SerializeField] private GameManager _gameManager;

        [Header("Model Prefab Settings")]
        [SerializeField] private int _scale = 6;
        [SerializeField] private GameObject _straightPrefab;
        [SerializeField] private GameObject _crossRoadPrefab;
        [SerializeField] private GameObject _deadEndPrefab;
        [SerializeField] private GameObject _cornerPrefab;
        [SerializeField] private GameObject _tJunctionPrefab;
        [SerializeField] private GameObject _itemPrefab;

        [Header("Player Prefab Settings")]
        [SerializeField] private GameObject _playerPrefab;



        private Vector3 _pos = Vector3.zero;
        private GameObject _mazeGeneration;

        protected BlocType[,] _map;
        private List<MapLocation> _itemsMap = new List<MapLocation>();

        //private Dictionary<int[,], DeadEndType> _deadEnd;
        //private List<int[,]> _deadEnd;

        /*        public BlocType[,] Map
                {
                    get
                    {
                        return _map;
                    }
                }*/

        //private Vector3 _scaleVector = Vector3.zero;


        public MapLocation startingPoint;
        #endregion

        #region BuiltIn Methods
        void Start()
        {
            _width = LevelManager.maseSize;
            _depth = LevelManager.maseSize;
            InitialiseMap();
            GenerateMap();
            DrawMap();
            PlaceItem();
            _minimap.DrawMiniMap(this);
        }


        #endregion

        #region Custom Methods




        private void InitialiseMap()
        {
            _map = new BlocType[_width, _depth];
            for (int z = 0; z < _depth; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _map[x, z] = BlocType.WALL;                    
                }
            }
        }

        public virtual void GenerateMap()
        {
            for (int z = 0; z < _depth; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        _map[x, z] = BlocType.CORRIDOR;
                    }
                }
            }
        }

        private void DrawMap()
        {
            //_deadEnd = new Dictionary<int[,], DeadEndType>();
            //_deadEnd = new List<int[,]>();
            _mazeGeneration = new GameObject("Generated Map");
            _mazeGeneration.transform.SetParent(transform);
            for (int z = 0; z < _depth; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_map[x, z] == BlocType.WALL)
                    {
/*                        _pos.Set(x * _scale, 0, z * _scale);
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.localScale *= _scale;
                        wall.transform.position = _pos;*/
                    }
                    else if (_map[x, z] == BlocType.CORRIDOR)
                    {
                        // Vertical corridor
                        if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.ALL_TYPE,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            Instantiate(_straightPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                        }
                        // Horizontal corridor
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_straightPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 90, 0);
                        }

                        // Crossroad
                        else if (Search(x, z, new BlocType[] {
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            Instantiate(_crossRoadPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                        }

                        // Horizontal dead end right
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_deadEndPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 90, 0);
                            //_deadEnd.Add(new int[x, z], DeadEndType.RIGHT);
                            //_deadEnd.Add(new int[x, z]);
                            startingPoint = new MapLocation(x, z);
                            _playerPrefab.transform.position = go.transform.position;
                        }
                        // Horizontal dead end left
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_deadEndPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, -90, 0);
                            //_deadEnd.Add(new int[x, z], DeadEndType.LEFT);
                            //_deadEnd.Add(new int[x, z]);

                        }
                        // Vertical dead end up
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            Instantiate(_deadEndPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            //_deadEnd.Add(new int[x, z], DeadEndType.UP);
                            //_deadEnd.Add(new int[x, z]);
                        }
                        // Vertical dead end down
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.ALL_TYPE,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_deadEndPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 180, 0);
                            //_deadEnd.Add(new int[x, z], DeadEndType.DOWN);
                            //_deadEnd.Add(new int[x, z]);
                        }

                        // Up right corner
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_cornerPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 90, 0);
                        }
                        // Up left corner
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.WALL}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            Instantiate(_cornerPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                        }
                        // Down left corner
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_cornerPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, -90, 0);
                        }
                        // Down right corner
                        else if (Search(x, z, new BlocType[] {
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_cornerPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 180, 0);
                        }

                        // Down T junction
                        else if (Search(x, z, new BlocType[] {
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_tJunctionPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, -90, 0);
                        }
                        // T junction
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.WALL, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.WALL}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_tJunctionPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 90, 0);
                        }
                        // -| junction
                        else if (Search(x, z, new BlocType[] {
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.ALL_TYPE,
                            BlocType.CORRIDOR, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.ALL_TYPE}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            GameObject go = Instantiate(_tJunctionPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                            go.transform.Rotate(0, 180, 0);
                        }
                        // |- junction
                        else if (Search(x, z, new BlocType[] {
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.WALL,
                            BlocType.WALL, BlocType.CORRIDOR, BlocType.CORRIDOR,
                            BlocType.ALL_TYPE, BlocType.CORRIDOR, BlocType.WALL}))
                        {
                            _pos.Set(x * _scale, 0, z * _scale);
                            Instantiate(_tJunctionPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                        }
                    }
                }
            }
        }

        public bool IsEmpty(int x, int z)
        {
            return _map[z, x] != BlocType.WALL;
        }

        public bool IsItem(int x, int z)
        {
            bool isItem = false;
            foreach (MapLocation location in _itemsMap)
            {
                if (location.X == z && location.Z == x)
                {
                    isItem = true;
                    break;
                }
            }
            return isItem;
        }

        private int CountEmpty()
        {
            int count = 0;
            for (int z = 0; z < _depth; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_map[x, z] != BlocType.WALL)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void PlaceItem()
        {
            int nbItems = 1;
            if (LevelManager.maseSize > 0)
            {
                int empty = CountEmpty();
                nbItems = (int)((float)empty / 10);
            }
            _gameManager.SetNbrItems(nbItems);
            while (nbItems > 0)
            {
                int randomX = Random.Range(0, _width);
                int randomZ = Random.Range(0, _depth);

                if (_map[randomX, randomZ] != BlocType.WALL)
                {
                    nbItems--;
                    _itemsMap.Add(new MapLocation(randomX, randomZ));
                    _pos.Set(randomX * _scale, -1f, randomZ * _scale);
                    GameObject go = Instantiate(_itemPrefab, _pos, Quaternion.identity, _mazeGeneration.transform);
                    go.GetComponent<ItemController>().SetGameManager(_gameManager);
                }
            }
        }
        private bool Search(int c, int r, BlocType[] pattern)
        {
            int count = 0;
            int pos = 0;

            for (int z = 1; z > -2; z--)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (pattern[pos] == BlocType.ALL_TYPE || pattern[pos] == _map[c + x, r + z])
                    {
                        count++;
                    }
                    pos++;
                }
            }
            return(count == 9);
        }

        /// <summary>
        /// Count number of specific bloc type around a bloc.
        /// Ignore diagonal neighbours.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blocType"></param>
        /// <returns></returns>
        protected int CountSquareNeighbours(int x, int z, BlocType blocType)
        {
            int count = 0;

            if (x <= 0 || x >= _width - 1 || z <= 0 || z >= _depth - 1) return 5;

            if (_map[x - 1, z] == blocType) count++;
            if (_map[x + 1, z] == blocType) count++;
            if (_map[x, z + 1] == blocType) count++;
            if (_map[x ,z - 1] == blocType) count++;
            return count;
        }


        /// <summary>
        /// Count number of specific bloc type around a bloc.
        /// Ignore diagonal horizontal and vertical neighbours.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blocType"></param>
        /// <returns></returns>
        private int CountDiagonalNeighbours(int x, int z, BlocType blocType)
        {
            int count = 0;

            if (x <= 0 || x >= _width  - 1 || z <= 0 || z >= _depth - 1) return 5;

            if (_map[x - 1, z - 1] == blocType) count++;
            if (_map[x + 1, z + 1] == blocType) count++;
            if (_map[x - 1, z + 1] == blocType) count++;
            if (_map[x + 1, z - 1] == blocType) count++;
            return count;
        }

        /// <summary>
        /// Count number of specific bloc type around a bloc.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blocType"></param>
        /// <returns></returns>
        protected int CountAllNeighbours(int x, int z, BlocType blocType)
        {
            return CountSquareNeighbours(x, z, blocType) + CountDiagonalNeighbours(x, z, blocType);
        }

        #endregion
    }
}
using _Script.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimiMap : MonoBehaviour
{
    public void Start()
    {
/*        var tex = new Texture2D(2, 2, TextureFormat.RGB24, true);
        var data = new byte[]
        {
            // mip 0: 2x2 size
            255, 0, 0, // red
            0, 255, 0, // green
            0, 0, 255, // blue
            255, 235, 4, // yellow
            // mip 1: 1x1 size
            0, 255, 255 // cyan
        };
        tex.SetPixelData(data, 0, 0); // mip 0
        tex.SetPixelData(data, 1, 12); // mip 1
        tex.filterMode = FilterMode.Point;
        tex.Apply(updateMipmaps: false);

        GetComponent<RawImage>().texture = tex;*/
    }

    public void DrawMiniMap(Maze maze)
    {
        Texture2D mapTexture = new Texture2D(maze.Width, maze.Depth);
        Color color;

        for (int z = 0; z < maze.Depth; z++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                if (maze.IsEmpty(x, z))
                {
                    color = Color.white;
                    if (maze.IsItem(x, z))
                    {
                        color = Color.blue;
                    }
                }
                else
                {
                    color = Color.gray;
                }

                if (maze.startingPoint.X == z
                    && maze.startingPoint.Z == x)
                {
                    color = Color.yellow;
                }

                mapTexture.SetPixel(z, x, color);
            }
        }

        mapTexture.filterMode = FilterMode.Point;
        mapTexture.Apply(updateMipmaps: false);
        GetComponent<RawImage>().texture = mapTexture;
    }
}

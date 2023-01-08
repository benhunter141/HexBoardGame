#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#endif

public class CustomPrimitives
{
#if UNITY_EDITOR
    private static Mesh CreateTriangleMesh()
    {
        Vector3[] vertices = {
             new Vector3(-0.5f, -0.5f, 0),
             new Vector3(0.5f, -0.5f, 0),
             new Vector3(0f, 0.5f, 0)
         };

        Vector2[] uv = {
             new Vector2(0, 0),
             new Vector2(1, 0),
             new Vector2(0.5f, 1)
         };

        int[] triangles = { 0, 1, 2 };

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    private static Mesh Create3DTriangleMesh()
    {
        float width = 0.2f;
        float length = 0.2f;
        float height = 0.05f;
        Vector3 leftSide = (Vector3.left * width / 2 + Vector3.forward * length).normalized;
        Vector3 rightSide = (Vector3.right * width / 2 + Vector3.forward * length).normalized;
        Vector3[] vertices = new Vector3[18]
        {
            new Vector3(width/2, 0, 0),
                new Vector3(width / 2, 0, 0),
                new Vector3(width / 2, 0, 0),
            new Vector3(0, 0, length),
                new Vector3(0, 0, length),
                new Vector3(0, 0, length),
            new Vector3(-width/2, 0, 0),
                new Vector3(-width / 2, 0, 0),
                new Vector3(-width / 2, 0, 0),
            new Vector3(width/2, height, 0),
                new Vector3(width/2, height, 0),
                new Vector3(width / 2, height, 0),
            new Vector3(0, height, length),
                new Vector3(0, height, length),
                new Vector3(0, height, length),
            new Vector3(-width/2, height, 0),
                new Vector3(-width / 2, height, 0),
                new Vector3(-width / 2, height, 0)
    };

        //uv not necessary... right?
        int[] triangles = {
            0, 16, 9, //1            
            1, 10, 3,
            3, 10, 12,
            4, 13, 6,
            6, 13, 15,
            0, 7, 16, //6
            11, 17, 14,
            2, 5, 8
        };

        Vector3[] normals = new Vector3[18]
    {
         Vector3.back,  //A
         rightSide,
         Vector3.down,

         rightSide,     //B
         leftSide,
         Vector3.down,

         leftSide,      //C
         Vector3.back,
         Vector3.down,

         Vector3.back,  //D
         rightSide,
         Vector3.up,

         rightSide,     //E
         leftSide,
         Vector3.up,

         leftSide,      //F
         Vector3.back,
         Vector3.up,
    };
        var mesh = new Mesh();
        Debug.Log($"norals length:{normals.Length} , verts length:{vertices.Length} , tris length:{triangles.Length} ");
        mesh.vertices = vertices;
        mesh.normals = normals;        
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }

    private static Mesh Create3DRing()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int n = 30;
        float radiusInner = 1f;
        float thickness = 0.1f;
        float radiusOuter = radiusInner + thickness;
        float height = 0.1f;
        float pi = Mathf.PI;

        //Outside ring
            for (int i = 0; i < n; i++) //foreach pair of points in the row
            {
                //Vertices
                float theta = -2 * pi / n * i;              //negative sign should flip the faces of the triangles
                float zCoord = radiusOuter * Mathf.Cos(theta);
                float yCoordBott = 0;
                float yCoordTop = height;
                float xCoord = radiusOuter * Mathf.Sin(theta);

                //Debug.Log($"theta:{theta} , xCoord:{xCoord} , zCoord:{zCoord} ");
                Vector3 v1 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v2 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v3 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v4 = new Vector3(xCoord, yCoordTop, zCoord);
                Vector3 v5 = new Vector3(xCoord, yCoordTop, zCoord);
                Vector3 v6 = new Vector3(xCoord, yCoordTop, zCoord);
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                vertices.Add(v4);
                vertices.Add(v5);
                vertices.Add(v6);
                //Triangles
                int i1 = 6 * i + 2;
                int i2 = 6 * i + 10;
                int i3 = 6 * i + 6;
                int i4 = 6 * i + 1;
                int i5 = 6 * i + 5;
                int i6 = 6 * i + 9;
                //if(lastPointsInRow) i2, i3 and i6 need to wrap (-30)
                if(i+1 == n) { i2 -= (n * 6); i3 -= (n * 6); i6 -= (n * 6); }
                //Debug.Log($"i1:{i1}, i2:{i2}, i3:{i3}, i4:{i4}, i5:{i5}, i6:{i6}");
                triangles.Add(i1);
                triangles.Add(i2);
                triangles.Add(i3);
                triangles.Add(i4);
                triangles.Add(i5);
                triangles.Add(i6);
            }           
        //Inside Ring
            for (int i = n; i < 2 * n; i++) //foreach pair of points in the row
            {
                //Vertices
                float theta = 2 * pi / n * i;
                float zCoord = radiusInner * Mathf.Cos(theta);
                float yCoordBott = 0;
                float yCoordTop = height;
                float xCoord = radiusInner * Mathf.Sin(theta);

                //Debug.Log($"theta:{theta} , xCoord:{xCoord} , zCoord:{zCoord} ");
                Vector3 v1 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v2 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v3 = new Vector3(xCoord, yCoordBott, zCoord);
                Vector3 v4 = new Vector3(xCoord, yCoordTop, zCoord);
                Vector3 v5 = new Vector3(xCoord, yCoordTop, zCoord);
                Vector3 v6 = new Vector3(xCoord, yCoordTop, zCoord);
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                vertices.Add(v4);
                vertices.Add(v5);
                vertices.Add(v6);
                //Triangles
                int i1 = 6 * i + 2;
                int i2 = 6 * i + 10;
                int i3 = 6 * i + 6;
                int i4 = 6 * i + 1;
                int i5 = 6 * i + 5;
                int i6 = 6 * i + 9;
                //if(lastPointsInRow) i2, i3 and i6 need to wrap (-30)
                if (i + 1 == 2*n) { i2 -= (n * 6); i3 -= (n * 6); i6 -= (n * 6); }
                //Debug.Log($"i1:{i1}, i2:{i2}, i3:{i3}, i4:{i4}, i5:{i5}, i6:{i6}");
                triangles.Add(i1);
                triangles.Add(i2);
                triangles.Add(i3);
                triangles.Add(i4);
                triangles.Add(i5);
                triangles.Add(i6);
            }
        //Top surface
        for (int i = 2*n; i < 3*n; i++) //foreach pair of points in the row
        {
            Debug.Log($"i is:{i}, should be 60. n is:{n}, should be 5?");
            //Vertices
            float theta = -2 * pi / n * i;              //negative sign should flip the faces of the triangles
            theta -= 4 * pi;
            float zCoordInner = radiusInner * Mathf.Cos(theta);
            float zCoordOuter = radiusOuter * Mathf.Cos(theta);
            float yCoord = height;
            float xCoordInner = radiusInner * Mathf.Sin(theta);
            float xCoordOuter = radiusOuter * Mathf.Sin(theta);
            Debug.Log($"theta:{theta} , xCoordOuter:{xCoordOuter} , zCoordOuter:{zCoordOuter} ");
            Debug.Log($"theta:{theta} , xCoordInner:{xCoordInner} , zCoordInner:{zCoordInner} ");
            Vector3 v1 = new Vector3(xCoordOuter, yCoord, zCoordOuter);
            Vector3 v2 = new Vector3(xCoordOuter, yCoord, zCoordOuter);
            Vector3 v3 = new Vector3(xCoordOuter, yCoord, zCoordOuter);
            Vector3 v4 = new Vector3(xCoordInner, yCoord, zCoordInner);
            Vector3 v5 = new Vector3(xCoordInner, yCoord, zCoordInner);
            Vector3 v6 = new Vector3(xCoordInner, yCoord, zCoordInner);
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            vertices.Add(v5);
            vertices.Add(v6);
            //Triangles
            int i1 = 6 * i + 2;
            int i2 = 6 * i + 10;
            int i3 = 6 * i + 6;
            int i4 = 6 * i + 1;
            int i5 = 6 * i + 5;
            int i6 = 6 * i + 9;
            //if(lastPointsInRow) i2, i3 and i6 need to wrap (-30)
            if (i + 1 == 3*n) { i2 -= (n * 6); i3 -= (n * 6); i6 -= (n * 6); }
            Debug.Log($"i1:{i1}, i2:{i2}, i3:{i3}, i4:{i4}, i5:{i5}, i6:{i6}");
            triangles.Add(i1);
            triangles.Add(i2);
            triangles.Add(i3);
            triangles.Add(i4);
            triangles.Add(i5);
            triangles.Add(i6);
        }




        Mesh mesh = new Mesh();
        Vector3[] vertArray = vertices.ToArray();
        mesh.vertices = vertArray;
        int[] triArray = triangles.ToArray();
        mesh.triangles = triArray;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        //if (backFaces) mesh = BackFaces(mesh);
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        //mesh.RecalculateTangents();

        return mesh;

    }

    private static Mesh Create3DEgg()
    {
        bool backFaces = false;
        /* Slime Dimensions
         * height = 1.6
         * eggwaistradius = 1.2
         * eggratio = 8
         */

        /*  Standard Egg dimensions:
         * 
         */

        float height = 1f;
        float eggWaistRadius = 0.45f; //waist diameter vs height
        float eggRatio = 3;  //ratio of top height to bottom height. Eg. eR = 4 means waist is at 20% height           
        List<Vector3> vertices = new List<Vector3>(); //total vertices long list count is n * 3j
        List<int> triangles = new List<int>();        //Added procedurally
        //using vertices.Add(v1);
        float vertsPerCircle = 18; //j
        float circleCount = 12; //n       minmum 3 for diamond shape, 6 will be blocky, 20?
        float rowCount = circleCount - 1;
        Debug.Log("Total Tri count should be: " + rowCount * vertsPerCircle * 2);
        Debug.Log("verts per circle: " + vertsPerCircle);
        Debug.Log("cicleCount: " + circleCount);        
        float eggBottomLength = height / (eggRatio + 1); //b bot
        float eggTopLength = eggRatio * height / (eggRatio + 1);
        float pi = Mathf.PI;


        //mesh math in here:
        for (float b = 0; b < rowCount; b++) //foreach row
        {
            Debug.Log("START OF ROW #:" + (b+1));
            //CALCULATE RADIUS FOR BOT OF ROW AND RADIUS FOR TOP OF ROW
            float rowBottomHeight = b * (eggBottomLength + eggTopLength) / rowCount; //height from base of egg
            Debug.Log("rowBotHeight:" + rowBottomHeight);

            float rowTopHeight = (b + 1) * (eggBottomLength + eggTopLength) / rowCount; //ERROR !!!!!!!!!!

            float y = rowBottomHeight - eggBottomLength; //height if origin at centre
            Debug.Log("y value(bot): " + y);
                float bBotRow; //THIS is used ONLY for radius calculation
            if (y > 0) bBotRow = eggTopLength;
            else bBotRow = eggBottomLength;
            Debug.Log("bbotrow: " + bBotRow);
            float rowBottomRadius = EggRadius(eggWaistRadius, bBotRow, y);
            Debug.Log("rowBot Radius: " + rowBottomRadius);
            //Debug.Log("rowtopheight should be 1.33, is: " + rowTopHeight);
            y = rowTopHeight - eggBottomLength;
            Debug.Log("y value(top): " + y);
                float bTopRow; 
            if (y > 0) bTopRow = eggTopLength;
            else bTopRow = eggBottomLength;
            Debug.Log("btoprow: " + bTopRow);
            float rowTopRadius = EggRadius(eggWaistRadius, bTopRow, y);
            Debug.Log("rowTopRadius: " + rowTopRadius);

            for (float i = 0; i < vertsPerCircle; i++) //foreach pair of verts at the top and bot of a row
            {
                //VERTICES
                float theta = -2*pi*i / vertsPerCircle;
                float xCoord = rowBottomRadius * Mathf.Sin(theta);
                float yCoord = rowBottomHeight;
                float zCoord = rowBottomRadius * Mathf.Cos(theta);
                Vector3 v1 = new Vector3(xCoord, yCoord, zCoord);
                Vector3 v2 = new Vector3(xCoord, yCoord, zCoord);
                Vector3 v3 = new Vector3(xCoord, yCoord, zCoord);
                xCoord = rowTopRadius * Mathf.Sin(theta);
                yCoord = rowTopHeight;
                zCoord = rowTopRadius * Mathf.Cos(theta);
                Vector3 v4 = new Vector3(xCoord, yCoord, zCoord);
                Vector3 v5 = new Vector3(xCoord, yCoord, zCoord);
                Vector3 v6 = new Vector3(xCoord, yCoord, zCoord);
                vertices.Add(v1); //vert 0, used in row to the left
                vertices.Add(v2); //vert 1, used below
                vertices.Add(v3); //vert 2, used below
                vertices.Add(v4); //vert 3, used in row to the left
                vertices.Add(v5); //vert 4, used in row to the left
                vertices.Add(v6); //vert 6, used below
                //TRIANGLES 1, 5, 9 and 2, 10, 6
                
                int nextRowStartingPoint = (6 * (int)vertsPerCircle * ((int)b + 1));
                int thisRowStartingPoint = (6 * (int)vertsPerCircle * ((int)b));
                int longListVertsPerRow = 6 * (int)vertsPerCircle;
                int startingPoint = (int)(6 * (vertsPerCircle * b + i));                    //starting point for the 6 vertices (2x3) declared here
                Debug.Log($"startingPoint: {startingPoint}, nextRowStartingPoint: {nextRowStartingPoint}");
                int i1 = startingPoint + 1;
                int i2 = startingPoint + 5;
                int i3 = startingPoint + 9;
                if (i3 >= nextRowStartingPoint) i3 -= longListVertsPerRow;
                int i4 = startingPoint + 2;
                int i5 = startingPoint + 10;
                if (i5 >= nextRowStartingPoint) i5 -= longListVertsPerRow;
                int i6 = startingPoint + 6;
                if (i6 >= nextRowStartingPoint) i6 -= longListVertsPerRow;
                  
                Debug.Log($"First Tri: {i1}, {i2}, {i3}.  Second Tri: {i4}, {i5}, {i6}.");
                triangles.Add(i1);
                triangles.Add(i2);
                triangles.Add(i3);
                triangles.Add(i4);
                triangles.Add(i5);
                triangles.Add(i6);


            }
        }

        //PUNCH HOLES
        //triangles.RemoveRange(48, 6);



        Mesh mesh = new Mesh();
        Vector3[] vertArray = vertices.ToArray();
        mesh.vertices = vertArray;
        int[] triArray = triangles.ToArray();
        mesh.triangles = triArray;
        Debug.Log("vert array length: " + vertArray.Length);
        Debug.Log("tri array length: " + triArray.Length);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        if (backFaces) mesh = BackFaces(mesh);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }

    private static Mesh Create3DHex()
    {
        bool backFaces = false;
        float radius = 1f;
        float height = 0.2f;
        List<Vector3> vertices = new List<Vector3>(); //
        List<int> triangles = new List<int>();        //
        
        for(int h = 0; h <= 1; h++)
        {
            float y = height * h;
            vertices.Add(new Vector3(0, y, 0)); //center vert
            for (int i = 0; i < 6; i++)
            {
                float theta = Mathf.PI / 3f * i;
                float x = Mathf.Cos(theta * radius);
                float z = Mathf.Sin(theta * radius);
                vertices.Add(new Vector3(x, y, z));
                if (h == 1)
                {
                    //top tris
                    Vector3 tri = new Vector3(7f, (9 + i), (8 + i));
                    triangles.Add(7);
                    if (i == 5) triangles.Add(8);
                    else triangles.Add(9 + i);
                    triangles.Add(8 + i);
                }
                //side tris
                if(i <= 4)
                {
                    triangles.Add(8 + i); //8,9,10,11,12 AND 8,9,10,11,12
                    triangles.Add(2 + 7*h + i); //2,3,4,5,6 AND 9,10,11,12,13
                    triangles.Add(1 + h + i); //3rd number 1,2,3,4,5,6 AND 2,3,4,5,6,1*
                }
                else
                {
                    if(h == 0)
                    {
                        triangles.Add(13);
                        triangles.Add(8);
                        triangles.Add(1);
                    }
                    else
                    {
                        triangles.Add(13);
                        triangles.Add(1);
                        triangles.Add(6);
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        Vector3[] vertArray = vertices.ToArray();
        mesh.vertices = vertArray;
        int[] triArray = triangles.ToArray();
        mesh.triangles = triArray;
        Debug.Log("vert array length: " + vertArray.Length);
        Debug.Log("tri array length: " + triArray.Length);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        if (backFaces) mesh = BackFaces(mesh);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }
    private static float EggRadius(float _a, float _b, float _y) //find the radius for each circle of verts going up the egg
    {
        return Mathf.Sqrt(_a * _a * (1 - _y * _y / (_b * _b)));
    }

    private static Mesh BackFaces(Mesh _mesh)
    {
        var mesh = _mesh;
        var vertices = mesh.vertices;
        //var uv = mesh.uv;
        //var normals = mesh.normals;
        var szV = vertices.Length;
        var newVerts = new Vector3[szV * 2];
        //var newUv = new Vector2[szV * 2];
        //var newNorms = new Vector3[szV * 2];
        for (var j = 0; j < szV; j++)
        {
            // duplicate vertices and uvs:
            newVerts[j] = newVerts[j + szV] = vertices[j];
            //newUv[j] = newUv[j + szV] = uv[j];
            // copy the original normals...
            //newNorms[j] = normals[j];
            // and revert the new ones
            //newNorms[j + szV] = -normals[j];
        }
        var triangles = mesh.triangles;
        var szT = triangles.Length;
        var newTris = new int[szT * 2]; // double the triangles
        for (var i = 0; i < szT; i += 3)
        {
            // copy the original triangle
            newTris[i] = triangles[i];
            newTris[i + 1] = triangles[i + 1];
            newTris[i + 2] = triangles[i + 2];
            // save the new reversed triangle
            var j = i + szT;
            newTris[j] = triangles[i] + szV;
            newTris[j + 2] = triangles[i + 1] + szV;
            newTris[j + 1] = triangles[i + 2] + szV;
        }
        mesh.vertices = newVerts;
        //mesh.uv = newUv;
        //mesh.normals = newNorms;
        mesh.triangles = newTris; // assign triangles last!
        return mesh;
    } //Adds backfaces

    private static GameObject CreateObject(string _objectToCreate)
    {
        var obj = new GameObject(_objectToCreate);
        var mesh = new Mesh();
        switch (_objectToCreate)    //creates the mesh from specific methods for each shape
        {
            case "Triangle":
                mesh = CreateTriangleMesh();        //2D single triangle
                break;
            case "3D Triangle":
                mesh = Create3DTriangleMesh();      //3D trianglular prism (2d tri + depth)
                break;
            case "Egg":
                mesh = Create3DEgg();
                break;
            case "Ring":
                mesh = Create3DRing();
                break;
            case "3D Hexagon":
                mesh = Create3DHex();
                break;
            default:
                
                break;
        }
        
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        //var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        //collider.sharedMesh = mesh;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    [MenuItem("GameObject/3D Object/Triangle", false, 0)]

    public static void CreateTriangle()
    {
        CreateObject("Triangle");
    }

    [MenuItem("GameObject/3D Object/3DTriangle", false, 0)]

    public static void Create3DTriangle()
    {
        CreateObject("3D Triangle");
    }

    [MenuItem("GameObject/3D Object/3DHex", false, 0)]
    public static void CreateHex()
    {
        CreateObject("3D Hexagon");
    }

    [MenuItem("GameObject/3D Object/Egg", false, 0)]

    public static void CreateEgg()
    {
        CreateObject("Egg");
    }
    [MenuItem("GameObject/3D Object/Ring", false, 0)]
    public static void CreateRing()
    {
        CreateObject("Ring");
    }


#endif
}
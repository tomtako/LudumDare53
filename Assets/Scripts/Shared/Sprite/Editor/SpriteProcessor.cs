using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class SpriteProcessor
{
    public const int PIXELS_PER_UNIT = 32;

    [MenuItem("Assets/Sprite/Process Sprite (Skip Reimport)", false, 0)]
    public static void ProcessSpriteSkipReimport()
    {
        string[] guids = Selection.assetGUIDs;

        if (null == guids || guids.Length == 0)
            return;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (null == texture)
                continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null) continue;
            if (Math.Abs(importer.spritePixelsPerUnit - PIXELS_PER_UNIT) < float.Epsilon) continue;

            importer.spritePixelsPerUnit = PIXELS_PER_UNIT;
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Point;
            importer.isReadable = true;
            importer.mipmapEnabled = false;


            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteExtrude = 0;
            settings.spriteGenerateFallbackPhysicsShape = false;
            importer.SetTextureSettings(settings);

            importer.isReadable = false;
            importer.SaveAndReimport();

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }


        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Sprite/Process Sprite", false, 0)]
    public static void processtSprite()
    {
        string[] guids = Selection.assetGUIDs;

        if (null == guids || guids.Length == 0)
            return;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (null == texture)
                continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.spritePixelsPerUnit = PIXELS_PER_UNIT;
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Point;
            importer.isReadable = true;
            importer.mipmapEnabled = false;


            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteExtrude = 0;
            settings.spriteGenerateFallbackPhysicsShape = false;
            importer.SetTextureSettings(settings);

            importer.isReadable = false;
            importer.SaveAndReimport();

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }


        AssetDatabase.Refresh();
    }

    public static void processtSprite2()
    {
        string[] guids = Selection.assetGUIDs;

        if (null == guids || guids.Length == 0)
            return;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (null == texture)
                continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.spritePixelsPerUnit = PIXELS_PER_UNIT;
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Point;
            importer.isReadable = true;
            importer.mipmapEnabled = false;


            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteExtrude = 0;
            settings.spriteGenerateFallbackPhysicsShape = false;
            importer.SetTextureSettings(settings);

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }


        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Sprite/Slice Sprite", false, 50)]
    public static void sliceSprite()
    {
        processtSprite2();

        string[] guids = Selection.assetGUIDs;

        if (null == guids || guids.Length == 0)
            return;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (null == texture)
                continue;

            if (!texture.name.Contains("_"))
            {
                Debug.LogWarningFormat("Could not process slice, asset needs an appended _WxH at the end of it's name: {0}", texture.name);
                return;
            }

            string[] split = texture.name.Split('_');

            if (split.Length != 2)
            {
                Debug.LogWarningFormat("Could not process slice, name is in incorrect format: {0}", texture.name);
            }

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null) continue;

            importer.isReadable = true;
            importer.spriteImportMode = SpriteImportMode.Multiple;

            string size = split[1];

            int width = int.Parse(size.Split('x')[0]);
            int height = int.Parse(size.Split('x')[1]);

            int rows = texture.height / height;
            int columns = texture.width / width;

            List<SpriteMetaData> metas = new List<SpriteMetaData>();

            int frame = 0;

            for (int r = rows - 1; r > -1; r--)
            {
                for (int c = 0; c < columns; c++)
                {
                    int x = c * width;
                    int y = r * height;

                    Color[] colors = texture.GetPixels(x, y, width, height);

                    if (colors.All(m => System.Math.Abs(m.a) < float.Epsilon))
                    {
                        continue;
                    }

                    SpriteMetaData meta = new SpriteMetaData();
                    meta.rect = new Rect(c * width, r * height, width, height);
                    meta.name = $"{texture.name}_{frame}";
                    metas.Add(meta);
                    frame++;
                }
            }

            importer.isReadable = false;
            importer.spritesheet = metas.ToArray();
            importer.SaveAndReimport();

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Sprite/Fix Physics Shapes", false, 50)]
    public static void fixPhysicsShapes()
    {
        processtSprite();

        string[] guids = Selection.assetGUIDs;

        if (null == guids || guids.Length == 0)
            return;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();

            for (int j = 0; j < sprites.Length; j++)
            {
                List<Vector2[]> vector2s = new List<Vector2[]>()
                {
                    new Vector2[]{  new Vector2(0,0), new Vector2(0,sprites[j].rect.size.y), new Vector2(sprites[j].rect.size.x,0), sprites[j].rect.size}
                };

                sprites[j].OverridePhysicsShape(vector2s);
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        AssetDatabase.Refresh();
    }
}

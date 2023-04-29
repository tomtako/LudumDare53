using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SpriteAnimationAssetCreation
{
    public const float DEFAULT_FRAME_TIME = 0.04F;
    public const string ANIMATION_SAVE_PATH = "Assets/Animations/";

    [MenuItem("Assets/Sprite/Create/Animation Asset")]
    public static void CreateCustom()
    {
        var asset = ScriptableObject.CreateInstance<SpriteAnimationAsset>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            asset.GetInstanceID(),
            ScriptableObject.CreateInstance<EndSpriteAnimationAssetNameEdit>(),
            "SpriteAnimationAsset.asset",
            AssetPreview.GetMiniThumbnail(asset),
            null);

    }

    [MenuItem("Assets/Sprite/Create/Animation")]
    public static void CreateSpriteAnimation()
    {
        string[] l_guids = Selection.assetGUIDs;

        if (null == l_guids || l_guids.Length == 0)
            return;

        for (int i = 0; i < l_guids.Length; i++)
        {
            string l_path = AssetDatabase.GUIDToAssetPath(l_guids[i]);
            Texture2D l_texture = AssetDatabase.LoadAssetAtPath<Texture2D>(l_path);

            if (null == l_texture)
                continue;

            string l_name = l_texture.name.Split('_')[0];

            if (!l_name.Contains("@"))
                continue;

            string l_animationAssetName = l_name.Split('@')[0];
            string l_animationName = l_name.Split('@')[1];
            string l_animationAssetPath = getAnimationAssetSavePath() + l_animationAssetName + ".asset";
            SpriteAnimationAsset l_animationAsset = AssetDatabase.LoadAssetAtPath<SpriteAnimationAsset>(l_animationAssetPath);

            if (l_animationAsset == null)
            {
                l_animationAsset = ScriptableObject.CreateInstance<SpriteAnimationAsset>();
                l_animationAsset.name = l_animationAssetName;
                l_animationAsset.animations = new List<SpriteAnimationData>();
                AssetDatabase.CreateAsset(l_animationAsset, AssetDatabase.GenerateUniqueAssetPath(l_animationAssetPath));
            }

            SpriteAnimationData l_animationData = l_animationAsset.animations.Find(a => a.name == l_animationName);

            if (l_animationData == null)
            {
                l_animationData = new SpriteAnimationData();
            }

            l_animationData.name = l_animationName;
            l_animationData.loop = l_animationName.Contains("loop") ? SpriteAnimationLoopMode.LOOPTOSTART : SpriteAnimationLoopMode.LOOPTOSTART;
            l_animationData.frameDatas = new List<SpriteAnimationFrameData>();

            Sprite[] l_sprites = AssetDatabase.LoadAllAssetsAtPath(l_path).OfType<Sprite>().ToArray();

            // check for json file
            string[] fileNameSplit = Path.GetFileNameWithoutExtension(l_path).Split('_');
            string fileName = fileNameSplit[0] + ".json";
            string jsonPath = Path.GetDirectoryName(l_path) + "/" + fileName;
            Debug.Log(jsonPath);
            TextAsset frameAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
            Gif gif = null;
            if (frameAsset)
                gif = JsonUtility.FromJson<Gif>(frameAsset.text);

            for (int j = 0; j < l_sprites.Length; j++)
            {
                Sprite l_sprite = l_sprites[j];
                SpriteAnimationFrameData l_frameData = new SpriteAnimationFrameData();
                l_frameData.time = gif != null && gif.frames.Count > j ?
                    gif.frames[j] / 1000f : DEFAULT_FRAME_TIME;
                l_frameData.sprite = l_sprite;
                l_animationData.frameDatas.Add(l_frameData);
            }

            if (!l_animationAsset.animations.Contains(l_animationData))
            {
                l_animationAsset.animations.Add(l_animationData);
            }

            EditorUtility.SetDirty(l_animationAsset);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(l_animationAsset), ImportAssetOptions.ForceUpdate);
        }

        AssetDatabase.SaveAssets();
    }
    [System.Serializable]
    public class Gif
    {
        public List<float> frames;
    }

    private static string getAnimationAssetSavePath()
    {
        return ANIMATION_SAVE_PATH;
    }
}

internal class EndSpriteAnimationAssetNameEdit : EndNameEditAction
{
    public override void Action(int InstanceID, string path, string file)
    {
        AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(InstanceID), AssetDatabase.GenerateUniqueAssetPath(path));
    }
}

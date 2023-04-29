using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;

namespace TakoboyStudios.Shared
{
    public class TMPGlyphEditor : OdinEditorWindow
    {
        [BoxGroup("Font Asset")]
        public TMP_FontAsset fontAsset;

        [Space]
        [ShowIf("fontAsset")]
        [BoxGroup("Bounding Box Offsets")]
        public float xOffset;
        [ShowIf("fontAsset")]
        [BoxGroup("Bounding Box Offsets")]
        public float yOffset;
        [ShowIf("fontAsset")]
        [BoxGroup("Bounding Box Offsets")]
        public float wOffset;
        [ShowIf("fontAsset")]
        [BoxGroup("Bounding Box Offsets")]
        public float hOffset;

        [ShowIf("fontAsset")]
        [BoxGroup("Character Offsets")]
        public int byOffset;
        [ShowIf("fontAsset")]
        [BoxGroup("Character Offsets")]
        public int adOffset;
        [ShowIf("fontAsset")]
        [BoxGroup("Character Offsets")]
        public int bxOffset;

        [ShowIf("fontAsset")]
        [BoxGroup("Setting Offsets")]

        [MenuItem("Window/TMP Glyph Editor")]
        static void Init()
        {
            var window = (TMPGlyphEditor)GetWindow(typeof(TMPGlyphEditor), false, "GlyphEditor");

            window.xOffset = -1;
            window.yOffset = -2;
            window.wOffset = 3;
            window.hOffset = 3;
            window.byOffset = 3;
            window.adOffset = 3;

            window.Show();
        }

        [ShowIf("fontAsset")]
        [Button ("Set Glyph Table")]
        public void SetGlyphTable()
        {
            for (var i = 0; i < fontAsset.glyphTable.Count; i++)
            {
                var rect = fontAsset.glyphTable[i].glyphRect;
                var metrics = fontAsset.glyphTable[i].metrics;

                fontAsset.glyphTable[i].glyphRect =
                    new GlyphRect(rect.x + (int)xOffset, rect.y + (int)yOffset, rect.width + (int)wOffset, rect.height + (int)hOffset);
                fontAsset.glyphTable[i].metrics = new GlyphMetrics(metrics.width + (int)wOffset, metrics.height + (int)hOffset,
                    metrics.horizontalBearingX +bxOffset, metrics.horizontalBearingY+ byOffset, metrics.horizontalAdvance+ adOffset);
            }

            AssetDatabase.SaveAssets();
        }

        [ShowIf("fontAsset")]
        [Button]
        public void SetAd()
        {
            for (var i = 0; i < fontAsset.glyphTable.Count; i++)
            {
                var metrics = fontAsset.glyphTable[i].metrics;

                fontAsset.glyphTable[i].metrics = new GlyphMetrics(metrics.width, metrics.height,
                    metrics.horizontalBearingX, metrics.horizontalBearingY, adOffset);
            }

            AssetDatabase.SaveAssets();
        }
    }
}
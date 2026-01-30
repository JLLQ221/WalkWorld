using UnityEditor;
using UnityEngine;

public class AnimationImg : MonoBehaviour
{
    //public Texture2D texture;

    //private void Start()
    //{
    //    AnimationClip clip = new AnimationClip();

    //    // load all of the sprites
    //    var sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));

    //    Debug.Log(sprites.Length);
    //    // TODO: link the sprites in the clip and set the times correctly
    //    // add the completed clip to the import result

    //    GenerateAnimation();
    //}

    //[ContextMenu("Generate Animation")]
    //private void GenerateAnimation()
    //{
    //    // Crear clip vacío
    //    AnimationClip clip = new AnimationClip();
    //    clip.frameRate = 12f; // velocidad de animación

    //    // Cargar todos los sprites del atlas/texture
    //    var sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
    //    var spriteFrames = new System.Collections.Generic.List<Sprite>();


    //    foreach (var s in sprites)
    //    {
    //        if (s is Sprite sprite)
    //            spriteFrames.Add(sprite);
    //    }

    //    // Crear keyframes
    //    ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[spriteFrames.Count];
    //    for (int i = 0; i < spriteFrames.Count; i++)
    //    {
    //        keyframes[i] = new ObjectReferenceKeyframe
    //        {
    //            time = i / clip.frameRate,
    //            value = spriteFrames[i]
    //        };
    //    }

    //    // Asignar curva al campo Image.sprite
    //    var binding = new EditorCurveBinding
    //    {
    //        type = typeof(UnityEngine.UI.Image),
    //        path = "", // objeto raíz
    //        propertyName = "m_Sprite"
    //    };

    //    AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

    //    // Guardar el clip como asset
    //    AssetDatabase.CreateAsset(clip, $"Assets/Animation/TalkAnimation/{spriteFrames[0].name.Remove(spriteFrames[0].name.Length - 1)}Anim.anim");
    //    AssetDatabase.SaveAssets();

    //    Debug.Log("Animation created with " + spriteFrames.Count + " frames.");
    //}
}

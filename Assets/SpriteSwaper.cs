using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwaper : MonoBehaviour
{
    public string mainSpriteName;
    public int skinNr;
 
    // This is where your spritesheets go
    // In the inspector, set the size to, for example 5, if you have 5 spritesheets
    // Then open each individual element and add the individual sprites from the spritesheets in here
    // This means if your spritesheet has 10 frames, the Sprites element in the inspector needs to contain these 10 sprites
    public Skins[] skins;
    SpriteRenderer spriteRenderer;
 
    // This spriteNr is helpful to easily add more accessories using the CustomizableAccessories.cs script
    public int spriteNr;
 
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    void LateUpdate()
    {
        SkinChoice();
    }
 
    void SkinChoice(){
        if (spriteRenderer.sprite.name.Contains(mainSpriteName)){
            string spriteName = spriteRenderer.sprite.name;
            spriteName = spriteName.Replace(mainSpriteName+"_", "");
            spriteNr = int.Parse(spriteName);
 
            spriteRenderer.sprite = skins[skinNr].sprites[spriteNr];
        }
    }
}
 
[System.Serializable]
public struct Skins{
    public Sprite[] sprites;
}

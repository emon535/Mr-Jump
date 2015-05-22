using UnityEngine;
using System;

[ExecuteInEditMode]
public class ReSkinAnimation : MonoBehaviour {

	public string spriteSheetName = "H1";
    public bool InEditMode = true;
    public string[] SpriteSheetNameToSortBack;
    public string[] BoneSpriteName;
    public int[] BoneSpriteSortingOrder;
    public int[] defaultBoneSortingOrder;

    void Awake()
    {
        spriteSheetName = PlayerPrefs.GetString("Hero", "H1");
    }


    void LateUpdate()
    {
        if (InEditMode)
        {
            LoadSprite();
        }
    }

    void Start()
    {
        LoadSprite();
    }

    public void LoadSprite()
    {
        if (!InEditMode)
        {
            spriteSheetName = PlayerPrefs.GetString("Hero", "H1");
        }

        var subSprites = Resources.LoadAll<Sprite>("Heros/" + spriteSheetName);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteName = renderer.sprite.name;
            var newSprite = Array.Find(subSprites, item => item.name == spriteName);

            if (newSprite)
            {
                renderer.sprite = newSprite;
                if (BoneSpriteName != null && SpriteSheetNameToSortBack != null)
                {
                    foreach (string sheetName in SpriteSheetNameToSortBack)
                    {
                        if (sheetName.Equals(spriteSheetName))
                        {
                            int i = 0;
                            foreach (string st in BoneSpriteName)
                            {
                                if (st.Equals(spriteName))
                                {
                                    renderer.sortingOrder = BoneSpriteSortingOrder[i];
                                }
                                i++;
                            }
                            break;
                        }
                        else
                        {
                            int i = 0;
                            foreach (string st in BoneSpriteName)
                            {
                                if (st.Equals(spriteName))
                                {
                                    renderer.sortingOrder = defaultBoneSortingOrder[i];
                                }
                                i++;
                            }
                        }
                    }
                }
            }
        }
    }
}

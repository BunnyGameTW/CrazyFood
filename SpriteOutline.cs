using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color color = Color.white;

    [Range(0, 25)]
    public int outlineSize = 1;
	bool isHover;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
       // spriteRenderer = GetComponentInChildren<SpriteRenderer>();
          //  GetComponent<SpriteRenderer>();
    }
    void OnEnable() {
        

      //  UpdateOutline(true);
    }

    void OnDisable() {
        //UpdateOutline(false);
    }

    void Update() {//not work
		 UpdateOutline(isHover);
		//else UpdateOutline(false);
    }
	void OnMouseOver(){
		//spriteRenderer = GetComponent<SpriteRenderer>();
		isHover=true;
		//UpdateOutline(true);

	}
	void OnMouseExit(){
		isHover = false;
		//UpdateOutline(false);


	}
    void UpdateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}

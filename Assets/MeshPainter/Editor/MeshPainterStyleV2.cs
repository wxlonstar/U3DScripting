using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshPainter))]
[CanEditMultipleObjects]
public class MeshPainterStyleV2 : Editor {
    string contolTexName = "";

    bool isPaint;
    float bSize = 1f;
    //float brushSize = 1f;
    float brushSize {
        get {
            return bSize;
        }
        set {
            if(value < 36f || value > 0.1f) {
                bSize = value;
            }
        }
    }
    float brushStronger = 0.5f;

    Texture[] brushTex;
    Texture[] texLayer;

    int selBrush = 0;
    int selTex = 0;

    float orthographicSize;

    int brushSizeInPourcent;
    Texture2D MaskTex;
    void OnSceneGUI() {
        if (isPaint) {
            brushSize = BrushSizeShortcut(brushSize);
            Painter();
        }

    }
    public override void OnInspectorGUI() {
        if (Check()) {
            GUIStyle boolBtnOn = new GUIStyle(GUI.skin.GetStyle("Button")); // get button style
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                isPaint = GUILayout.Toggle(isPaint, EditorGUIUtility.IconContent("EditCollider"), boolBtnOn, GUILayout.Width(35), GUILayout.Height(25));// turn on if in paint mode
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            //brushSize = (int)EditorGUILayout.Slider("Brush Size", brushSize, 1, 36);// brush size
            brushSize = (float)EditorGUILayout.Slider("Brush Size", brushSize, 0.1f, 36);
            brushStronger = EditorGUILayout.Slider("Brush Stronger", brushStronger, 0, 1f);// brush strength

            IniBrush();
            layerTex();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal("box", GUILayout.Width(340));
                    selTex = GUILayout.SelectionGrid(selTex, texLayer, 4, "gridlist", GUILayout.Width(340), GUILayout.Height(86));
                    GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal("box", GUILayout.Width(318));
                    selBrush = GUILayout.SelectionGrid(selBrush, brushTex, 9, "gridlist", GUILayout.Width(340), GUILayout.Height(70));
                    GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
    }

    // get textures in material
    void layerTex() {
        Transform Select = Selection.activeTransform;
        texLayer = new Texture[4];
        texLayer[0] = AssetPreview.GetAssetPreview(Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_80A09D49")) as Texture;
        texLayer[1] = AssetPreview.GetAssetPreview(Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_AAD0C7D8")) as Texture;
        texLayer[2] = AssetPreview.GetAssetPreview(Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_B5E77F27")) as Texture;
        texLayer[3] = AssetPreview.GetAssetPreview(Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_6948CFA8")) as Texture;
    }

    // initialize brushes
    void IniBrush() {
        string MeshPaintEditorFolder = "Assets/MeshPainter/Editor/";
        ArrayList BrushList = new ArrayList();
        Texture BrushesTL;
        int BrushNum = 0;
        do {
            BrushesTL = (Texture)AssetDatabase.LoadAssetAtPath(MeshPaintEditorFolder + "Brushes/Brush" + BrushNum + ".png", typeof(Texture));

            if (BrushesTL) {
                BrushList.Add(BrushesTL);
            }
            BrushNum++;
        } while (BrushesTL);
        brushTex = BrushList.ToArray(typeof(Texture)) as Texture[];
    }

    // check current status
    bool Check() {
        bool Check = false;
        Transform Select = Selection.activeTransform;
        Texture ControlTex = Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_FB2B5AB1");
        if(Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.shader == Shader.Find("MileShader/MyaStyle_v3") || Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.shader == Shader.Find("Shader Graphs/MyaStyle_v3")) {
            if(ControlTex == null)
            {
                EditorGUILayout.HelpBox("Can't find splat map, you may create one !", MessageType.Error);
                if (GUILayout.Button("Create a splat map")) {
                    creatContolTex();
                    //Select.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_Control", creatContolTex());
                }
            }
            else {
                Check = true;
            }
        }
        else {
            EditorGUILayout.HelpBox("Current shader is not right", MessageType.Error);
        }
        return Check;
    }

    //创建Contol贴图
    void creatContolTex() {

        //创建一个新的Contol贴图
        string ContolTexFolder = "Assets/MeshPainter/Controller/";
        //Texture2D newMaskTex = new Texture2D(512, 512, TextureFormat.ARGB32, true);
        Texture2D newMaskTex = new Texture2D(512, 512, TextureFormat.RGBA32, true);
        Color[] colorBase = new Color[512 * 512];
        for(int t = 0; t< colorBase.Length; t++) {
            colorBase[t] = new Color(1, 0, 0, 0);
        }
        newMaskTex.SetPixels(colorBase);

        //判断是否重名
        bool exporNameSuccess = true;
        for(int num = 1; exporNameSuccess; num++) {
            string Next = Selection.activeTransform.name +"_"+ num;
            if (!File.Exists(ContolTexFolder + Selection.activeTransform.name + ".tga")) {
                contolTexName = Selection.activeTransform.name;
                exporNameSuccess = false;
            }
            else if (!File.Exists(ContolTexFolder + Next + ".tga")) {
                contolTexName = Next;
                exporNameSuccess = false;
            }

        }

        string path = ContolTexFolder + contolTexName + ".tga";
        byte[] bytes = newMaskTex.EncodeToPNG();
        File.WriteAllBytes(path, bytes);//save


        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);//import asset and update
        //Contol贴图的导入设置
        TextureImporter textureIm = AssetImporter.GetAtPath(path) as TextureImporter;
        textureIm.textureCompression = TextureImporterCompression.Uncompressed;
        textureIm.isReadable = true;
        textureIm.anisoLevel = 9;
        textureIm.mipmapEnabled = false;
        textureIm.wrapMode = TextureWrapMode.Clamp;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);//update


        setContolTex(path);//set up splat map

    }

    void setContolTex(string path) {
        Texture2D ControlTex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        Selection.activeTransform.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("Texture2D_FB2B5AB1", ControlTex);
    }

    float BrushSizeShortcut(float currentSize) {
        Event e = Event.current;
        /*
        if(orthographicSize > 0.1f && orthographicSize < 36f) {
            //Debug.Log(orthographicSize); 
        }
        */
        if(this.brushSize >= 0.1f && this.brushSize <= 36f) {
            if(e.keyCode == KeyCode.LeftBracket) {
                
                currentSize = currentSize - 0.1f;
                EditorGUILayout.Slider("Brush Size", currentSize, 0.1f, 36);
            } else if(e.keyCode == KeyCode.RightBracket) {
                currentSize = currentSize + 0.1f;
                EditorGUILayout.Slider("Brush Size", currentSize, 0.1f, 36);
            }
        }

        
        return currentSize;
    }

    void Painter() {
        Transform CurrentSelect = Selection.activeTransform;
        MeshFilter temp = CurrentSelect.GetComponent<MeshFilter>();//get mesh filter of this model
        orthographicSize = (brushSize * CurrentSelect.localScale.x) * (temp.sharedMesh.bounds.size.x / 200);//brush size on model
        MaskTex = (Texture2D)CurrentSelect.gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("Texture2D_FB2B5AB1");//get splat map in material

        brushSizeInPourcent = (int)Mathf.Round((brushSize * MaskTex.width) / 100);//bursh size on model
        bool ToggleF = false;
        Event e = Event.current;// event for now
        HandleUtility.AddDefaultControl(0);
        RaycastHit raycastHit = new RaycastHit();
        Ray terrain = HandleUtility.GUIPointToWorldRay(e.mousePosition);// cast a ray
        if (Physics.Raycast(terrain, out raycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("MilePaint"))) {    //check if the layer is right 
            Handles.color = new Color(1f, 1f, 0f, 1f);//handle color
            Handles.DrawWireDisc(raycastHit.point, raycastHit.normal, orthographicSize);//draw a circle for brush postion

            //when to draw
            if ((e.type == EventType.MouseDrag && e.alt == false && e.control == false && e.shift == false && e.button == 0) || (e.type == EventType.MouseDown && e.shift == false && e.alt == false && e.control == false && e.button == 0 && ToggleF == false)) {
                //which chanel to draw
                Color targetColor = new Color(1f, 0f, 0f, 0f);
                switch (selTex) {
                    case 0:
                        targetColor = new Color(1f, 0f, 0f, 0f);
                        break;
                    case 1:
                        targetColor = new Color(0f, 1f, 0f, 0f);
                        break;
                    case 2:
                        targetColor = new Color(0f, 0f, 1f, 0f);
                        break;
                    case 3:
                        targetColor = new Color(0f, 0f, 0f, 1f);
                        break;

                }

                Vector2 pixelUV = raycastHit.textureCoord;

                //get area size coverd by brush
                int PuX = Mathf.FloorToInt(pixelUV.x * MaskTex.width);
                int PuY = Mathf.FloorToInt(pixelUV.y * MaskTex.height);
                int x = Mathf.Clamp(PuX - brushSizeInPourcent / 2, 0, MaskTex.width - 1);
                int y = Mathf.Clamp(PuY - brushSizeInPourcent / 2, 0, MaskTex.height - 1);
                int width = Mathf.Clamp((PuX + brushSizeInPourcent / 2), 0, MaskTex.width) - x;
                int height = Mathf.Clamp((PuY + brushSizeInPourcent / 2), 0, MaskTex.height) - y;

                Color[] terrainBay = MaskTex.GetPixels(x, y, width, height, 0);//get color in splat map coverd by brush

                Texture2D TBrush = brushTex[selBrush] as Texture2D;//brush shape textures
                float[] brushAlpha = new float[brushSizeInPourcent * brushSizeInPourcent];//brush opacity

                //brush opacity via brush shape
                for (int i = 0; i < brushSizeInPourcent; i++) {
                    for (int j = 0; j < brushSizeInPourcent; j++) {
                        brushAlpha[j * brushSizeInPourcent + i] = TBrush.GetPixelBilinear(((float)i) / brushSizeInPourcent, ((float)j) / brushSizeInPourcent).a;
                    }
                }

                //real color after draw
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++) {
                        int index = (i * width) + j;
                        float Stronger = brushAlpha[Mathf.Clamp((y + i) - (PuY - brushSizeInPourcent / 2), 0, brushSizeInPourcent - 1) * brushSizeInPourcent + Mathf.Clamp((x + j) - (PuX - brushSizeInPourcent / 2), 0, brushSizeInPourcent - 1)] * brushStronger;

                        terrainBay[index] = Color.Lerp(terrainBay[index], targetColor, Stronger);
                    }
                }
                Undo.RegisterCompleteObjectUndo(MaskTex, "meshPaint");//used for undo

                MaskTex.SetPixels(x, y, width, height, terrainBay, 0);//save texuture after paint
                MaskTex.Apply();
                ToggleF = true;
            }

            //else if (e.type == EventType.MouseUp && e.alt == false && e.button == 0 && ToggleF == true) {
            else if (e.type == EventType.MouseUp && e.alt == false && e.button == 0) {
                    SaveTexture();//save splat map
                ToggleF = false;
            }
        }
    }
    public void SaveTexture() {
        var path = AssetDatabase.GetAssetPath(MaskTex);
        //var bytes = MaskTex.EncodeToPNG();
        var bytes = MaskTex.EncodeToTGA();
        File.WriteAllBytes(path, bytes);
        //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);//force update
    }
}

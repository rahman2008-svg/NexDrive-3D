using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameBootstrap : MonoBehaviour
{
    public static GameBootstrap Instance;
    public CarController car;
    public Text hud;
    private float elapsed;
    private int collected;
    private readonly List<GameObject> pickups = new List<GameObject>();

    void Awake() { Instance = this; }

    void Start()
    {
        Application.targetFrameRate = 60;
        BuildWorld();
        BuildUI();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (hud != null && car != null)
            hud.text = "NEXDRIVE 3D  |  " + Mathf.RoundToInt(car.SpeedKmh) + " km/h\nCrystals: " + collected + "/" + 12 + "   |   Time: " + elapsed.ToString("0.0") + "s\nWASD / Arrows to drive  •  Space to brake";
    }

    void BuildWorld()
    {
        RenderSettings.ambientLight = new Color(.58f, .62f, .70f);
        RenderSettings.fog = true; RenderSettings.fogColor = new Color(.68f,.76f,.84f); RenderSettings.fogMode = FogMode.Exponential; RenderSettings.fogDensity = .003f;
        var sun = new GameObject("Sun"); var light = sun.AddComponent<Light>(); light.type = LightType.Directional; light.intensity = 1.2f; sun.transform.rotation = Quaternion.Euler(48,-32,0);
        Make("Ground", PrimitiveType.Plane, new Vector3(0,-.15f,0), new Vector3(18,1,18), new Color(.22f,.48f,.25f));
        Make("Main Road", PrimitiveType.Cube, new Vector3(0,.01f,0), new Vector3(8,.12f,180), new Color(.16f,.17f,.19f));
        for(int z=-80;z<=80;z+=8) Make("Road Marking",PrimitiveType.Cube,new Vector3(0,.09f,z),new Vector3(.16f,.02f,3.5f),new Color(.95f,.86f,.52f));
        for(int side=-1;side<=1;side+=2)
        {
            Make("Sidewalk",PrimitiveType.Cube,new Vector3(side*5,.04f,0),new Vector3(1.8f,.2f,180),new Color(.48f,.5f,.5f));
            for(int z=-75;z<=75;z+=15)
            {
                float h=Random.Range(5f,15f), w=Random.Range(4f,7f);
                Make("City Building",PrimitiveType.Cube,new Vector3(side*Random.Range(10f,15f),h/2f,z+Random.Range(-3f,3f)),new Vector3(w,h,Random.Range(5f,9f)),new Color(Random.Range(.35f,.7f),Random.Range(.38f,.68f),Random.Range(.42f,.72f)));
                var tree=Make("Street Tree",PrimitiveType.Cylinder,new Vector3(side*7f,1.2f,z+5),new Vector3(.35f,1.2f,.35f),new Color(.35f,.2f,.1f));
                Make("Tree Crown",PrimitiveType.Sphere,new Vector3(side*7f,3.1f,z+5),new Vector3(1.6f,1.8f,1.6f),new Color(.12f,.42f,.2f));
            }
        }
        var carObj = new GameObject("Player Car"); carObj.transform.position = new Vector3(0,.65f,-55);
        var body = Make("Car Body",PrimitiveType.Cube,carObj.transform.position+new Vector3(0,.3f,0),new Vector3(1.8f,.65f,3.3f),new Color(.04f,.38f,.95f)); body.transform.SetParent(carObj.transform,true);
        var cabin = Make("Car Cabin",PrimitiveType.Cube,carObj.transform.position+new Vector3(0,.82f,-.15f),new Vector3(1.25f,.55f,1.55f),new Color(.08f,.12f,.17f)); cabin.transform.SetParent(carObj.transform,true);
        foreach(int sx in new[]{-1,1}) foreach(int sz in new[]{-1,1}) { var wheel=Make("Wheel",PrimitiveType.Cylinder,carObj.transform.position+new Vector3(sx*.94f,.12f,sz*1.05f),new Vector3(.34f,.22f,.34f),Color.black); wheel.transform.rotation=Quaternion.Euler(0,0,90); wheel.transform.SetParent(carObj.transform,true); }
        var rb=carObj.AddComponent<Rigidbody>(); rb.mass=1100; rb.drag=.25f; rb.angularDrag=2; rb.constraints=RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;
        var collider=carObj.AddComponent<BoxCollider>(); collider.center=new Vector3(0,.25f,0); collider.size=new Vector3(1.8f,.65f,3.3f);
        car=carObj.AddComponent<CarController>();
        var camObj=new GameObject("Follow Camera"); var cam=camObj.AddComponent<Camera>(); cam.fieldOfView=70; camObj.tag="MainCamera"; camObj.AddComponent<AudioListener>(); camObj.AddComponent<FollowCamera>().target=carObj.transform;
        for(int i=0;i<12;i++) { int z=-42+i*10; var p=Make("Crystal",PrimitiveType.Sphere,new Vector3((i%2==0?-2.2f:2.2f),1.1f,z),new Vector3(.65f,.65f,.65f),new Color(0.1f,1f,.9f)); p.GetComponent<Collider>().isTrigger=true; var rbp=p.AddComponent<Rigidbody>(); rbp.isKinematic=true; rbp.useGravity=false; p.AddComponent<CrystalPickup>(); pickups.Add(p); }
    }

    GameObject Make(string n, PrimitiveType type, Vector3 pos, Vector3 scale, Color color)
    {
        var g=GameObject.CreatePrimitive(type); g.name=n; g.transform.position=pos; g.transform.localScale=scale;
        var r=g.GetComponent<Renderer>(); if(r!=null){ var m=new Material(Shader.Find("Standard")); m.color=color; r.material=m; }
        return g;
    }

    void BuildUI()
    {
        var canvasObj=new GameObject("HUD Canvas"); var canvas=canvasObj.AddComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay; canvasObj.AddComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; canvasObj.AddComponent<GraphicRaycaster>();
        var textObj=new GameObject("HUD Text"); textObj.transform.SetParent(canvasObj.transform,false); hud=textObj.AddComponent<Text>(); hud.font=Resources.GetBuiltinResource<Font>("Arial.ttf"); hud.fontSize=22; hud.color=Color.white; hud.alignment=TextAnchor.UpperLeft; hud.horizontalOverflow=HorizontalWrapMode.Wrap; hud.rectTransform.anchorMin=new Vector2(0,1); hud.rectTransform.anchorMax=new Vector2(0,1); hud.rectTransform.pivot=new Vector2(0,1); hud.rectTransform.anchoredPosition=new Vector2(22,-20); hud.rectTransform.sizeDelta=new Vector2(650,100);
        AddTouchButton(canvasObj.transform,"◀",new Vector2(70,70),new Vector2(65,65),"left"); AddTouchButton(canvasObj.transform,"▶",new Vector2(215,70),new Vector2(65,65),"right"); AddTouchButton(canvasObj.transform,"▲",new Vector2(-115,145),new Vector2(70,70),"accelerate"); AddTouchButton(canvasObj.transform,"▼",new Vector2(-115,65),new Vector2(70,70),"brake");
    }
    void AddTouchButton(Transform parent,string label,Vector2 pos,Vector2 size,string key)
    {
        var o=new GameObject("Touch "+key); o.transform.SetParent(parent,false); var b=o.AddComponent<Button>(); var img=o.AddComponent<Image>(); img.color=new Color(.05f,.08f,.12f,.55f); b.targetGraphic=img; var rt=o.GetComponent<RectTransform>(); rt.anchorMin=rt.anchorMax=new Vector2(pos.x<0?1:0,0); rt.pivot=new Vector2(.5f,.5f); rt.anchoredPosition=pos; rt.sizeDelta=size;
        var t=new GameObject("Label"); t.transform.SetParent(o.transform,false); var tx=t.AddComponent<Text>(); tx.font=Resources.GetBuiltinResource<Font>("Arial.ttf"); tx.text=label; tx.fontSize=30; tx.alignment=TextAnchor.MiddleCenter; tx.color=Color.white; tx.rectTransform.anchorMin=Vector2.zero; tx.rectTransform.anchorMax=Vector2.one; tx.rectTransform.offsetMin=tx.rectTransform.offsetMax=Vector2.zero;
        var hold=o.AddComponent<TouchHold>(); hold.key=key;
    }
    public void Collect(GameObject item){collected++; Destroy(item);}
}

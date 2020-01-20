# VR-Lab
## Woordje uitleg
Voor de stadsdichter van Antwerpen moesten we een vr project maken. Wij hebben gekozen voor het gedicht : Het huis in mij. We hebben een vr project gerealiseerd met 360° video.
Het is geschreven voor de ocolus quest. 

## Opstarten van een project voor de oculus Quest.
Een handig youtube filmpje voor het opstarten van een unity project voor de quest : https://www.youtube.com/watch?v=qiJpjnzW-mw&t=223s

Wat we nodig hebben voor we beginnen is een unity versie met de android build support ook geïnstalleerd. (Dit kan toegevoegd worden op al eerder geïnstalleerde unity versies.)
![toevoegen Android](/afbeeldingen/android.png)

1. We beginnen met het aanmaken van een nieuw 3D unity project.
![Create](/afbeeldingen/create.png)

2. We gaan beginnen met in de asset store de oculus integration plugin te downloaden.
![oculusPlugin](/afbeeldingen/oculusplugin.png)

2. Bij de build settings moeten we nu het platform wijzigen naar Android. (Als er nog geen android module geladen is moet je deze installeren met de open download page knop.)
![build Settings](/afbeeldingen/buildSetting.png)

3. Als we veranderd zijn van platform moeten we nu naar de player settings gaan en hier een paar instellingen veranderen. 

4. Wat we eerst bij de player settings gaan veranderen is bij de XR settings Virtual Reality toelaten en de oculus SDK toevoegen.
![xr settings](/afbeeldingen/xrsettings.png)

5. Hierna gaan we bij de plater settings naar de tab Other Settings en hier moeten we twee dingen veranderen. Voor te beginnen moet de Vulkan grafics API verwijderd worden en moet de API veranderen naar een hogere API. Dit moet minimum de 4.4 API zijn 
![API](/afbeeldingen/API.png)
![graphics API](/afbleedingen/grapicsAPI.png)

6. Nu is het unity project klaar en moeten we nog een paar dingen doen voordat we het unity project kunnen pushen naar de quest. In de oculus app waarmee je de quest hebt verbonden op more settings drukken developer settings en de developper mode enablen. Als we dit gedaan hebben gaan we de de bril verbinden met de computer. Dit doen we met de usb C kabel dat meegeleverd is. Als we dit gedaan hebben krijgen we in de bril een popup of we USB debugging toestaan van deze computer. Vink always allow aan en accepteer de popup.

Nu zijn we helemaal klaar voor een app te schrijven voor de Oculus Quest.

## uitleg code

Waarmee ik ga beginnen is de animatie van de container uit te leggen. 
De file waarmee wij gewerkt hebben is een .fbx file gemaakt met cinema 4D. Hierin zit de animatie waar we een animatie controller gaan voor moeten maken. Hierna kunnen we de animatie in de code laten starten en stoppen wanneer we willen.

### animatie container

1. We gaan beginnen met een Animatie controller aan te maken.
![aanmaken animator](/afbeeldingen/aanmaken_animator.png) 

2. De animatie controller laat toe om states te maken in een animatie. Aan elke state kunnen we een bool hangen dat de state van de animatie gaat triggeren. Wat je altijd moet doen is een default state maken. Hierin zal de animatie altijd opstarten. Voor de containet hebben we dit de waiting state genoemd. We hebben hiernaast ook een state waarbij de container opengaat. Zoals je kan zien hebben we ook twee parameters nodig de start en de stop paramater. Deze hebben we gelinkt aan de transities (pijlen tussen de twee states). Dus wat er nu gaat gebeuren is hetvolgende: Als we in de waiting state zijn en we maken start true dan zal er een transitie gebeuren naar de open container state en zal de containter opengaan. Als we nu wachten tot de container open is en we zetten de stop paramater op true zal de container terug naar de waiting state gaan en dus terug naar het begin van de animatie gaan. Nu we tussen de states kunnen gaan moeten we nog aan de open container state de effectieve animatie toevoegen. Die staat in het fbx file en sleep je in het kadertje van motion
![animator](/afbeeldingen/animator.png)
![open container state](/afbeeldingen/state.png)

3. Als we de animatie controller hebben geconfigureerd gaan we het fbx file in de scene slepen en gaan we zien dat er nog een controller toegevoegd moet worden. Dit doen we door de net aangemaakte controller te slepen in de animator.
![animator container](/afbeeldingen/animator_container.png)

4. Nu is het klaar om de controller te gaan besturen in het script. We beginnen met het declaren van de animator.
```c#
    public Animator anim;
```

5. Wat we de Update functie gaan doen is de bools setten. Wanneer we willen beginnen in met de functie gaan we de start bool op true zetten en de stop op false. We kunnen met een simple if het einde van de van de open container state bepalen en hierin de start op false zetten en de stop op true. We moeten nu de animatie terug klaar zetten zodat als we opnieuw beginnen de animatie terug gaat afspelen.
```c#
    // starten
    anim.SetBool("start", true);
    anim.SetBool("stop", false);
    //einde
    if( anim.GetCurrentAnimatorStateInfo(0).IsName("open container state") && 
        anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
    {
        anim.SetBool("start", false);
        anim.SetBool("stop", true);
        anim.Play("Waiting State",-1,0f);
    }
```

### 360 video aan de binnekant van een sphere

De uitdaging was om aan de binnekant van de sphere een video te tonen. Dit hebben we gedaan door met een script een game object te maken dat aan de binnekant van de sphere de mogelijkheid heeft om een video af te spelen. Voor dit script te laten werken voor de oculus moeten we het in een nieuw mapje zetten genaamd Editor anders gaat het niet werken.

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InvertedSphere : EditorWindow
{
    private string st = "1.0";

    [MenuItem("GameObject/Create Other/Inverted Sphere...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(InvertedSphere));
    }

    public void OnGUI()
    {
        GUILayout.Label("Enter sphere size:");
        st = GUILayout.TextField(st);

        float f;
        if (!float.TryParse(st, out f))
            f = 1.0f;
        if (GUILayout.Button("Create Inverted Sphere"))
        {
            CreateInvertedSphere(f);
        }
    }

    private void CreateInvertedSphere(float size)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshFilter mf = go.GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;

        GameObject goNew = new GameObject();
        goNew.name = "Inverted Sphere";
        MeshFilter mfNew = goNew.AddComponent<MeshFilter>();
        mfNew.sharedMesh = new Mesh();


        //Scale the vertices;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = vertices[i] * size;
        mfNew.sharedMesh.vertices = vertices;

        // Reverse the triangles
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }
        mfNew.sharedMesh.triangles = triangles;

        // Reverse the normals;
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];
        mfNew.sharedMesh.normals = normals;


        mfNew.sharedMesh.uv = mesh.uv;
        mfNew.sharedMesh.uv2 = mesh.uv2;
        mfNew.sharedMesh.RecalculateBounds();

        // Add the same material that the original sphere used
        MeshRenderer mr = goNew.AddComponent<MeshRenderer>();
        mr.sharedMaterial = go.GetComponent<Renderer>().sharedMaterial;

        DestroyImmediate(go);
    }
}
```

Nu we dit hebben aangemaakt kunnen we de sphere gebruiken. Dit doen we door in de scene een inverted sphere toe te voegen.
![inverted sphere](/afbeeldingen/sphere.png)

Nu we de sphere hebben kunnen we de video die we willen afspelen er gewoon opslepen.
Deze sphere kunnen we ook via code besturen. Dit doen we zoals een normale videoplayer.

We declareren eerst de twee sphere als gameobject. Hier gaan we laten de videoplayer uithalen

```c#
    public GameObject VideoSphere;
    public GameObject BlurSphere;

    VideoPlayer Video;
    VideoPlayer Blur;
```

In de start functie die 1 keer wordt uitgevoerd als het programma opstart gaan we de videoplayer uit de gameobjecten halen en de video's voorbereiden.
Op een videospeler kan je een functie toevoegen die uitgevoerd wordt als de video ten einde is. Hier heb ik gebruik van gemaakt om een bool te togglen zodat ik weet wanneer de video gedaan is.
```c#
    Video = VideoSphere.GetComponent<VideoPlayer>();
    Blur = BlurSphere.GetComponent<VideoPlayer>();

    Video.Prepare();
    Blur.Prepare();

    Video.loopPointReached += eindeVideo;
    Blur.loopPointReached += eindeBlur;
```
De functie die dan wordt uitgevoerd ziet er als volgt uit:
```c#
    void eindeVideo(UnityEngine.Video.VideoPlayer vp){
        //code die moet worden uitgevoerd...
    }
```

Wat we met de videoplayer verder kunnen doen is deze starten, pauzeren en stoppen. Dit doen we op deze manier: 
```c#
    Blur.Pause();
    Video.Play();
```

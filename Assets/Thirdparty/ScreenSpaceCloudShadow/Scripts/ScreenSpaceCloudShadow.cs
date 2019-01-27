using UnityEngine;
using System.Collections;
using System.IO;
[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class ScreenSpaceCloudShadow : MonoBehaviour
{
	[Header("Direction")]
	public bool isTopDown = true;

	[Header("Cloud")]
 	float time=0;
	static float timeInterval=1f/30f;
	float timeUntilNextFrame=1;
	
	public int textureIndex=0;
	public Texture cloudTexture = null;
	public Texture[] textures= null;
	
	public Vector2 cloudTiling = new Vector2(2, 2);
	public Vector2 cloudWindSpeed = new Vector2(0.05f, 0.05f);

	[Header("Shadow")]
	public Color shadowColor = new Color(0f,0.04f,0.23f,1f);
	public float shadowIntensity = 0.3f;

    private Renderer _ren = null;
	//private Shader _shader = null;

    void Awake(){
        _ren = GetComponent<Renderer>();
		//_shader = _ren && _ren.sharedMaterial ? _ren.sharedMaterial.shader : null;	
    }

    void Start(){
        OnWillRenderObject();
    }

	void OnValidate(){
		Refresh();
	}

    void OnEnable(){
		Refresh();

        if (Camera.main) Camera.main.depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnDisable(){
        if (Camera.main) Camera.main.depthTextureMode = DepthTextureMode.None;
    }

	void OnWillRenderObject(){
		
		time=Time.time;
		
		if(time>timeUntilNextFrame){
			textureIndex+=1;
			
			timeUntilNextFrame+=timeInterval;
		}
				
		
		if(textureIndex>textures.Length-1){
			textureIndex=0;
		}

		cloudTexture=textures[textureIndex];

		_ren.sharedMaterial.SetTexture ("_MainTex", cloudTexture);
		
		// Adjust Quad position to Farplane
		Camera cam = Camera.main;
		float dist = cam.farClipPlane - 0.1f; // 0.1 is some magic value to avoid culling
		Vector3 campos = cam.transform.position;
		Vector3 camray = cam.transform.forward * dist;
		Vector3 quadpos = campos + camray;
		transform.position = quadpos;

        // Adjust Quad size to Farplane
        Vector3 scale = transform.parent ? transform.parent.localScale : Vector3.one;
        float h = cam.orthographic ? cam.orthographicSize * 2f : Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * dist * 2f;
        transform.localScale = new Vector3(h * cam.aspect / scale.x, h / scale.y, 0f);

        // Render cloud shadow at GameView only
        bool isGameView = Camera.current == null || Camera.current == Camera.main;
		if(isGameView){
			transform.rotation = Quaternion.LookRotation(quadpos - campos, cam.transform.up); // align uv with _depthtexture
			
			float t = Time.time;
			_ren.sharedMaterial.SetVector("_CloudFactor", new Vector4(
				cloudWindSpeed.x * t
				, cloudWindSpeed.y * t
				, cloudTiling.x
				, cloudTiling.y));

			if(cam.orthographic){
				_ren.sharedMaterial.EnableKeyword("SSCS_ORTHO");
				_ren.sharedMaterial.SetVector("_WorldSpaceCameraRay", camray);
			}
			else{
				_ren.sharedMaterial.DisableKeyword("SSCS_ORTHO");
			}

		}
        else{			
            transform.LookAt(campos);
		}		
    }

	public void Refresh(){
		if (_ren){
			_ren.sharedMaterial.SetTexture ("_MainTex", cloudTexture);

			_ren.sharedMaterial.SetVector("_ShadowFactor", new Vector4(
				(1f - shadowColor.r) * shadowIntensity
				,(1f - shadowColor.g) * shadowIntensity
				,(1f - shadowColor.b) * shadowIntensity
				,1f));

			if(isTopDown){
				_ren.sharedMaterial.EnableKeyword("SSCS_TOPDOWN");	
			}
			else{
				_ren.sharedMaterial.DisableKeyword("SSCS_TOPDOWN");	
			}
		}
	}
}

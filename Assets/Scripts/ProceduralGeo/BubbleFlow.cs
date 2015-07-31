using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BubbleFlow : MonoBehaviour {

	private GameObject _dataObject;
	private Dictionary<string, int> trendingTopics = new Dictionary<string, int>();
    public Font bubbleFont;
	public List<GameObject> bubbles = new List<GameObject>();
    public List<GameObject> bubbleTexts = new List<GameObject>();
	public List<Material> bubbleColors = new List<Material>();
	public Sector sector = Sector.Automotive;

	public float bubbleOffset = 0.1f;
	public float zOffset = 0.1f;
	public float backdropOffset = 0.25f;
	public float animationSpeed = 5.0f;

	private int baseRadial = 0;
	private Vector2 intersection0 = Vector2.zero;
	private Vector2 intersection1 = Vector2.zero;

	private Vector3 centroid = Vector3.zero;
	private float centroidRadius = 0.0f;

	private GameObject _backdrop;

    void OnEnable() {        
		SelectionReaction.OnSelect += ModeSwitch;
    }

    void OnDisable() {
		SelectionReaction.OnSelect -= ModeSwitch;
    }

	void ModeSwitch(Sector sect) {
		if (bubbles.Count == 0) {
			BubbleFlowInit(sect);
		} else {
			DestroyAllChildren();
		}
	}

    void Init() {
        int tt = GetTrendingTopicsDictionary();
        if (tt == 1) {
            CreateBubbles(trendingTopics);
            CalculateCentroid();
            CreateBackdrop();
            Recenter();
            SetPathAnimation();
            CenterObjects();
        }
    }

    void BubbleFlowInit(Sector sect) {
        sector = sect;
        Invoke("Init", 1.0f);        
    }

	// Use this for initialization
	void Start () {
		_dataObject = GameObject.Find("DataObject");
	}

	private int GetTrendingTopicsDictionary(){
		if (_dataObject.GetComponent<TechTrendsWrapper>().isLoaded == true) {
			trendingTopics = _dataObject.GetComponent<TechTrendsWrapper>().GetSectorToDictionary(0, sector); //that zero eventually needs to correspond to weeks past
			return 1;
		}
		return 0;
	}

	public int GetVertCount(int val){
		if (val > 32) {
			return 64;
		}

		if (val > 16) {
			return 56;
		}
		if (val > 8) {
			return 48;
		}

		if (val > 4) {
			return 40;
		}

		return 32;
	}

	public void CreateBubbles(Dictionary<string, int> inputDictionary){
		int iterator = 0;
        GameObject Canvas = GameObject.Find("Canvas");
         
		foreach(KeyValuePair<string, int> entry in inputDictionary) {
			GameObject go  = new GameObject();
			go.name = entry.Key;
			go.transform.SetParent(this.transform);
			go.transform.localEulerAngles = transform.parent.localEulerAngles;
			bubbles.Add(go);

			float area = (float)entry.Value / 50.0f;
			area = Mathf.Clamp(area, 0.005f, 1.0f);

			ProceduralCircle pc = go.AddComponent<ProceduralCircle>();
			pc.Area = area;
			pc.circleVerts = GetVertCount(entry.Value);
			pc.material = bubbleColors[iterator];

			iterator++;
			if(iterator > 8) {iterator = 8;}; //clamp the iterator;

			go.AddComponent<HighlightReaction>();
			SphereCollider sc = go.AddComponent<SphereCollider>();
            sc.radius = 1.0f;
			go.layer = 8;
            go.AddComponent<Highlight>();

            //and now to create the respective text objects 
            GameObject bubbleText = new GameObject();
            //text object
            Text txt = bubbleText.AddComponent<Text>();
            txt.text = entry.Key + " : " + entry.Value;
            txt.font = bubbleFont;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontSize = 8;
            txt.color = new Color((206f / 255f), (206f / 255f), (206f / 255f));
            
            RectTransform rt = bubbleText.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(360f, 60f);
            bubbleText.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            bubbleText.name = entry.Key + "Text";
            bubbleText.transform.SetParent(Canvas.transform);
           
            PinTextToGameObject pinTex = bubbleText.AddComponent<PinTextToGameObject>();
            pinTex.pinnedObject = go;
            if(iterator == 0) { 
                pinTex.offset = new Vector3(0,0, -0.275f);
            } else {
                pinTex.offset = new Vector3(0, 0, -0.25f);
            }
            bubbleText.AddComponent<OrientTowards>();
            TextTruncator tt = bubbleText.AddComponent<TextTruncator>();
            tt.fullName = entry.Key;
            tt.trendCount = entry.Value;

            bubbleTexts.Add(bubbleText);            
		}
		SetBubblePositions();
	}

	private void SetBubblePositions(){
		for (int i = 0; i < bubbles.Count; i++) {
			//First we check to make sure all of our bubbles have a Procedural Circle attached, else error
			if(bubbles[i].GetComponent<ProceduralCircle>() == null){
				Debug.LogError("No Prodecedural Circle found on object.");
			}
		}

		for( int i = 0; i < bubbles.Count; i++){
			if (i == 0){ //position first bubble
				bubbles[i].transform.localPosition = Vector3.zero;

			} else if(i == 1){ //position second bubble
				float nextX = GetRadialOffset(bubbles[0], bubbles[1]);
				bubbles[i].transform.localPosition = new Vector3( nextX, 0f, (zOffset * (float)i));

			} else { //here we get into the meat of positioning the rest of the circles in the list
				//STUB HERE
				float radius0 = GetRadialOffset(bubbles[baseRadial], bubbles[i]);
				float radius1 = GetRadialOffset(bubbles[i-1], bubbles[i]);

				int intersectionCounts = GetCircCircIntersect(bubbles[baseRadial], radius0, bubbles[i-1], radius1);

				//We've have our first intersections, now to parse what we have
				if(intersectionCounts == 0) { 
					Debug.LogError("No valid intersections found. Can't places spheres.");
				} else if(intersectionCounts == 1) { //set the position to the lone intersection
					//this case shouldn't happen... in theory
					Debug.LogError("Only one intersection found, this is very bad.");
				} else {
					//position three is still a special case
					if(i==2){
						Vector2 nextPosition = Vector2.zero;
						if(intersection0.y > intersection1.y) {
							nextPosition = intersection0;
						} else {
							nextPosition = intersection1;
						}

						bubbles[i].transform.localPosition = new Vector3(nextPosition.x, nextPosition.y,
						                                                 (zOffset * (float)i));
					
					//drilling down in the most general case
					} else { 
						Vector2 checkPoint0 = new Vector2(bubbles[i-2].transform.localPosition.x, bubbles[i-2].transform.localPosition.y);
						Vector2 checkPoint1 = new Vector2(bubbles[baseRadial+1].transform.localPosition.x, bubbles[baseRadial+1].transform.localPosition.y);

						int intersect0Collision = GetCircCollision(intersection0, bubbles[i].GetComponent<ProceduralCircle>().Radius,
						                                           checkPoint0, bubbles[i-2].GetComponent<ProceduralCircle>().Radius);

						intersect0Collision += GetCircCollision(intersection0, bubbles[i].GetComponent<ProceduralCircle>().Radius,
						                                        checkPoint1, bubbles[baseRadial+1].GetComponent<ProceduralCircle>().Radius);

						if(intersect0Collision == 0) {
							bubbles[i].transform.localPosition = new Vector3(intersection0.x, intersection0.y, (zOffset * (float)i));

						}

						int intersect1Collision = GetCircCollision(intersection1, bubbles[i].GetComponent<ProceduralCircle>().Radius,
						                                           checkPoint0, bubbles[i-2].GetComponent<ProceduralCircle>().Radius);

						intersect1Collision += GetCircCollision(intersection1, bubbles[i].GetComponent<ProceduralCircle>().Radius,
						                                           checkPoint1, bubbles[baseRadial+1].GetComponent<ProceduralCircle>().Radius);

						if(intersect1Collision == 0) {
							bubbles[i].transform.localPosition = new Vector3(intersection1.x, intersection1.y, (zOffset * (float)i));
							// should be set
						} else {//Second verse eerily similar to the first...
							baseRadial++;
							radius0 = GetRadialOffset(bubbles[baseRadial], bubbles[i]);
							intersectionCounts = GetCircCircIntersect(bubbles[baseRadial], radius0, bubbles[i-1], radius1);

							if(intersectionCounts == 0) { 
								Debug.LogError("No valid intersections found. Can't places spheres.");
							} else if(intersectionCounts == 1) { //set the position to the lone intersection
								//this case shouldn't happen... in theory
								Debug.LogError("Only one intersection found, this is very bad.");
							} else {
								//reset some of these values for use again
								intersect0Collision = 0;
								intersect1Collision = 0;
								checkPoint1 = new Vector2(bubbles[baseRadial+1].transform.localPosition.x, bubbles[baseRadial+1].transform.localPosition.y);

								intersect0Collision = GetCircCollision(intersection0, bubbles[i].GetComponent<ProceduralCircle>().Radius,
								                                           checkPoint0, bubbles[i-2].GetComponent<ProceduralCircle>().Radius);
								
								intersect0Collision += GetCircCollision(intersection0, bubbles[i].GetComponent<ProceduralCircle>().Radius,
								                                        checkPoint1, bubbles[baseRadial+1].GetComponent<ProceduralCircle>().Radius);
								
								if(intersect0Collision == 0) {
									bubbles[i].transform.localPosition = new Vector3(intersection0.x, intersection0.y, (zOffset * (float)i));
								}

								intersect1Collision = GetCircCollision(intersection1, bubbles[i].GetComponent<ProceduralCircle>().Radius,
								                                           checkPoint0, bubbles[i-2].GetComponent<ProceduralCircle>().Radius);
								
								intersect1Collision += GetCircCollision(intersection1, bubbles[i].GetComponent<ProceduralCircle>().Radius,
								                                        checkPoint1, bubbles[baseRadial+1].GetComponent<ProceduralCircle>().Radius);
								
								if(intersect1Collision == 0) {
									bubbles[i].transform.localPosition = new Vector3(intersection1.x, intersection1.y, (zOffset * (float)i));
								} else {
									//if this doesn't catch you have a really big problem
									Debug.LogError("Couldn't find another acceptable position for this sphere.");
								}
							}
						}
					}
				}
			}
		}
	}

	//overloading for clarity
	private float GetRadialOffset (GameObject obj0, GameObject obj1){
		return GetRadialOffset(obj0.GetComponent<ProceduralCircle>().Radius,
		                obj1.GetComponent<ProceduralCircle>().Radius,
		                bubbleOffset);		
	}

	private float GetRadialOffset (float firstRadius, float secondRadius, float offset){
		float radii = firstRadius + secondRadius + offset;
		return radii;
	}

	//overloading for clarity
	private int GetCircCircIntersect(GameObject obj0, float radius0, GameObject obj1, float radius1){
		return GetCircCircIntersect (obj0.transform.localPosition.x,
		                             obj0.transform.localPosition.y,
		                            radius0,
		                            obj1.transform.localPosition.x,
		                            obj1.transform.localPosition.y,
		                            radius1,
		                            out intersection0,
		                            out intersection1);
	}

	private int GetCircCircIntersect(
		float cx0, float cy0, float radius0,
		float cx1, float cy1, float radius1,
		out Vector2 intersection0, out Vector2 intersection1)
	{
		// Find the distance between the centers.
		float dx = cx0 - cx1;
		float dy = cy0 - cy1;
		float dist = Mathf.Sqrt(dx * dx + dy * dy);
		
		// See how many solutions there are.
		if (dist > radius0 + radius1)
		{
			// No solutions, the circles are too far apart.
			intersection0 = new Vector2(float.NaN, float.NaN);
			intersection1 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		else if (dist < Mathf.Abs(radius0 - radius1))
		{
			// No solutions, one circle contains the other.
			intersection0 = new Vector2(float.NaN, float.NaN);
			intersection1 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		else if ((dist == 0) && (radius0 == radius1))
		{
			// No solutions, the circles coincide.
			intersection0 = new Vector2(float.NaN, float.NaN);
			intersection1 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		else
		{
			// Find a and h.
			float a = (radius0 * radius0 -
			            radius1 * radius1 + dist * dist) / (2 * dist);
			float h = Mathf.Sqrt(radius0 * radius0 - a * a);
			
			// Find P2.
			float cx2 = cx0 + a * (cx1 - cx0) / dist;
			float cy2 = cy0 + a * (cy1 - cy0) / dist;
			
			// Get the points P3.
			intersection0 = new Vector2(
				(cx2 + h * (cy1 - cy0) / dist),
				(cy2 - h * (cx1 - cx0) / dist));
			intersection1 = new Vector2(
				(cx2 - h * (cy1 - cy0) / dist),
				(cy2 + h * (cx1 - cx0) / dist));
			
			// See if we have 1 or 2 solutions.
			if (dist == radius0 + radius1) return 1;
			return 2;
		}
	}

	private int GetCircCollision(Vector2 pos0, float radius0, Vector2 pos1, float radius1){
		return GetCircCollision(pos0.x, pos0.y, radius0,
							   pos1.x, pos1.y, radius1);
	}

	private int GetCircCollision(float cx0, float cy0, float radius0, float cx1, float cy1, float radius1){
		float radii = radius0 + radius1;
		float dist = Mathf.Sqrt(((cx0 - cx1) * (cx0 - cx1)) + ((cy0 - cy1) * (cy0 - cy1)));
		if (dist < radii) {
			return 1;
		}
		return 0;
	}

	private void CalculateCentroid(){
		float maxX = 0f;
		float minX = 0f;
		float maxY = 0f;
		float minY = 0f;

		foreach (GameObject bub in bubbles) {		
			float _localRad = bub.GetComponent<ProceduralCircle>().Radius;
			if((bub.transform.localPosition.x + _localRad) > maxX){
				maxX = bub.transform.localPosition.x + _localRad;
			}

			if((bub.transform.localPosition.x - _localRad) < minX){
				minX = bub.transform.localPosition.x - _localRad;
			}

			if((bub.transform.localPosition.y + _localRad) > maxY){
				maxY = bub.transform.localPosition.y + _localRad;
			}

			if((bub.transform.localPosition.y - _localRad) < minY){
				minY = bub.transform.localPosition.y - _localRad;
			}
		}

		centroid = new Vector3 (maxX + minX, maxY + minY, 0f);

		maxX += Mathf.Abs(minX);
		maxY += Mathf.Abs(minY);
		centroidRadius = (maxX > maxY) ? maxX : maxY;
		//centroidRadius /= 1.8f;
	}

	private void CreateBackdrop(){
		_backdrop = new GameObject();
		_backdrop.name = "Backdrop";
		_backdrop.transform.SetParent (this.transform);
        _backdrop.transform.localEulerAngles = Vector3.zero;
		_backdrop.transform.localPosition = new Vector3 (centroid.x * 0.5f, centroid.y * 0.5f, backdropOffset);
		_backdrop.transform.localScale = new Vector3 (centroidRadius * 2.05f, centroidRadius * 2.05f, centroidRadius * 2.05f);
		
		ProceduralCircle pc = _backdrop.AddComponent<ProceduralCircle>();
		pc.Area = _backdrop.transform.localScale.x;
		pc.circleVerts = 96;
		pc.material = bubbleColors[10];
	}

	public void Recenter(){
		float newScale =  1.0f / (centroidRadius / 1.8f);
		this.transform.localScale = new Vector3 (newScale, newScale, newScale);
		this.transform.localPosition = centroid * -0.5f;
	}

	private void SetPathAnimation(){
		List<Vector3> path = new List<Vector3>();
		for (int i = 0; i < bubbles.Count; i++) {
			//create a Vector3 of paths for each bubble
			path.Add(bubbles[i].transform.localPosition);
			//create a PathAnimation object for each bubble
			PathAnimation pa = bubbles[i].AddComponent<PathAnimation>();
			pa.speed = animationSpeed;
			pa.SetControlPoints(path);
		}
	}

	private void CenterObjects(){
		foreach (GameObject bub in bubbles) {
			Vector3 zeroish = new Vector3(0,0, bub.transform.localPosition.z);
			bub.transform.localPosition = zeroish;
		}
	}

	// Update is called once per frame
	void Update () {
        /*
		if (Input.GetKeyUp (KeyCode.Space)) {
			if(bubbles.Count > 0){
				DestroyAllChildren();
			} else {
				int tt = GetTrendingTopicsDictionary();

				if(tt == 1){
					CreateBubbles(trendingTopics);			
					CalculateCentroid();
					CreateBackdrop();
					Recenter();
					SetPathAnimation();
					CenterObjects();
				}
			}
		}*/
	}

	void DestroyAllChildren(){
		foreach(GameObject bub in bubbles){
				Destroy(bub);
		}
		bubbles.Clear();
        
        foreach(GameObject bub in bubbleTexts) {
            Destroy(bub);
        }
        bubbleTexts.Clear();
		Destroy(_backdrop);
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;

		baseRadial = 0;
		intersection0 = Vector2.zero;
		intersection1 = Vector2.zero;		
		centroid = Vector3.zero;
		centroidRadius = 0.0f;
	}

	void OnDrawGizmos(){
		//Gizmos.DrawWireSphere(centroid, centroidRadius);
	}
}

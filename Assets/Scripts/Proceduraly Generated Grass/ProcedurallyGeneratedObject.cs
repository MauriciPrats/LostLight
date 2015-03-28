using UnityEngine;
using System.Collections;

public class ProcedurallyGeneratedObject : MonoBehaviour {

	public enum Billboard{Red,Blue,Green,Black};

	public int randomSeed = 0;

	public GameObject objectiveMesh;
	public Texture mapTexture;
	//public GameObject planet;

	public GameObject redBillboard;
	public GameObject greenBillboard;
	public GameObject blueBillboard;

	public float density = 1f;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		//Debug.Log (paintedObject.GetComponent<MeshFilter> ().mesh.uv.Length);
		Vector2[] uvs = objectiveMesh.GetComponent<MeshFilter> ().mesh.uv;
		Vector3[] vertices = objectiveMesh.GetComponent<MeshFilter> ().mesh.vertices;
		Vector3[] normals = objectiveMesh.GetComponent<MeshFilter> ().mesh.normals;
		Texture2D texture = mapTexture as Texture2D;
		int[] triangles = objectiveMesh.GetComponent<MeshFilter> ().mesh.triangles;
		if(texture!=null){
			transform.position = objectiveMesh.transform.position;
			transform.parent = objectiveMesh.transform;

			for(int i = triangles.Length-3;i>=0;i-=3){
				Color vertex1Col = texture.GetPixelBilinear(uvs[triangles[i]].x,uvs[triangles[i]].y);
				Color vertex2Col = texture.GetPixelBilinear(uvs[triangles[i+1]].x,uvs[triangles[i+1]].y);
				Color vertex3Col = texture.GetPixelBilinear(uvs[triangles[i+2]].x,uvs[triangles[i+2]].y);

				Vector3 vertex1Pos = vertices[triangles[i]];
				Vector3 vertex2Pos = vertices[triangles[i+1]];
				Vector3 vertex3Pos = vertices[triangles[i+2]];

				float area = getAreaSize(vertex1Pos,vertex2Pos,vertex3Pos);

				int ammountOfPlantsToPut = (int) (area * density);
				float extraArea = (area*density) - ammountOfPlantsToPut;
				float rand = Random.value;
				if(rand<=extraArea){
					ammountOfPlantsToPut++;
				}

				Vector3 normal = (normals[triangles[i]] + normals[triangles[i+1]] + normals[triangles[i+2]]) / 3f;
				//Debug.Log(normals.Length);

				int redBillboardPlants = 0;
				int greenBillboardPlants = 0;
				int blueBillboardPlants = 0;

				putPlantsNumber(ref redBillboardPlants,ref greenBillboardPlants,ref blueBillboardPlants,getColor(vertex1Col),ammountOfPlantsToPut);
				putPlantsNumber(ref redBillboardPlants,ref greenBillboardPlants,ref blueBillboardPlants,getColor(vertex2Col),ammountOfPlantsToPut);
				putPlantsNumber(ref redBillboardPlants,ref greenBillboardPlants,ref blueBillboardPlants,getColor(vertex3Col),ammountOfPlantsToPut);

				bool differentVertexColors = false;
				if((redBillboardPlants!=0 && greenBillboardPlants!=0 )
				   || (redBillboardPlants!=0 && blueBillboardPlants!=0 )
				   || (blueBillboardPlants!=0 && greenBillboardPlants!=0 )
				   || ((blueBillboardPlants+greenBillboardPlants+redBillboardPlants)<ammountOfPlantsToPut)){
					differentVertexColors = true;
				}

				//If( different vertex colors, then put plants close to vertices, otherwise, random in the triangle

					createRandomPlants(redBillboardPlants,redBillboard,vertex1Pos,vertex2Pos,vertex3Pos,normal);
					createRandomPlants(greenBillboardPlants,greenBillboard,vertex1Pos,vertex2Pos,vertex3Pos,normal);
					createRandomPlants(blueBillboardPlants,blueBillboard,vertex1Pos,vertex2Pos,vertex3Pos,normal);

			}
		}
		transform.localEulerAngles = new Vector3(0f,0f,0f);
		//transform.parent = null;

		/*if(texture!=null){
			transform.position = objectiveMesh.transform.position;
			transform.parent = objectiveMesh.transform;
			for(int i = 0;i<vertices.Length;i++){

				Color vertexColor = texture.GetPixelBilinear(uvs[i].x,uvs[i].y);//texture.GetPixel((int)uvs[i].x,(int)uvs[i].y);

				if(vertexColor.r == vertexColor.b && vertexColor.r == vertexColor.g){
					//We put nothing
				}else if(vertexColor.r > vertexColor.b &&vertexColor.r>vertexColor.g){
					//It's red
					createNewBillboard(redBillboard,vertices[i]);
				}else if(vertexColor.g > vertexColor.b &&vertexColor.g>vertexColor.r){
					//It's green
					createNewBillboard(greenBillboard,vertices[i]);
				}else if(vertexColor.b > vertexColor.r &&vertexColor.b>vertexColor.g){
					//It's blue
					createNewBillboard(blueBillboard,vertices[i]);
				}

			}
			transform.localEulerAngles = new Vector3(0f,0f,0f);
			transform.parent = null;
		}*/
	}

	void createRandomPlants(int ammountOfPlants,GameObject billboard,Vector3 vertix1,Vector3 vertix2,Vector3 vertix3,Vector3 normal){
		for(int i = 0;i<ammountOfPlants;i++){
			float r1 = Random.value;
			float r2 = Random.value;
			Vector3 randomPosition = (1f - Mathf.Sqrt(r1)) * vertix1 + (Mathf.Sqrt(r1) * (1 - r2)) * vertix2 + (Mathf.Sqrt(r1) * r2) * vertix3;
			createNewBillboard(billboard,randomPosition,normal);
		}

	}

	void putPlantsNumber(ref int redPlants,ref int greenPlants,ref int bluePlants,Billboard billboard,int ammountOfTotalPlants){
		float toAdd = (ammountOfTotalPlants * 0.33f);
		int intPart = (int) toAdd;
		float floatPart = toAdd - intPart;
		if(Random.value<=floatPart){
			intPart++;
		}
		if(billboard.Equals(Billboard.Blue)){
			bluePlants+=(int) intPart;
		}else if(billboard.Equals(Billboard.Red)){
			redPlants+=(int) intPart;
		}else if(billboard.Equals(Billboard.Green)){
			greenPlants+=(int) intPart;
		}
	}


	Billboard getColor(Color vertexColor){
		if(vertexColor.r > vertexColor.b &&vertexColor.r>vertexColor.g){
			return Billboard.Red;
		}else if(vertexColor.g > vertexColor.b &&vertexColor.g>vertexColor.r){
			return Billboard.Green;
		}else if(vertexColor.b > vertexColor.r &&vertexColor.b>vertexColor.g){
			return Billboard.Blue;
		}
		return Billboard.Black;
	}

	float getAreaSize(Vector3 p1,Vector3 p2,Vector3 p3){
		//Debug.Log(objectiveMesh.transform.lossyScale.x);
		//float area = (Vector3.Cross(p2-p1,p3-p1).magnitude/2f);
		Vector3 A = p1 * objectiveMesh.transform.lossyScale.x;
		Vector3 B = p2 * objectiveMesh.transform.lossyScale.x;
		Vector3 C = p3 * objectiveMesh.transform.lossyScale.x;

		Vector3 V = Vector3.Cross(A-B, A-C);
		float area = V.magnitude * 0.5f;
		//Debug.Log (area);
		area = area;
		//Debug.Log (area);
		return area;
	}

	void createNewBillboard(GameObject billboard,Vector3 position,Vector3 normal){
		GameObject newObject = Instantiate(billboard) as GameObject;
		newObject.transform.position = (objectiveMesh.transform.position+(position * objectiveMesh.transform.lossyScale.y ));
		newObject.transform.parent = transform;
		newObject.transform.up = (normal).normalized;
		newObject.transform.position += newObject.transform.up * (billboard.GetComponent<MeshRenderer> ().bounds.size.y * 0.5f);

		//Debug.Log (billboard.GetComponent<MeshRenderer> ().bounds.size.y);
		//newObject.transform.RotateAround (newObject.transform.position, newObject.transform.up, Random.value * 180f);
		float zrotation = newObject.transform.eulerAngles.z;
		newObject.transform.forward = Vector3.forward;
		newObject.transform.eulerAngles = new Vector3 (newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y, zrotation);
		//newObject.transform.RotateAround (newObject.transform.position, newObject.transform.forward, (Random.value * 30f)-15f);
		newObject.transform.Rotate (billboard.transform.eulerAngles);

		//Oposite
		/*GameObject newObjectO = Instantiate(billboard) as GameObject;
		newObjectO.transform.position = newObject.transform.position;
		newObjectO.transform.parent = transform;
		newObjectO.transform.forward = newObject.transform.forward * -1f;
		*/
	}

}

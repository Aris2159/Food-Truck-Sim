using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] private LayerMask pickupLayers;
    [SerializeField] GameObject PickupPos;


    private float maxDistance = 20.0f;


    private bool hasObject = false;
    private GameObject PickedItem;


    void Start()
    {
        
    }

   



    void Update()
    {
        
        if  (Input.GetKeyDown(KeyCode.E) && !hasObject) 
            { Shootray();}

        else if (Input.GetKeyDown(KeyCode.E) && hasObject)
            {
                hasObject = false; PickedItem = null;
            }




        if (hasObject)
        {
            moveObj();

        }



    }



    private void moveObj()
    {
        PickedItem.transform.position = PickupPos.transform.position;
            




    }

    private void Shootray()
    {
        
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hitObject;



        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 5f);



        if (Physics.Raycast(ray, out hitObject, maxDistance, pickupLayers))
        {


            PickedItem = hitObject.collider.gameObject;

            hasObject = true;





        }

    }






}

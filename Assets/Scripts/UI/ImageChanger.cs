using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> images;
    [SerializeField] private Image image;
    [SerializeField] private float changeTime;
    private float changeTimeControl;

    private void Update() {
        ChangeImage();
    }

    private void ChangeImage(){
        if((Time.time - changeTimeControl) >= changeTime){
            int index = Random.Range(0, images.Count - 1);
            image.sprite = images[index];
            changeTimeControl = Time.time;
        }
    }
}

/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ImageFilling.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/23
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace MGS.AIReadBoy
{
    [RequireComponent(typeof(Image))]
    public class ImageFilling : MonoBehaviour
    {
        public float speed = 1.0f;
        Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (image.fillAmount >= 1)
            {
                image.fillClockwise = false;
            }
            else if (image.fillAmount <= 0)
            {
                image.fillClockwise = true;
            }

            var vec = image.fillClockwise ? 1 : -1;
            var delta = vec * speed * Time.deltaTime;
            image.fillAmount += delta;
        }
    }
}
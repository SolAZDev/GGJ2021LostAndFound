using System;
using UnityEngine;


namespace FSR
{
    public class FSR_TerrainSurface : MonoBehaviour
    {

        private IndexTerrain indexTerrain = new IndexTerrain();
        [SerializeField] private FSR_Data data;



        public String GetSurface(Vector3 playerPosition)
        {

            String terrain = transform.gameObject.GetComponent<Terrain>().ToString();

            // String[] surfaceName = indexTerrain.GetMainTextureName(playerPosition).Split('_');
            String surfaceName = indexTerrain.GetMainTextureName(playerPosition);

            bool mismatch = true;
            string fSurface = String.Empty;

            foreach (FSR_Data.SurfaceType surface in data.surfaces)
            {

                if (surfaceName.ToLower().Contains(surface.name.ToLower()))
                {
                    mismatch = false;
                    fSurface = surface.name;
                }
            }


            if (!mismatch)
            {
                return fSurface;
            }
            else
            {
                throw new UnityException("looks like you have mismatching surfaces names, make sure all the surfaces components have the same name specified in the FSR data");
            }




        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


    public class ByggPlacerare : MonoBehaviour
    {
        public Grid grid; // grid
        public ByggValet aktivByggnad; // vilken byggnad via byggvalet.cs
        public LayerMask Byggnader; // rätt lzyer

        private Dictionary<Vector3Int, GameObject> placeradeByggnader = new Dictionary<Vector3Int, GameObject>(); // plasts

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // tar musens porition
                Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // tar musens porition 2

                if (!placeradeByggnader.ContainsKey(gridPosition))
                {
                    placeraByggnaden(gridPosition); // byggna till poritionsnen
                }
                else
                {
                    Debug.Log("Ej placerbar här! (redan tagen av annan byggnad) " + placeradeByggnader[gridPosition].name);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                BobGone();
            }
        }

        private void placeraByggnaden(Vector3Int gridPosition)
        {
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // finala positionen plus -8 på z värdet
            finalPosition.z = -8;

            GameObject buildingPrefab = aktivByggnad.getAktivByggnad(); // väljer aktiva byggnaden &
            GameObject newBuilding = Instantiate(buildingPrefab, finalPosition, Quaternion.identity); // placerar den
            newBuilding.layer = LayerMask.NameToLayer("Byggnader"); // på den korrekta layern

            // sparar i hashen
            placeradeByggnader[gridPosition] = newBuilding;

            // sparar vilken byggnad placeras
            if (newBuilding.TryGetComponent<extractorlogik>(out var extractor))
                extractor.placerad = true;
            if (newBuilding.TryGetComponent<lampalogiken>(out var lampa))
                lampa.placerad = true;

            Debug.Log("Byggnad placerad vid: " + finalPosition);
        }

        public void BobGone()
        {
            gameObject.SetActive(false); // hejdå byggare bob
        }

        private bool IntePlaceraVidKnappar()
        {
            return EventSystem.current.IsPointerOverGameObject(); // inte vid knappar
        }
    }

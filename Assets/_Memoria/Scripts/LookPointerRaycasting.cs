using Gamelogic;
using System.Collections;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
	public class LookPointerRaycasting : GLMonoBehaviour, IOnValidate, IUpdate
	{
		public float maxDistance;
		public LayerMask ignoredLayerMask;
		public bool debugOutput;

		private DIOManager _dioManager;
		private RaycastHit _raycastHit;
		private Ray _ray;
		private Vector3 _forwardVector;
		private PitchGrabObject _actualPitchGrabObject;

		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;
		}

		public void OnValidate()
		{
			maxDistance = Mathf.Max(0.0f, maxDistance);
		}

		public void Update()
		{
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();

            if (_dioManager.bgiiesMode)
            {
                if (_dioManager.lookPointerInstanceBgiies.zoomActive)
                    return;
                if (_dioManager.mouseInput)
                {
                    _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
                else
                {
                    if (_dioManager.lookPointerInstanceBgiies.zoomActive)
                        return;
                    _ray = _dioManager.kinectFace.ray;
                    if(_ray.direction != Vector3.zero)
                    {
                        _dioManager.panelBgiies.primerMovimiento = true;
                    }
                }
            }
            else
            {
                _forwardVector = transform.TransformDirection(Vector3.forward);
                _ray = new Ray(transform.position, _forwardVector);
            }

            maxDistance = 1f;
            Debug.DrawLine(_ray.origin, _ray.direction * maxDistance, Color.red);

            if (Physics.Raycast(_ray, out _raycastHit, maxDistance, ignoredLayerMask))
            {
				var posiblePitcheGrabObject = _raycastHit.transform.gameObject.GetComponent<PitchGrabObject>();

				if (posiblePitcheGrabObject == null)
					return;

                if(!_dioManager.panelBgiies.mostrarCategoria)
                {
                    if (posiblePitcheGrabObject.dioController.visualizationController.id != _dioManager.actualVisualization)
                    {
                        if (_actualPitchGrabObject != null)
                        {
                            _actualPitchGrabObject.OnUnDetect();    //funcion para ignorar las imagenes que se encuentren en otras vistas
                        }

                        return;
                    }
                }


				if (_actualPitchGrabObject == null)
				{
					_actualPitchGrabObject = posiblePitcheGrabObject;   //en una primera instancia actualPitch es null, la primera vez que toca una foto valida toma el valor de posiblePitch
                    RegisterRay(_actualPitchGrabObject);
                }
				else
				{
					if (_actualPitchGrabObject.idName != posiblePitcheGrabObject.idName)    //si actualPitch no coincide con posiblePitch se actualiza actualPitch
					{
						_actualPitchGrabObject.OnUnDetect();            // actualPitch se hace null
						_actualPitchGrabObject = posiblePitcheGrabObject;   //se le asigna el valor de posiblePitch
                        RegisterRay(_actualPitchGrabObject);
                    }
				}

				_actualPitchGrabObject.OnDetected();        //activa el MARCAR de buttonPanel y activa LookPointerStay que aplica ZoomIn(iluminar foto)
			}
			else
			{
				if (_actualPitchGrabObject == null)
				{
					_dioManager.buttonPanel.DisableZoomIn();        //si no se apunta a ninguna foto se oscurece la foto
					return;
				}

				_actualPitchGrabObject.OnUnDetect();        //si actualPitch no era nulo se hace null
			}
		}

        public void RegisterRay(PitchGrabObject foto)
        {
            var action = "Move ray vector";
            _dioManager.csvCreator.AddLines(action, foto.idName);
        }
	}
}
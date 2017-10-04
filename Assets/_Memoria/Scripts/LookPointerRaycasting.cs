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
        bool initialized = false;
        //testing

        public void Initialize()
        {
            initialized = true;
        }

		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;
            initialized = true;
		}

		public void OnValidate()
		{
			maxDistance = Mathf.Max(0.0f, maxDistance);
		}

		public void Update()
		{
            if (!initialized)
                return;
            if (GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "BGIIESMode"))
            {
                if (_dioManager.lookPointerInstanceBgiies.zoomActive)
                    return;
                if (_dioManager.mouseInput)
                {
                    if((_dioManager.panelBgiies.posInicialMouse != Input.mousePosition) && !_dioManager.panelBgiies.primerMovimiento)
                    {
                        _dioManager.panelBgiies.primerMovimiento = true;
                        _dioManager.panelBgiies.InitExperiment();
                    }
                        
                    _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
                else
                {
                    if (_dioManager.lookPointerInstanceBgiies.zoomActive)
                        return;
                    _ray = _dioManager.kinectFace.ray;
                    if((_ray.direction != Vector3.zero) && !_dioManager.panelBgiies.primerMovimiento)
                    {
                        _dioManager.panelBgiies.primerMovimiento = true;
                        _dioManager.panelBgiies.InitExperiment();
                    }
                }
            }
            else
            {
                _forwardVector = transform.TransformDirection(Vector3.forward);
                //DELETE THIS trying things
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    /*
                if (EyetrackerManager.Instance._useEyetribe || EyetrackerManager.Instance._useMouse)
                {
                    _ray = EyetrackerManager.Instance.screenPoint;
                }
                else
                {
                    _ray = new Ray(transform.position, _forwardVector);
                }*/
            }

            maxDistance = 1f;
            //Debug.DrawLine(_ray.origin, _ray.direction * maxDistance, Color.red);

            if (Physics.Raycast(_ray, out _raycastHit, maxDistance, ignoredLayerMask))
            {
				var posiblePitcheGrabObject = _raycastHit.transform.gameObject.GetComponent<PitchGrabObject>();

                if (posiblePitcheGrabObject == null)
					return;
                /*
                if (!_dioManager.panelBgiies.mostrarCategoria)
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
                */
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
			{/*
				if (_actualPitchGrabObject == null)
				{
                    if(!_dioManager.bgiiesMode)
                        _dioManager.buttonPanel.DisableZoomIn();
					return;
				}
                */
				_actualPitchGrabObject.OnUnDetect();        //si actualPitch no era nulo se hace null
			}
		}

        public void RegisterRay(PitchGrabObject foto)
        {
            var action = "Move ray vector";
            //DELETE THIS tie to csv creator
            //_dioManager.csvCreator.AddLines(action, foto.idName);
        }

        public void ResetActualPitchGrabObject()
        {
            _actualPitchGrabObject = null;
        }
	}
}
﻿/*
* Copyright (c) <2018> Side Effects Software Inc.
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
* 1. Redistributions of source code must retain the above copyright notice,
*    this list of conditions and the following disclaimer.
*
* 2. The name of Side Effects Software may not be used to endorse or
*    promote products derived from this software without specific prior
*    written permission.
*
* THIS SOFTWARE IS PROVIDED BY SIDE EFFECTS SOFTWARE "AS IS" AND ANY EXPRESS
* OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN
* NO EVENT SHALL SIDE EFFECTS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
* INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
* OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
* LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
* NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
* EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

// Uncomment to profile
//#define HEU_PROFILER_ON

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HoudiniEngineUnity
{
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Typedefs (copy these from HEU_Common.cs)
	using HAPI_NodeId = System.Int32;
	using HAPI_PartId = System.Int32;
	using HAPI_ParmId = System.Int32;
	using HAPI_StringHandle = System.Int32;


	/// <summary>
	/// Represents a Part object containing mesh / geometry/ attribute data.
	/// </summary>
	public class HEU_PartData : ScriptableObject
	{
		//	DATA ------------------------------------------------------------------------------------------------------

		[SerializeField]
		private HAPI_PartId _partID;
		public HAPI_PartId PartID { get { return _partID; } }

		[SerializeField]
		private string _partName;
		public string PartName { get { return _partName; } }

		[SerializeField]
		private HAPI_NodeId _objectNodeID;

		[SerializeField]
		private HAPI_NodeId _geoID;

		[SerializeField]
		private HAPI_PartType _partType;

		[SerializeField]
		private HEU_GeoNode _geoNode;

		public HEU_GeoNode ParentGeoNode { get { return _geoNode; } }

		public HEU_HoudiniAsset ParentAsset { get { return (_geoNode != null) ? _geoNode.ParentAsset : null; } }

		public bool IsPartInstancer() { return _partType == HAPI_PartType.HAPI_PARTTYPE_INSTANCER; }

		[SerializeField]
		private bool _isPartInstanced;

		public bool IsPartInstanced() { return _isPartInstanced; }

		[SerializeField]
		private int _partPointCount;

		public int GetPartPointCount() { return _partPointCount; }

		[SerializeField]
		private bool _isObjectInstancer;

		public bool IsObjectInstancer() { return _isObjectInstancer; }

		[SerializeField]
		private bool _objectInstancesGenerated;

		public bool ObjectInstancesBeenGenerated { get { return _objectInstancesGenerated; } set { _objectInstancesGenerated = value; } }

		[SerializeField]
		private List<HEU_ObjectInstanceInfo> _objectInstanceInfos;

		// Store volume position to use when applying transform
		[SerializeField]
		private Vector3 _terrainOffsetPosition;

#pragma warning disable 0414
		[SerializeField]
		private UnityEngine.Object _assetDBTerrainData;
#pragma warning restore 0414

		public bool IsPartVolume() { return _partOutputType == PartOutputType.VOLUME; }

		public bool IsPartCurve() { return _partOutputType == PartOutputType.CURVE; }

		public bool IsPartMesh() { return _partOutputType == PartOutputType.MESH; }

		[SerializeField]
		private bool _isPartEditable;

		public bool IsPartEditable() { return _isPartEditable; }

		public enum PartOutputType
		{
			NONE,
			MESH,
			VOLUME,
			CURVE,
			INSTANCER
		}
		[SerializeField]
		private PartOutputType _partOutputType;

		[SerializeField]
		private HEU_Curve _curve;

		[SerializeField]
		private HEU_AttributesStore _attributesStore;

		[SerializeField]
		private bool _haveInstancesBeenGenerated;

		public bool HaveInstancesBeenGenerated() { return _haveInstancesBeenGenerated; }

		[SerializeField]
		private int _meshVertexCount;

		public int MeshVertexCount { get { return _meshVertexCount; } }

		[SerializeField]
		private HEU_GeneratedOutput _generatedOutput;

		public HEU_GeneratedOutput GeneratedOutput { get { return _generatedOutput; } }

		public GameObject OutputGameObject { get { return _generatedOutput._outputData._gameObject; } }

		[SerializeField]
		private string _volumeLayerName;


		//  LOGIC -----------------------------------------------------------------------------------------------------


		public HEU_PartData()
		{
			_partID = HEU_Defines.HEU_INVALID_NODE_ID;
			_geoID = HEU_Defines.HEU_INVALID_NODE_ID;
			_objectNodeID = HEU_Defines.HEU_INVALID_NODE_ID;
			_partOutputType = PartOutputType.NONE;

			_generatedOutput = new HEU_GeneratedOutput();
		}

		public void Initialize(HEU_SessionBase session, HAPI_PartId partID, HAPI_NodeId geoID, HAPI_NodeId objectNodeID, HEU_GeoNode geoNode, 
			ref HAPI_PartInfo partInfo, HEU_PartData.PartOutputType partOutputType, bool isEditable, bool isObjectInstancer)
		{
			_partID = partID;
			_geoID = geoID;
			_objectNodeID = objectNodeID;
			_geoNode = geoNode;

			_partOutputType = partOutputType;
			_partType = partInfo.type;
			_partName = HEU_SessionManager.GetString(partInfo.nameSH, session);
			_isPartInstanced = partInfo.isInstanced;
			_partPointCount = partInfo.pointCount;
			_isPartEditable = isEditable;
			_meshVertexCount = partInfo.vertexCount;

			_isObjectInstancer = isObjectInstancer;
			_objectInstancesGenerated = false;
			_objectInstanceInfos = new List<HEU_ObjectInstanceInfo>();

			_volumeLayerName = null;

			//Debug.LogFormat("PartData initialized with ID: {0} and name: {1}", partID, _partName);
		}

		public void SetGameObjectName(string partName)
		{
			if(_generatedOutput._outputData._gameObject == null)
			{
				return;
			}

			string currentName = _generatedOutput._outputData._gameObject.name;
			if (!currentName.Equals(partName) && (!currentName.EndsWith(")") || !currentName.StartsWith(partName)))
			{
				// Only updating name if not already the same. Otherwise GetUniqueNameForSibling will append an unique identifier which is annoying.
				// Also not updating if current name is not a unique version of partName (ie. with (*) appended). This keeps the previous partName as is.
				partName = HEU_EditorUtility.GetUniqueNameForSibling(ParentAsset.RootGameObject.transform, partName);
				_generatedOutput._outputData._gameObject.name = partName;
			}
		}

		public void SetGameObject(GameObject gameObject)
		{
			_generatedOutput._outputData._gameObject = gameObject;
		}

		public void SetVolumeLayerName(string name)
		{
			_volumeLayerName = name;
		}

		public string GetVolumeLayerName()
		{
			return _volumeLayerName;
		}

		/// <summary>
		/// Destroy generated data such as instances and gameobject.
		/// </summary>
		public void DestroyAllData()
		{
			ClearObjectInstanceInfos();

			if(_curve != null)
			{
				if (ParentAsset != null)
				{
					ParentAsset.RemoveCurve(_curve);
				}
				_curve.DestroyAllData();
				HEU_GeneralUtility.DestroyImmediate(_curve);
				_curve = null;
			}

			if(_attributesStore != null)
			{
				DestroyAttributesStore();
			}

			if(_generatedOutput != null)
			{
				HEU_GeneratedOutput.DestroyGeneratedOutput(_generatedOutput);
			}
		}

		/// <summary>
		/// Apply given HAPI transform to this part's gameobject
		/// </summary>
		/// <param name="hapiTransform">The HAPI transform to apply</param>
		public void ApplyHAPITransform(ref HAPI_Transform hapiTransform)
		{
			GameObject outputGO = OutputGameObject;
			if(outputGO == null)
			{
				return;
			}

			if (IsPartVolume())
			{
				HAPI_Transform hapiTransformVolume = new HAPI_Transform();
				hapiTransform.CopyTo(ref hapiTransformVolume);

				hapiTransformVolume.position[0] += _terrainOffsetPosition[0];
				hapiTransformVolume.position[1] += _terrainOffsetPosition[1];
				hapiTransformVolume.position[2] += _terrainOffsetPosition[2];

				HEU_HAPIUtility.ApplyLocalTransfromFromHoudiniToUnity(ref hapiTransformVolume, outputGO.transform);
			}
			else
			{
				HEU_HAPIUtility.ApplyLocalTransfromFromHoudiniToUnity(ref hapiTransform, outputGO.transform);
			}
		}

		/// <summary>
		/// Get debug info for this part
		/// </summary>
		public void GetDebugInfo(StringBuilder sb)
		{
			sb.AppendFormat("PartID: {0}, PartName: {1}, ObjectID: {2}, GeoID: {3}, PartType: {4}, GameObject: {5}\n", PartID, PartName, _objectNodeID, _geoID, _partType, OutputGameObject);
		}

		/// <summary>
		/// Returns true if this part's mesh is using the given material.
		/// </summary>
		/// <param name="materialData">Material data containing the material to check</param>
		/// <returns>True if this part is using the given material</returns>
		public bool IsUsingMaterial(HEU_MaterialData materialData)
		{
			return HEU_GeneratedOutput.IsOutputUsingMaterial(materialData._material, _generatedOutput);
		}

		/// <summary>
		/// Adds gameobjects that should be cloned when cloning the whole asset.
		/// </summary>
		/// <param name="clonableObjects">List of game objects to add to</param>
		public void GetClonableObjects(List<GameObject> clonableObjects)
		{
			// TODO: check if geotype not HAPI_GeoType.HAPI_GEOTYPE_INTERMEDIATE

			if (!IsPartInstanced() && OutputGameObject != null)
			{
				clonableObjects.Add(OutputGameObject);
			}
		}

		public void GetClonableParts(List<HEU_PartData> clonableParts)
		{
			if (!IsPartInstanced() && OutputGameObject != null)
			{
				clonableParts.Add(this);
			}
		}

		/// <summary>
		/// Adds gameobjects that were output from this part.
		/// </summary>
		/// <param name="outputObjects">List to add to</param>
		public void GetOutputGameObjects(List<GameObject> outputObjects)
		{
			// TODO: check if geotype not HAPI_GeoType.HAPI_GEOTYPE_INTERMEDIATE

			if (!IsPartInstanced() && OutputGameObject != null)
			{
				outputObjects.Add(OutputGameObject);
			}
		}

		/// <summary>
		/// Adds this Part's HEU_GeneratedOutput to given output list.
		/// </summary>
		/// <param name="output">List to add to</param>
		public void GetOutput(List<HEU_GeneratedOutput> outputs)
		{
			if (!IsPartInstanced() && _generatedOutput != null)
			{
				outputs.Add(_generatedOutput);
			}
		}

		/// <summary>
		/// Returns self if it has the given output gameobject.
		/// </summary>
		/// <param name="inGameObject">The output gameobject to check</param>
		/// <returns>Valid HEU_PartData or null if no match</returns>
		public HEU_PartData GetHDAPartWithGameObject(GameObject inGameObject)
		{
			return (inGameObject == OutputGameObject) ? this : null;
		}

		private void SetObjectInstancer(bool bObjectInstancer)
		{
			_isObjectInstancer = bObjectInstancer;
		}

        /// <summary>
        /// Clear out existing instances for this part.
        /// </summary>
        public void ClearInstances()
        {
			GameObject outputGO = OutputGameObject;
			if (outputGO == null)
			{
				return;
			}

			List<GameObject> instances = HEU_GeneralUtility.GetInstanceChildObjects(outputGO);
            for (int i = 0; i < instances.Count; ++i)
            {
				HEU_GeneralUtility.DestroyGeneratedComponents(instances[i]);
                HEU_GeneralUtility.DestroyImmediate(instances[i]);
            }

			_haveInstancesBeenGenerated = false;
		}

		/// <summary>
		/// Clear out object instance infos for this part.
		/// </summary>
		private void ClearObjectInstanceInfos()
		{
			if (_objectInstanceInfos != null)
			{
				int numObjInstances = _objectInstanceInfos.Count;
				for (int i = 0; i < numObjInstances; ++i)
				{
					HEU_GeneralUtility.DestroyImmediate(_objectInstanceInfos[i]);
				}
				_objectInstanceInfos.Clear();

				ObjectInstancesBeenGenerated = false;
			}
		}

		/// <summary>
		/// Clean up and remove any HEU_ObjectInstanceInfos that don't have 
		/// valid parts. This can happen if the object node being instanced
		/// has changed (no parts). The instancer should then clear out 
		/// any created HEU_ObjectInstanceInfos for that object node as otherwise
		/// it leaves a dangling instance input for the user.
		/// </summary>
		public void ClearInvalidObjectInstanceInfos()
		{
			if (_objectInstanceInfos != null)
			{
				int numObjInstances = _objectInstanceInfos.Count;
				for (int i = 0; i < numObjInstances; ++i)
				{
					// Presume that if invalid ID then this is using Unity object instead of Houdini generated object
					if(_objectInstanceInfos[i]._instancedObjectNodeID == HEU_Defines.HEU_INVALID_NODE_ID)
					{
						continue;
					}

					bool bDestroyIt = true;
					HEU_ObjectNode instancedObjNode = ParentAsset.GetObjectWithID(_objectInstanceInfos[i]._instancedObjectNodeID);
					if (instancedObjNode != null)
					{
						List<HEU_PartData> cloneParts = new List<HEU_PartData>();
						instancedObjNode.GetClonableParts(cloneParts);
						bDestroyIt = cloneParts.Count == 0;
					}

					if(bDestroyIt)
					{
						HEU_ObjectInstanceInfo objInstanceInfo = _objectInstanceInfos[i];
						_objectInstanceInfos.RemoveAt(i);
						i--;
						numObjInstances = _objectInstanceInfos.Count;

						HEU_GeneralUtility.DestroyImmediate(objInstanceInfo);
					}
				}
			}
		}

		/// <summary>
		/// Clear generated data for this part.
		/// </summary>
		public void ClearGeneratedData()
		{
			ClearInstances();

			// Commented out because we need to keep components around until we parse the cooked data
			// and compare user overrides HEU_GeneralUtility.DestroyGeneratedComponents(_gameObject);

			ObjectInstancesBeenGenerated = false;
		}

		public void ClearGeneratedMeshOutput()
		{
			if (_generatedOutput != null)
			{
				HEU_GeneralUtility.DestroyGeneratedMeshMaterialsLODGroups(_generatedOutput._outputData._gameObject, true);
				HEU_GeneratedOutput.DestroyGeneratedOutputChildren(_generatedOutput);
				HEU_GeneratedOutput.ClearGeneratedMaterialReferences(_generatedOutput._outputData);
				HEU_GeneralUtility.DestroyGeneratedComponents(_generatedOutput._outputData._gameObject);
			}
		}

		public void ClearGeneratedVolumeOutput()
		{
			if (_generatedOutput != null)
			{
				HEU_GeneralUtility.DestroyTerrainComponents(_generatedOutput._outputData._gameObject);
				_assetDBTerrainData = null;
			}
		}

		/// <summary>
		/// Generate part instances (packed primvites).
		/// </summary>
		public void GeneratePartInstances(HEU_SessionBase session)
		{
			if(HaveInstancesBeenGenerated())
			{
				Debug.LogWarningFormat("Part {0} has already had its instances generated!", name);
				return;
			}

			HAPI_PartInfo partInfo = new HAPI_PartInfo();
			if (!session.GetPartInfo(_geoID, _partID, ref partInfo))
			{
				return;
			}

			//Debug.LogFormat("Instancer: name={0}, instanced={1}, instance count={2}, instance part count={3}",
			//	HEU_SessionManager.GetString(partInfo.nameSH, session), partInfo.isInstanced, partInfo.instanceCount, partInfo.instancedPartCount);

			if (!IsPartInstancer())
			{
				Debug.LogErrorFormat("Generate Part Instances called on a non-instancer part {0} for asset {1}!", PartName, ParentAsset.AssetName);
				return;
			}

			if (partInfo.instancedPartCount <= 0)
			{
				Debug.LogErrorFormat("Invalid instanced part count: {0} for part {1} of asset {2}", partInfo.instancedPartCount, PartName, ParentAsset.AssetName);
				return;
			}

			// Get the instance node IDs to get the geometry to be instanced.
			// Get the instanced count to all the instances. These will end up being mesh references to the mesh from instance node IDs.

			Transform partTransform = OutputGameObject.transform;

			// Get each instance's transform
			HAPI_Transform[] instanceTransforms = new HAPI_Transform[partInfo.instanceCount];
			if(!HEU_GeneralUtility.GetArray3Arg(_geoID, PartID, HAPI_RSTOrder.HAPI_SRT, session.GetInstancerPartTransforms, instanceTransforms, 0, partInfo.instanceCount))
			{
				return;
			}

			// Get part IDs for the parts being instanced
			HAPI_NodeId[] instanceNodeIDs = new HAPI_NodeId[partInfo.instancedPartCount];
			if (!HEU_GeneralUtility.GetArray2Arg(_geoID, PartID, session.GetInstancedPartIds, instanceNodeIDs, 0, partInfo.instancedPartCount))
			{
				return;
			}

			// Get instance names if set
			string[] instancePrefixes = null;
			HAPI_AttributeInfo instancePrefixAttrInfo = new HAPI_AttributeInfo();
			HEU_GeneralUtility.GetAttributeInfo(session, _geoID, PartID, HEU_Defines.DEFAULT_INSTANCE_PREFIX_ATTR, ref instancePrefixAttrInfo);
			if(instancePrefixAttrInfo.exists)
			{
				instancePrefixes = HEU_GeneralUtility.GetAttributeStringData(session, _geoID, PartID, HEU_Defines.DEFAULT_INSTANCE_PREFIX_ATTR, ref instancePrefixAttrInfo);
			}

			int numInstances = instanceNodeIDs.Length;
			for (int i = 0; i < numInstances; ++i)
			{
				HEU_PartData partData = _geoNode.GetPartFromPartID(instanceNodeIDs[i]);
				if (partData == null)
				{
					Debug.LogErrorFormat("Part with id {0} is missing. Unable to setup instancer!", instanceNodeIDs[i]);
					return;
				}

				// If the part we're instancing is itself an instancer, make sure it has generated its instances
				if(partData.IsPartInstancer() && !partData.HaveInstancesBeenGenerated())
				{
					partData.GeneratePartInstances(session);
				}

				Debug.Assert(partData.OutputGameObject != null, "Instancer's reference (part) is missing gameobject!");

				HAPI_PartInfo instancePartInfo = new HAPI_PartInfo();
				session.GetPartInfo(_geoID, instanceNodeIDs[i], ref instancePartInfo);

				int numTransforms = instanceTransforms.Length;
				for (int j = 0; j < numTransforms; ++j)
				{
					GameObject newInstanceGO = HEU_EditorUtility.InstantiateGameObject(partData.OutputGameObject, partTransform, false, false);

					newInstanceGO.name = GetInstanceOutputName(PartName, instancePrefixes, (j + 1));

					newInstanceGO.isStatic = OutputGameObject.isStatic;

					HEU_HAPIUtility.ApplyLocalTransfromFromHoudiniToUnity(ref instanceTransforms[j], newInstanceGO.transform);

					// When cloning, the instanced part might have been made invisible, so re-enable renderer to have the cloned instance display it.
					HEU_GeneralUtility.SetGameObjectRenderVisiblity(newInstanceGO, true);
					HEU_GeneralUtility.SetGameObjectChildrenRenderVisibility(newInstanceGO, true);
				}
			}

			_haveInstancesBeenGenerated = true;
		}

		/// <summary>
		/// Generate instances from given Houdini Engine object node ID
		/// </summary>
		/// <param name="session">Active session to use</param>
		/// <param name="objectNodeID">The source object node ID to create instances from</param>
		/// <param name="instancePrefixes">Array of instance names to use</param>
		public void GenerateInstancesFromObjectID(HEU_SessionBase session, HAPI_NodeId objectNodeID, string[] instancePrefixes)
		{
			int numInstances = GetPartPointCount();
			if (numInstances <= 0)
			{
				return;
			}

			HEU_ObjectInstanceInfo instanceInfo = GetObjectInstanceInfoWithObjectID(objectNodeID);
			if (instanceInfo != null && (instanceInfo._instancedInputs.Count > 0))
			{
				List<HEU_InstancedInput> validInstancedGameObjects = instanceInfo._instancedInputs;
				int instancedObjCount = validInstancedGameObjects.Count;

				SetObjectInstancer(true);
				ObjectInstancesBeenGenerated = true;

				Transform partTransform = OutputGameObject.transform;

				HAPI_Transform[] instanceTransforms = new HAPI_Transform[numInstances];
				if (session.GetInstanceTransforms(_geoID, HAPI_RSTOrder.HAPI_SRT, instanceTransforms, 0, numInstances))
				{
					int numTransforms = instanceTransforms.Length;
					for (int j = 0; j < numTransforms; ++j)
					{
						int randomIndex = UnityEngine.Random.Range(0, instancedObjCount);
						CreateNewInstanceFromObject(validInstancedGameObjects[randomIndex]._instancedGameObject, (j + 1), partTransform, 
							ref instanceTransforms[j], objectNodeID, null, 
							validInstancedGameObjects[randomIndex]._rotationOffset, validInstancedGameObjects[randomIndex]._scaleOffset, instancePrefixes);
					}
				}
			}
			else
			{
				HEU_ObjectNode instancedObjNode = ParentAsset.GetObjectWithID(objectNodeID);
				if (instancedObjNode != null)
				{
					GenerateInstancesFromObject(session, instancedObjNode, instancePrefixes);
				}
				else
				{
					Debug.LogWarningFormat("Instanced object with ID {0} not found. Unable to generate instances!", objectNodeID);
				}
			}
		}

		/// <summary>
		/// Generate instances from another object node (sourceObject).
		/// </summary>
		/// <param name="session"></param>
		/// <param name="sourceObject">The object node to create instances from.</param>
		public void GenerateInstancesFromObject(HEU_SessionBase session, HEU_ObjectNode sourceObject, string[] instancePrefixes)
		{
			// Create instance of this object for all points

			List<HEU_PartData> clonableParts = new List<HEU_PartData>();
			sourceObject.GetClonableParts(clonableParts);

			int numInstances = GetPartPointCount();
			if (numInstances <= 0)
			{
				return;
			}

			SetObjectInstancer(true);
			ObjectInstancesBeenGenerated = true;

			Transform partTransform = OutputGameObject.transform;

			HAPI_Transform[] instanceTransforms = new HAPI_Transform[numInstances];
			if (session.GetInstanceTransforms(_geoID, HAPI_RSTOrder.HAPI_SRT, instanceTransforms, 0, numInstances))
			{
				int numInstancesCreated = 0;
				int numTransforms = instanceTransforms.Length;
				for (int j = 0; j < numTransforms; ++j)
				{
					int numClones = clonableParts.Count;
					for (int c = 0; c < numClones; ++c)
					{
						CreateNewInstanceFromObject(clonableParts[c].OutputGameObject, (numInstancesCreated + 1), partTransform, ref instanceTransforms[j], 
							sourceObject.ObjectID, null, Vector3.zero, Vector3.one, instancePrefixes);
						numInstancesCreated++;
					}
				}
			}
		}

		/// <summary>
		/// Generate instances from object IDs found in the asset.
		/// </summary>
		/// <param name="session"></param>
		public void GenerateInstancesFromObjectIds(HEU_SessionBase session, string[] instancePrefixes)
		{
			int numInstances = GetPartPointCount();
			if (numInstances <= 0)
			{
				return;
			}

			HAPI_NodeId[] instancedNodeIds = new HAPI_NodeId[numInstances];
			if (!session.GetInstancedObjectIds(_geoID, instancedNodeIds, 0, numInstances))
			{
				return;
			}

			HAPI_Transform[] instanceTransforms = new HAPI_Transform[numInstances];
			if (!session.GetInstanceTransforms(_geoID, HAPI_RSTOrder.HAPI_SRT, instanceTransforms, 0, numInstances))
			{
				return;
			}

			SetObjectInstancer(true);
			ObjectInstancesBeenGenerated = true;

			Transform partTransform = OutputGameObject.transform;

			int numInstancesCreated = 0;
			for (int i = 0; i < numInstances; ++i)
			{
				if (instancedNodeIds[i] == HEU_Defines.HEU_INVALID_NODE_ID)
				{
					// Skipping points without valid instanced IDs
					continue;
				}

				HEU_ObjectInstanceInfo instanceInfo = GetObjectInstanceInfoWithObjectID(instancedNodeIds[i]);
				if(instanceInfo != null && (instanceInfo._instancedInputs.Count > 0))
				{
					List<HEU_InstancedInput> validInstancedGameObjects = instanceInfo._instancedInputs;
					int randomIndex = UnityEngine.Random.Range(0, validInstancedGameObjects.Count);

					CreateNewInstanceFromObject(validInstancedGameObjects[randomIndex]._instancedGameObject, (numInstancesCreated + 1), partTransform, ref instanceTransforms[i],
						instanceInfo._instancedObjectNodeID, null, 
						validInstancedGameObjects[randomIndex]._rotationOffset, validInstancedGameObjects[randomIndex]._scaleOffset, instancePrefixes);
					numInstancesCreated++;
				}
				else
				{
					HEU_ObjectNode instancedObjNode = ParentAsset.GetObjectWithID(instancedNodeIds[i]);
					if (instancedObjNode == null)
					{
						Debug.LogErrorFormat("Object with ID {0} not found for instancing!", instancedNodeIds[i]);
						continue;
					}

					List<HEU_PartData> cloneParts = new List<HEU_PartData>();
					instancedObjNode.GetClonableParts(cloneParts);

					int numClones = cloneParts.Count;
					for (int c = 0; c < numClones; ++c)
					{
						CreateNewInstanceFromObject(cloneParts[c].OutputGameObject, (numInstancesCreated + 1), partTransform, ref instanceTransforms[i],
							instancedObjNode.ObjectID, null, Vector3.zero, Vector3.one, instancePrefixes);
						numInstancesCreated++;
					}
				}
			}
		}

		/// <summary>
		/// Generate instances from Unity objects specified via attributes. 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="unityInstanceAttr">Name of the attribute to get the Unity path from.</param>
		public void GenerateInstancesFromUnityAssetPathAttribute(HEU_SessionBase session, string unityInstanceAttr)
		{
			int numInstances = GetPartPointCount();
			if (numInstances <= 0)
			{
				return;
			}

			HAPI_Transform[] instanceTransforms = new HAPI_Transform[numInstances];
			if (!session.GetInstanceTransforms(_geoID, HAPI_RSTOrder.HAPI_SRT, instanceTransforms, 0, numInstances))
			{
				return;
			}

			HAPI_AttributeInfo instanceAttrInfo = new HAPI_AttributeInfo();
			int[] instanceAttrID = new int[0];
			HEU_GeneralUtility.GetAttribute(session, _geoID, _partID, unityInstanceAttr, ref instanceAttrInfo, ref instanceAttrID, session.GetAttributeStringData);

			string[] instancePathAttrValues = HEU_SessionManager.GetStringValuesFromStringIndices(instanceAttrID);

			Debug.AssertFormat(instanceAttrInfo.owner == HAPI_AttributeOwner.HAPI_ATTROWNER_POINT, "Expected to parse {0} owner attribute but got {1} instead!", HAPI_AttributeOwner.HAPI_ATTROWNER_POINT, instanceAttrInfo.owner);
			Debug.AssertFormat(instancePathAttrValues.Length == numInstances, "Number of instances {0} does not match point attribute count {1} for part {2} of asset {3}",
				numInstances, instancePathAttrValues.Length, PartName, ParentAsset.AssetName);

			string[] instancePrefixes = null;
			HAPI_AttributeInfo instancePrefixAttrInfo = new HAPI_AttributeInfo();
			HEU_GeneralUtility.GetAttributeInfo(session, _geoID, _partID, HEU_Defines.DEFAULT_INSTANCE_PREFIX_ATTR, ref instancePrefixAttrInfo);
			if (instancePrefixAttrInfo.exists)
			{
				instancePrefixes = HEU_GeneralUtility.GetAttributeStringData(session, _geoID, _partID, HEU_Defines.DEFAULT_INSTANCE_PREFIX_ATTR, ref instancePrefixAttrInfo);
			}

			SetObjectInstancer(true);
			ObjectInstancesBeenGenerated = true;

			Transform partTransform = OutputGameObject.transform;

			// Keep track of loaded objects so we only need to load once for each object
			Dictionary<string, GameObject> loadedUnityObjectMap = new Dictionary<string, GameObject>();

			// Temporary empty gameobject in case where specified Unity object is not found
			GameObject tempGO = null;

			int numInstancesCreated = 0;
			for (int i = 0; i < numInstances; ++i)
			{
				GameObject unitySrcGO = null;

				Vector3 rotationOffset = Vector3.zero;
				Vector3 scaleOffset = Vector3.one;

				HEU_ObjectInstanceInfo instanceInfo = GetObjectInstanceInfoWithObjectPath(instancePathAttrValues[i]);
				if (instanceInfo != null && (instanceInfo._instancedInputs.Count > 0))
				{
					List<HEU_InstancedInput> validInstancedGameObjects = instanceInfo._instancedInputs;
					int randomIndex = UnityEngine.Random.Range(0, validInstancedGameObjects.Count);

					unitySrcGO = validInstancedGameObjects[randomIndex]._instancedGameObject;
					rotationOffset = validInstancedGameObjects[randomIndex]._rotationOffset;
					scaleOffset = validInstancedGameObjects[randomIndex]._scaleOffset;
				}

				if (unitySrcGO == null)
				{
					if (string.IsNullOrEmpty(instancePathAttrValues[i]))
					{
						continue;
					}

					if (!loadedUnityObjectMap.TryGetValue(instancePathAttrValues[i], out unitySrcGO))
					{
						// Try loading it
						HEU_AssetDatabase.ImportAsset(instancePathAttrValues[i], HEU_AssetDatabase.HEU_ImportAssetOptions.Default);
						unitySrcGO = HEU_AssetDatabase.LoadAssetAtPath(instancePathAttrValues[i], typeof(GameObject)) as GameObject;

						if (unitySrcGO == null)
						{
							Debug.LogErrorFormat("Unable to load asset at {0} for instancing!", instancePathAttrValues[i]);

							// Even though the source Unity object is not found, we should create an object instance info so
							// that it will be exposed in UI and user can override
							if (tempGO == null)
							{
								tempGO = new GameObject();
							}
							unitySrcGO = tempGO;
						}

						// Adding to map even if not found so we don't flood the log with the same error message
						loadedUnityObjectMap.Add(instancePathAttrValues[i], unitySrcGO);
					}
				}

				CreateNewInstanceFromObject(unitySrcGO, (numInstancesCreated + 1), partTransform, ref instanceTransforms[i], 
					HEU_Defines.HEU_INVALID_NODE_ID, instancePathAttrValues[i], rotationOffset, scaleOffset, instancePrefixes);
				numInstancesCreated++;
			}

			if (tempGO != null)
			{
				HEU_GeneralUtility.DestroyImmediate(tempGO, bRegisterUndo:false);
			}
		}

		/// <summary>
		/// Generate instances from a single existing Unity asset.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="assetPath"></param>
		public void GenerateInstancesFromUnityAssetPath(HEU_SessionBase session, string assetPath, string[] instancePrefixes)
		{
			int numInstances = GetPartPointCount();
			if (numInstances <= 0)
			{
				return;
			}

			HAPI_Transform[] instanceTransforms = new HAPI_Transform[numInstances];
			if (!session.GetInstanceTransforms(_geoID, HAPI_RSTOrder.HAPI_SRT, instanceTransforms, 0, numInstances))
			{
				return;
			}

			SetObjectInstancer(true);
			ObjectInstancesBeenGenerated = true;

			GameObject instancedAssetGameObject = null;

			HEU_ObjectInstanceInfo instanceInfo = GetObjectInstanceInfoWithObjectPath(assetPath);

			List<HEU_InstancedInput> validInstancedGameObjects = null;
			int instancedObjCount = 0;

			Vector3 rotationOffset = Vector3.zero;
			Vector3 scaleOffset = Vector3.one;

			if (instanceInfo != null && (instanceInfo._instancedInputs.Count > 0))
			{
				validInstancedGameObjects = instanceInfo._instancedInputs;
				instancedObjCount = validInstancedGameObjects.Count;
			}
			
			if(instancedObjCount == 0)
			{
				HEU_AssetDatabase.ImportAsset(assetPath, HEU_AssetDatabase.HEU_ImportAssetOptions.Default);
				instancedAssetGameObject = HEU_AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
			}

			if (instancedAssetGameObject != null)
			{
				int numInstancesCreated = 0;
				for (int i = 0; i < numInstances; ++i)
				{
					GameObject instancedGameObject;
					if(instancedAssetGameObject == null)
					{
						// Get random override
						int randomIndex = UnityEngine.Random.Range(0, instancedObjCount);
						instancedGameObject = validInstancedGameObjects[randomIndex]._instancedGameObject;
						rotationOffset = validInstancedGameObjects[randomIndex]._rotationOffset;
						scaleOffset = validInstancedGameObjects[randomIndex]._scaleOffset;
					}
					else
					{
						instancedGameObject = instancedAssetGameObject;
					}

					CreateNewInstanceFromObject(instancedGameObject, (numInstancesCreated + 1), OutputGameObject.transform, ref instanceTransforms[i], 
						HEU_Defines.HEU_INVALID_NODE_ID, assetPath, rotationOffset, scaleOffset, instancePrefixes);
					numInstancesCreated++;
				}
			}
			else
			{
				Debug.LogErrorFormat("Unable to load asset at {0} for instancing!", assetPath);
			}
		}

		/// <summary>
		/// Create a new instance of the sourceObject.
		/// </summary>
		/// <param name="sourceObject">GameObject to instance.</param>
		/// <param name="instanceIndex">Index of the instance within the part.</param>
		/// <param name="parentTransform">Parent of the new instance.</param>
		/// <param name="hapiTransform">HAPI transform to apply to the new instance.</param>
		private void CreateNewInstanceFromObject(GameObject sourceObject, int instanceIndex, Transform parentTransform, ref HAPI_Transform hapiTransform, 
			HAPI_NodeId instancedObjectNodeID, string instancedObjectPath, Vector3 rotationOffset, Vector3 scaleOffset, string[] instancePrefixes)
		{
			GameObject newInstanceGO = null;

			if (HEU_EditorUtility.IsPrefabOriginal(sourceObject))
			{
				newInstanceGO = HEU_EditorUtility.InstantiatePrefab(sourceObject) as GameObject;
				newInstanceGO.transform.parent = parentTransform;
			}
			else
			{
				newInstanceGO = HEU_EditorUtility.InstantiateGameObject(sourceObject, parentTransform, false, false);	
			}

			// To get the instance output name, we pass in the instance index. The actual name will be +1 from this.
			newInstanceGO.name = GetInstanceOutputName(PartName, instancePrefixes, instanceIndex);

			newInstanceGO.isStatic = OutputGameObject.isStatic;

			Transform instanceTransform = newInstanceGO.transform;
			HEU_HAPIUtility.ApplyLocalTransfromFromHoudiniToUnity(ref hapiTransform, instanceTransform);

			// Apply offsets
			Vector3 rotation = instanceTransform.localRotation.eulerAngles;
			instanceTransform.localRotation = Quaternion.Euler(rotation + rotationOffset);
			instanceTransform.localScale = Vector3.Scale(instanceTransform.localScale, scaleOffset);

			// When cloning, the instanced part might have been made invisible, so re-enable renderer to have the cloned instance display it.
			HEU_GeneralUtility.SetGameObjectRenderVisiblity(newInstanceGO, true);
			HEU_GeneralUtility.SetGameObjectChildrenRenderVisibility(newInstanceGO, true);

			// Add to object instance info map. Find existing object instance info, or create it.
			HEU_ObjectInstanceInfo instanceInfo = null;
			if(instancedObjectNodeID != HEU_Defines.HEU_INVALID_NODE_ID)
			{
				instanceInfo = GetObjectInstanceInfoWithObjectID(instancedObjectNodeID);
			}
			else if(!string.IsNullOrEmpty(instancedObjectPath))
			{
				instanceInfo = GetObjectInstanceInfoWithObjectPath(instancedObjectPath);
			}

			if (instanceInfo == null)
			{
				instanceInfo = CreateObjectInstanceInfo(sourceObject, instancedObjectNodeID, instancedObjectPath);
			}

			instanceInfo._instances.Add(newInstanceGO);
		}

		/// <summary>
		/// Returns the output instance's name for given instance index. 
		/// The instance name convention is: PartName_Instance1
		/// User could override the prefix (PartName) with their own via given instancePrefixes array.
		/// </summary>
		/// <param name="partName"></param>
		/// <param name="userPrefix"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string GetInstanceOutputName(string partName, string[] userPrefix, int index)
		{
			string prefix = null;
			if(userPrefix == null || userPrefix.Length == 0)
			{
				prefix = partName;
			}
			else if(userPrefix.Length == 1)
			{
				prefix = userPrefix[0];
			}
			else if(index >= 0 && (index <= userPrefix.Length))
			{
				prefix = userPrefix[index - 1];
			}
			return prefix + HEU_Defines.HEU_INSTANCE + index;
		}

		public HEU_Curve GetCurve(bool bEditableOnly)
		{
			if(_curve != null && (!bEditableOnly || _curve.IsEditable()))
			{
				return _curve;
			}
			return null;
		}

		/// <summary>
		/// Set visibility on this part's gameobject.
		/// </summary>
		/// <param name="bVisibility">True if visible.</param>
		public void SetVisiblity(bool bVisibility)
		{
			if(_curve != null)
			{
				bVisibility &= HEU_PluginSettings.Curves_ShowInSceneView;
			}

			if(HEU_GeneratedOutput.HasLODGroup(_generatedOutput))
			{
				foreach(HEU_GeneratedOutputData childOutput in _generatedOutput._childOutputs)
				{
					HEU_GeneralUtility.SetGameObjectRenderVisiblity(childOutput._gameObject, bVisibility);
				}
			}
			else
			{
				HEU_GeneralUtility.SetGameObjectRenderVisiblity(OutputGameObject, bVisibility);
			}
		}

		/// <summary>
		/// Calculate the visiblity of this part, based on parent's state and part properties.
		/// </summary>
		/// <param name="bParentVisibility">True if parent is visible</param>
		/// <param name="bParentDisplayGeo">True if parent is a display node</param>
		public void CalculateVisibility(bool bParentVisibility, bool bParentDisplayGeo)
		{
			// Editable part is hidden unless parent is a display geo
			bool bIsVisible = !IsPartInstanced() && bParentVisibility && (!_isPartEditable || bParentDisplayGeo);
			SetVisiblity(bIsVisible);
		}

		public void SetColliderState(bool bEnabled)
		{
			HEU_GeneralUtility.SetGameObjectColliderState(OutputGameObject, bEnabled);
		}

		public void CalculateColliderState()
		{
			// Using visiblity to figure out collider state, for now
			bool bEnabled = false;

			if (HEU_GeneratedOutput.HasLODGroup(_generatedOutput))
			{
				foreach (HEU_GeneratedOutputData childOutput in _generatedOutput._childOutputs)
				{
					MeshRenderer partMeshRenderer = childOutput._gameObject.GetComponent<MeshRenderer>();
					if (partMeshRenderer != null)
					{
						bEnabled = partMeshRenderer.enabled;
					}
					else
					{
						bEnabled = false;
					}
					HEU_GeneralUtility.SetGameObjectColliderState(childOutput._gameObject, bEnabled);
				}
			}
			else
			{
				MeshRenderer partMeshRenderer = OutputGameObject.GetComponent<MeshRenderer>();
				if (partMeshRenderer != null)
				{
					bEnabled = partMeshRenderer.enabled;
				}
				HEU_GeneralUtility.SetGameObjectColliderState(OutputGameObject, bEnabled);
			}
		}

		/// <summary>
		/// Copy relevant components from sourceGO to targetGO.
		/// </summary>
		/// <param name="sourceGO">Source gameobject to copy from.</param>
		/// <param name="targetGO">Target gameobject to copy to.</param>
		/// <param name="assetName">Name of the asset.</param>
		/// <param name="sourceToTargetMeshMap">Map of existing meshes to newly created meshes. This helps keep track of shared meshes that should be copied but still shared in new asset.</param>
		/// <param name="sourceToCopiedMaterials">Map of existing materials with their new copied counterparts. Keeps track of which materials have been newly copied in order to reuse.</param>
		/// <param name="bWriteMeshesToAssetDatabase">Whether to store meshes to database. Required for prefabs.</param>
		/// <param name="bakedAssetPath">Path to asset's database cache. Could be null in which case it will be filled.</param>
		/// <param name="assetDBObject">The asset database object to write out the persistent mesh data to. Could be null, in which case it might be created.</param>
		/// <param name="assetObjectFileName">File name of the asset database object. This will be used to create new assetDBObject.</param>
		private void CopyGameObjectComponents(GameObject sourceGO, GameObject targetGO, string assetName, Dictionary<Mesh, Mesh> sourceToTargetMeshMap, Dictionary<Material, Material> sourceToCopiedMaterials, bool bWriteMeshesToAssetDatabase, 
			ref string bakedAssetPath, ref UnityEngine.Object assetDBObject, string assetObjectFileName, bool bDeleteExistingComponents, bool bDontDeletePersistantResources)
		{
			// Copy mesh, collider, material, and textures into its own directory in the Assets folder

			HEU_HoudiniAsset parentAsset = ParentAsset;

			// Handle LOD group. This should have child gameobjects whose components need to be parsed properly to make sure
			// the mesh and materials are properly copied.
			LODGroup sourceLODGroup = sourceGO.GetComponent<LODGroup>();
			if (sourceLODGroup != null)
			{
				LODGroup targetLODGroup = targetGO.GetComponent<LODGroup>();
				if(targetLODGroup == null)
				{
					targetLODGroup = targetGO.AddComponent<LODGroup>();
				}

				CopyChildGameObjects(sourceGO, targetGO, assetName, sourceToTargetMeshMap, sourceToCopiedMaterials, bWriteMeshesToAssetDatabase, ref bakedAssetPath,
						ref assetDBObject, assetObjectFileName, bDeleteExistingComponents, bDontDeletePersistantResources);

				LOD[] sourceLODs = targetLODGroup.GetLODs();
				if(sourceLODs != null)
				{
					List<GameObject> targetChilden = HEU_GeneralUtility.GetChildGameObjects(targetGO);

					LOD[] targetLODs = new LOD[sourceLODs.Length];
					for(int i = 0; i < sourceLODs.Length; ++i)
					{
						if (sourceLODs[i].renderers != null && sourceLODs[i].renderers.Length > 0)
						{
							GameObject childGO = sourceLODs[i].renderers[0].gameObject;
							if (childGO != null)
							{
								GameObject targetChildGO = HEU_GeneralUtility.GetGameObjectByName(targetChilden, childGO.name);
								if (targetChildGO != null)
								{
									targetLODs[i] = new LOD(sourceLODs[i].screenRelativeTransitionHeight, targetChildGO.GetComponents<Renderer>());
								}
							}
						}
					}

					// Sort by screen transition as it might not be properly ordered. Unity complains if not in decreasing order.
					System.Array.Sort(targetLODs, (a, b) => (b.screenRelativeTransitionHeight > a.screenRelativeTransitionHeight) ? 1 : -1);

					targetLODGroup.SetLODs(targetLODs);
				}
			}

			// Mesh for render
			MeshFilter targetMeshFilter = targetGO.GetComponent<MeshFilter>();
			MeshFilter sourceMeshFilter = sourceGO.GetComponent<MeshFilter>();
			if (sourceMeshFilter != null)
			{
				if (targetMeshFilter == null)
				{
					targetMeshFilter = HEU_EditorUtility.AddComponent<MeshFilter>(targetGO, true) as MeshFilter;
				}

				Mesh originalMesh = sourceMeshFilter.sharedMesh;
				if (originalMesh != null)
				{
					Mesh targetMesh = null;
					if (!sourceToTargetMeshMap.TryGetValue(originalMesh, out targetMesh))
					{
						// Create this mesh
						targetMesh = Mesh.Instantiate(originalMesh) as Mesh;
						sourceToTargetMeshMap[originalMesh] = targetMesh;

						if (bWriteMeshesToAssetDatabase)
						{
							HEU_AssetDatabase.CreateAddObjectInAssetCacheFolder(assetName, assetObjectFileName, targetMesh, ref bakedAssetPath, ref assetDBObject);
						}
					}

					targetMeshFilter.sharedMesh = targetMesh;
				}
			}
			else if (targetMeshFilter != null)
			{
				HEU_GeneralUtility.DestroyImmediate(targetMeshFilter);
			}

			// Mesh for collider
			MeshCollider targetMeshCollider = targetGO.GetComponent<MeshCollider>();
			MeshCollider sourceMeshCollider = sourceGO.GetComponent<MeshCollider>();
			if (sourceMeshCollider != null)
			{
				if (targetMeshCollider == null)
				{
					targetMeshCollider = HEU_EditorUtility.AddComponent<MeshCollider>(targetGO, true) as MeshCollider;
				}

				Mesh originalColliderMesh = sourceMeshCollider.sharedMesh;
				if (originalColliderMesh != null)
				{
					Mesh targetColliderMesh = null;
					if (!sourceToTargetMeshMap.TryGetValue(originalColliderMesh, out targetColliderMesh))
					{
						// Create this mesh
						targetColliderMesh = Mesh.Instantiate(originalColliderMesh) as Mesh;
						sourceToTargetMeshMap[originalColliderMesh] = targetColliderMesh;

						if (bWriteMeshesToAssetDatabase)
						{
							HEU_AssetDatabase.CreateAddObjectInAssetCacheFolder(assetName, assetObjectFileName, targetColliderMesh, ref bakedAssetPath, ref assetDBObject);
						}
					}

					targetMeshCollider.sharedMesh = targetColliderMesh;
				}
			}
			else if (targetMeshCollider != null)
			{
				HEU_GeneralUtility.DestroyImmediate(targetMeshFilter);
			}

			// Materials and textures
			MeshRenderer targetMeshRenderer = targetGO.GetComponent<MeshRenderer>();
			MeshRenderer sourceMeshRenderer = sourceGO.GetComponent<MeshRenderer>();
			if (sourceMeshRenderer != null)
			{
				if (targetMeshRenderer == null)
				{
					targetMeshRenderer = HEU_EditorUtility.AddComponent<MeshRenderer>(targetGO, true) as MeshRenderer;
				}

				Material[] generatedMaterials = HEU_GeneratedOutput.GetGeneratedMaterialsForGameObject(_generatedOutput, sourceGO);

				Material[] materials = sourceMeshRenderer.sharedMaterials;
				if (materials != null && materials.Length > 0)
				{
					if (string.IsNullOrEmpty(bakedAssetPath))
					{
						// Need to create the baked folder in order to store materials and textures
						bakedAssetPath = HEU_AssetDatabase.CreateUniqueBakePath(assetName);
					}

					int numMaterials = materials.Length;
					for (int m = 0; m < numMaterials; ++m)
					{
						Material srcMaterial = materials[m];
						if(srcMaterial == null)
						{
							continue;
						}

						Material newMaterial = null;
						if (sourceToCopiedMaterials.TryGetValue(srcMaterial, out newMaterial))
						{
							materials[m] = newMaterial;
							continue;
						}

						// If srcMaterial is a Unity material (not Houdini generated), then skip copying
						HEU_MaterialData materialData = parentAsset.GetMaterialData(srcMaterial);
						if (materialData != null && materialData.IsExistingMaterial())
						{
							continue;
						}

						// Check override material
						if(generatedMaterials != null && (m < generatedMaterials.Length) && (srcMaterial != generatedMaterials[m]))
						{
							// This materials has been overriden. No need to copy it, just use as is.
							continue;
						}

						string materialPath = HEU_AssetDatabase.GetAssetPath(srcMaterial);
						if (!string.IsNullOrEmpty(materialPath) && HEU_AssetDatabase.IsPathInAssetCache(materialPath))
						{
							newMaterial = HEU_AssetDatabase.LoadAssetCopy(srcMaterial, bakedAssetPath, typeof(Material), false) as Material;
							if (newMaterial == null)
							{
								throw new HEU_HoudiniEngineError(string.Format("Unable to copy material. Stopping bake!"));
							}
						}
						else if(HEU_AssetDatabase.ContainsAsset(srcMaterial))
						{
							// Material is stored in Asset Database, but outside the cache. This is most likely an existing material specified by user, so use as is.
							continue;
						}
						else
						{
							// Material is not in Asset Database (probably default material). So create a copy of it in Asset Database.
							newMaterial = HEU_MaterialFactory.CopyMaterial(srcMaterial);
							HEU_MaterialFactory.WriteMaterialToAssetCache(newMaterial, bakedAssetPath, newMaterial.name);
						}

						if(newMaterial != null)
						{
							sourceToCopiedMaterials.Add(srcMaterial, newMaterial);

							// Diffuse texture
							if (newMaterial.HasProperty("_MainTex"))
							{
								Texture srcDiffuseTexture = newMaterial.mainTexture;
								if (srcDiffuseTexture != null)
								{
									Texture newDiffuseTexture = HEU_AssetDatabase.LoadAssetCopy(srcDiffuseTexture, bakedAssetPath, typeof(Texture), false) as Texture;
									if (newDiffuseTexture == null)
									{
										throw new HEU_HoudiniEngineError(string.Format("Unable to copy texture. Stopping bake!"));
									}
									newMaterial.mainTexture = newDiffuseTexture;
								}
							}

							// Normal map
							Texture srcNormalMap = materials[m].GetTexture(HEU_Defines.UNITY_SHADER_BUMP_MAP);
							if (srcNormalMap != null)
							{
								Texture newNormalMap = HEU_AssetDatabase.LoadAssetCopy(srcNormalMap, bakedAssetPath, typeof(Texture), false) as Texture;
								if (newNormalMap == null)
								{
									throw new HEU_HoudiniEngineError(string.Format("Unable to copy texture. Stopping bake!"));
								}
								newMaterial.SetTexture(HEU_Defines.UNITY_SHADER_BUMP_MAP, newNormalMap);
							}

							materials[m] = newMaterial;
						}
					}

					targetMeshRenderer.sharedMaterials = materials;
				}
			}
			else if (targetMeshRenderer != null)
			{
				HEU_GeneralUtility.DestroyImmediate(targetMeshRenderer);
			}

			// Terrain component
			Terrain targetTerrain = targetGO.GetComponent<Terrain>();
			Terrain sourceTerrain = sourceGO.GetComponent<Terrain>();
			TerrainData targetTerrainData = null;
			if (sourceTerrain != null)
			{
				if (targetTerrain == null)
				{
					targetTerrain = HEU_EditorUtility.AddComponent<Terrain>(targetGO, true) as Terrain;
				}

				TerrainData sourceTerrainData = sourceTerrain.terrainData;
				if (sourceTerrainData != null)
				{
					targetTerrainData = targetTerrain.terrainData;
					if (targetTerrainData != null && targetTerrainData != sourceTerrainData && HEU_AssetDatabase.ContainsAsset(targetTerrainData))
					{
						// Get path to existing terrain data asset location
						bakedAssetPath = HEU_AssetDatabase.GetAssetRootPath(targetTerrainData);
					}

					//targetTerrainData = TerrainData.Instantiate(sourceTerrainData);
					//targetTerrain.terrainData = targetTerrainData;
					//targetTerrain.Flush();

					// Note: ignoring bWriteMeshesToAssetDatabase and always writing to asset db
					//HEU_AssetDatabase.CreateAddObjectInAssetCacheFolder(assetName, assetObjectFileName, targetTerrainData, ref bakedAssetPath, ref assetDBObject);

					if (string.IsNullOrEmpty(bakedAssetPath))
					{
						bakedAssetPath = HEU_AssetDatabase.CreateUniqueBakePath(assetName);
					}

					// We're going to copy the source terrain data asset file, then load the copy and assign to the target
					// Note: ignoring bWriteMeshesToAssetDatabase and always writing to asset db because terrain data needs to be stored on file
					targetTerrainData = HEU_AssetDatabase.LoadAssetCopy(sourceTerrainData, bakedAssetPath, typeof(TerrainData), true) as TerrainData;
					targetTerrain.terrainData = targetTerrainData;
					targetTerrain.Flush();
				}
			}
			else if (targetTerrain != null)
			{
				targetTerrainData = targetTerrain.terrainData;
				if (HEU_AssetDatabase.ContainsAsset(targetTerrainData))
				{
					targetTerrain.terrainData = null;
					HEU_AssetDatabase.DeleteAsset(targetTerrainData);
					targetTerrainData = null;
				}

				HEU_GeneralUtility.DestroyImmediate(targetTerrain);
			}

			// Terrain collider
			TerrainCollider targetTerrainCollider = targetGO.GetComponent<TerrainCollider>();
			TerrainCollider sourceTerrainCollider = sourceGO.GetComponent<TerrainCollider>();
			if (sourceTerrainCollider != null)
			{
				if (targetTerrainCollider == null)
				{
					targetTerrainCollider = HEU_EditorUtility.AddComponent<TerrainCollider>(targetGO, true) as TerrainCollider;
				}

				targetTerrainCollider.terrainData = targetTerrainData;
			}
			else if(targetTerrainCollider != null)
			{
				HEU_GeneralUtility.DestroyImmediate(targetTerrainCollider);
			}
		}

		/// <summary>
		/// Copy the child GameObjects of the given sourceGO to targetGO, along with making sure all components have been properly copied.
		/// targetGO might already have existing children.
		/// </summary>
		/// <param name="sourceGO">Source gameobject to copy from.</param>
		/// <param name="targetGO">Target gameobject to copy to.</param>
		/// <param name="assetName">Name of the asset.</param>
		/// <param name="sourceToTargetMeshMap">Map of existing meshes to newly created meshes. This helps keep track of shared meshes that should be copied but still shared in new asset.</param>
		/// <param name="sourceToCopiedMaterials">Map of existing materials with their new copied counterparts. Keeps track of which materials have been newly copied in order to reuse.</param>
		/// <param name="bWriteMeshesToAssetDatabase">Whether to store meshes to database. Required for prefabs.</param>
		/// <param name="bakedAssetPath">Path to asset's database cache. Could be null in which case it will be filled.</param>
		/// <param name="assetDBObject">The asset database object to write out the persistent mesh data to. Could be null, in which case it might be created.</param>
		/// <param name="assetObjectFileName">File name of the asset database object. This will be used to create new assetDBObject.</param>
		/// <param name="bDeleteExistingComponents">True if should delete existing components to then re-add.</param>
		/// <param name="bDontDeletePersistantResources">True if not to delete persisten file resources in the project.</param>
		private void CopyChildGameObjects(GameObject sourceGO, GameObject targetGO, string assetName, Dictionary<Mesh, Mesh> sourceToTargetMeshMap, Dictionary<Material, Material> sourceToCopiedMaterials,
			bool bWriteMeshesToAssetDatabase, ref string bakedAssetPath, ref UnityEngine.Object assetDBObject, string assetObjectFileName, bool bDeleteExistingComponents, bool bDontDeletePersistantResources)
		{
			Transform targetTransform = targetGO.transform;
			List<GameObject> unprocessedTargetChildren = HEU_GeneralUtility.GetChildGameObjects(targetGO);

			List<GameObject> srcChildGameObjects = HEU_GeneralUtility.GetChildGameObjects(sourceGO);
			int numChildren = srcChildGameObjects.Count;
			for (int i = 0; i < numChildren; ++i)
			{
				GameObject srcChildGO = srcChildGameObjects[i];

				GameObject targetChildGO = HEU_GeneralUtility.GetGameObjectByName(unprocessedTargetChildren, srcChildGO.name);
				if (targetChildGO == null)
				{
					targetChildGO = new GameObject(srcChildGO.name);
					targetChildGO.transform.parent = targetTransform;
				}
				else
				{
					if (bDeleteExistingComponents)
					{
						HEU_GeneralUtility.DestroyGeneratedMeshMaterialsLODGroups(targetChildGO, bDontDeletePersistantResources);
					}

					unprocessedTargetChildren.Remove(targetChildGO);

					// Update transform of each existing instance
					HEU_GeneralUtility.CopyLocalTransformValues(srcChildGO.transform, targetChildGO.transform);
				}

				// Copy component data
				CopyGameObjectComponents(srcChildGO, targetChildGO, assetName, sourceToTargetMeshMap, sourceToCopiedMaterials, bWriteMeshesToAssetDatabase, ref bakedAssetPath, 
					ref assetDBObject, assetObjectFileName, bDeleteExistingComponents, bDontDeletePersistantResources);
			}

			if (unprocessedTargetChildren.Count > 0)
			{
				// Clean up any children that we haven't updated as they don't exist in the source
				HEU_GeneralUtility.DestroyBakedGameObjects(unprocessedTargetChildren);
			}
		}

		/// <summary>
		/// Bake this part out to a new gameobject, and returns it. 
		/// Copies all relevant components.
		/// Supports baking of part and object instances.
		/// </summary>
		/// <param name="parentTransform">The parent for the new object. Can be null.</param>
		/// <param name="bWriteMeshesToAssetDatabase">Whether to store meshes to database. Required for prefabs.</param>
		/// <param name="bakedAssetPath">Path to asset's database cache. Could be null in which case it will be filled.</param>
		/// <param name="sourceToTargetMeshMap">Map of existing meshes to newly created meshes. This helps keep track of shared meshes that should be copied but still shared in new asset.</param>
		/// <param name="sourceToCopiedMaterials">Map of existing materials with their new copied counterparts. Keeps track of which materials have been newly copied in order to reuse.</param>
		/// <param name="assetDBObject">The asset database object to write out the persistent mesh data to. Could be null, in which case it might be created.</param>
		/// <param name="assetObjectFileName">File name of the asset database object. This will be used to create new assetDBObject.</param>
		/// <param name="bReconnectPrefabInstances">Reconnect prefab instances to its prefab parent.</param>
		/// <returns>The newly created gameobject.</returns>
		public GameObject BakePartToNewGameObject(Transform parentTransform, bool bWriteMeshesToAssetDatabase, ref string bakedAssetPath, Dictionary<Mesh, Mesh> sourceToTargetMeshMap, Dictionary<Material, Material> sourceToCopiedMaterials, ref UnityEngine.Object assetDBObject, string assetObjectFileName, bool bReconnectPrefabInstances)
		{
			GameObject outputGameObject = OutputGameObject;
			if (outputGameObject == null)
			{
				return null;
			}
			// This creates a copy of the part's gameobject, along with instances if it has them.
			// If the instances are prefab instances, then this disconnects the connection. We re-connect them back in the call below.
			GameObject targetGO = HEU_EditorUtility.InstantiateGameObject(outputGameObject, parentTransform, true, true);
			targetGO.name = HEU_PartData.AppendBakedCloneName(outputGameObject.name);

			BakePartToGameObject(targetGO, false, false, bWriteMeshesToAssetDatabase, ref bakedAssetPath, sourceToTargetMeshMap, sourceToCopiedMaterials, ref assetDBObject, assetObjectFileName, bReconnectPrefabInstances);

			return targetGO;
		}

		/// <summary>
		/// Bake this part out to the given targetGO. Existing components might be destroyed.
		/// Supports baking of part and object instances.
		/// </summary>
		/// <param name="targetGO">Target gameobject to bake out to.</param>
		/// <param name="bDeleteExistingComponents">Whether to destroy existing components on the targetGO.</param>
		/// <param name="bDontDeletePersistantResources">Whether to delete persistant resources stored in the project.</param>
		/// <param name="bWriteMeshesToAssetDatabase">Whether to store meshes to database. Required for prefabs.</param>
		/// <param name="bakedAssetPath">Path to asset's database cache. Could be null in which case it will be filled.</param>
		/// <param name="sourceToTargetMeshMap">Map of existing meshes to newly created meshes. This helps keep track of shared meshes that should be copied but still shared in new asset.</param>
		/// <param name="sourceToCopiedMaterials">Map of existing materials with their new copied counterparts. Keeps track of which materials have been newly copied in order to reuse.</param>
		/// <param name="assetDBObject">The asset database object to write out the persistent mesh data to. Could be null, in which case it might be created.</param>
		/// <param name="assetObjectFileName">File name of the asset database object. This will be used to create new assetDBObject.</param>
		/// <param name="bReconnectPrefabInstances">Reconnect prefab instances to its prefab parent.</param>
		public void BakePartToGameObject(GameObject targetGO, bool bDeleteExistingComponents, bool bDontDeletePersistantResources, bool bWriteMeshesToAssetDatabase, ref string bakedAssetPath, Dictionary<Mesh, Mesh> sourceToTargetMeshMap, Dictionary<Material, Material> sourceToCopiedMaterials, ref UnityEngine.Object assetDBObject, string assetObjectFileName, bool bReconnectPrefabInstances)
		{
			GameObject outputGameObject = OutputGameObject;
			if (outputGameObject == null)
			{
				return;
			}
			else if (outputGameObject == targetGO)
			{
				Debug.LogError("Copy and target objects cannot be the same!");
				return;
			}

			string assetName = ParentAsset.AssetName;

			Transform targetTransform = targetGO.transform;

			if (IsPartInstancer() || IsObjectInstancer())
			{
				// Instancer

				// Instancer has a gameobject with children. The parent is an empty transform, while the
				// the children have all the data. The children could have an assortment of meshes.

				// Keeps track of unprocessed children. Any leftover will be destroyed.
				List<GameObject> unprocessedTargetChildren = HEU_GeneralUtility.GetChildGameObjects(targetGO);

				List<GameObject> srcChildGameObjects = HEU_GeneralUtility.GetChildGameObjects(outputGameObject);
				int numChildren = srcChildGameObjects.Count;
				for (int i = 0; i < numChildren; ++i)
				{
					GameObject srcChildGO = srcChildGameObjects[i];

					GameObject targetChildGO = HEU_GeneralUtility.GetGameObjectByName(unprocessedTargetChildren, srcChildGO.name);
					if (targetChildGO == null)
					{
						targetChildGO = new GameObject(srcChildGO.name);
						targetChildGO.transform.parent = targetTransform;
					}
					else
					{
						if (bDeleteExistingComponents)
						{
							HEU_GeneralUtility.DestroyGeneratedMeshMaterialsLODGroups(targetChildGO, bDontDeletePersistantResources);
						}

						unprocessedTargetChildren.Remove(targetChildGO);

						// Update transform of each existing instance
						HEU_GeneralUtility.CopyLocalTransformValues(srcChildGO.transform, targetChildGO.transform);

						if (bReconnectPrefabInstances && HEU_EditorUtility.IsPrefabInstance(srcChildGO))
						{
							// Reconnect back to the prefab if the source was a prefab instance
							GameObject prefabSource = HEU_EditorUtility.GetPrefabParent(srcChildGO) as GameObject;
							if (prefabSource != null)
							{
								targetChildGO = HEU_EditorUtility.ConnectGameObjectToPrefab(targetChildGO, prefabSource);

								// Update transform of each existing instance again since prefab connect above resets it
								HEU_GeneralUtility.CopyLocalTransformValues(srcChildGO.transform, targetChildGO.transform);

								continue;
							}
						}
					}

					// Copy component data
					CopyGameObjectComponents(srcChildGO, targetChildGO, assetName, sourceToTargetMeshMap, sourceToCopiedMaterials, bWriteMeshesToAssetDatabase, ref bakedAssetPath, 
						ref assetDBObject, assetObjectFileName, bDeleteExistingComponents, bDontDeletePersistantResources);
				}

				if (unprocessedTargetChildren.Count > 0)
				{
					// Clean up any children that we haven't updated as they don't exist in the source
					HEU_GeneralUtility.DestroyBakedGameObjects(unprocessedTargetChildren);
				}
			}
			else
			{
				// Not an instancer, regular object (could also be instanced)
				// TODO: For instanced object, should we not instantiate if it is not visible?

				if (bDeleteExistingComponents)
				{
					HEU_GeneralUtility.DestroyGeneratedMeshMaterialsLODGroups(targetGO, bDontDeletePersistantResources);
				}

				// Copy component data
				CopyGameObjectComponents(outputGameObject, targetGO, assetName, sourceToTargetMeshMap, sourceToCopiedMaterials, bWriteMeshesToAssetDatabase, ref bakedAssetPath, 
					ref assetDBObject, assetObjectFileName, bDeleteExistingComponents, bDontDeletePersistantResources);

			}
		}

		/// <summary>
		/// Processs and build the mesh for this part.
		/// </summary>
		/// <param name="session">Active session to use.</param>
		/// <param name="bGenerateUVs">Whether to generate UVs manually.</param>
		/// <param name="bGenerateTangents">Whether to generate tangents manually.</param>
		/// <param name="bGenerateNormals">Whether to generate normals manually.</param>
		/// <returns>True if successfully built the mesh.</returns>
		public bool GenerateMesh(HEU_SessionBase session, bool bGenerateUVs, bool bGenerateTangents, bool bGenerateNormals, bool bUseLODGroups)
		{
			if(OutputGameObject == null)
			{
				return false;
			}

			if (IsPartCurve())
			{
				_curve.GenerateMesh(OutputGameObject);

				// When a Curve asset is used as input node, it creates this editable and useless curve part type.
				// For now deleting it as it causes issues on recook (from scene load), as well as unnecessary curve editor UI.
				// Should revisit sometime in the future to review this.
				return (_curve != null && _curve.GetNumPoints() > 0);
			}
			else
			{
				bool bResult = true;

				if (MeshVertexCount > 0)
				{
					// Get the geometry and material information from Houdini

					HEU_HoudiniAsset asset = ParentAsset;
					if (asset == null)
					{
						Debug.LogErrorFormat("Asset not found. Unable to generate mesh for part {0}!", _partName);
						return false;
					}

					List<HEU_MaterialData> materialCache = asset.GetMaterialCache();

					HEU_GenerateGeoCache geoCache = HEU_GenerateGeoCache.GetPopulatedGeoCache(session, ParentAsset.AssetID, _geoID, _partID, bUseLODGroups,
						materialCache, asset.GetValidAssetCacheFolderPath());
					if (geoCache == null)
					{
						// Failed to get necessary info for generating geometry.
						return false;
					}

					List<HEU_GeoGroup> LODGroupMeshes = null;
					int defaultMaterialKey = 0;

					// Build the GeoGroup using points or vertices
					if (asset.GenerateMeshUsingPoints)
					{
						bResult = HEU_GenerateGeoCache.GenerateGeoGroupUsingGeoCachePoints(session, geoCache, bGenerateUVs, bGenerateTangents, bGenerateNormals, bUseLODGroups, IsPartInstanced(),
							out LODGroupMeshes, out defaultMaterialKey);
					}
					else
					{
						bResult = HEU_GenerateGeoCache.GenerateGeoGroupUsingGeoCacheVertices(session, geoCache, bGenerateUVs, bGenerateTangents, bGenerateNormals, bUseLODGroups, IsPartInstanced(), 
							out LODGroupMeshes, out defaultMaterialKey);
					}

					// Now generate and attach meshes and materials
					if (bResult)
					{
						int numLODs = LODGroupMeshes != null ? LODGroupMeshes.Count : 0;
						if (numLODs > 1)
						{
							bResult = HEU_GenerateGeoCache.GenerateLODMeshesFromGeoGroups(session, LODGroupMeshes, geoCache, _generatedOutput, defaultMaterialKey, bGenerateUVs, bGenerateTangents, bGenerateNormals, IsPartInstanced());
						}
						else if (numLODs == 1)
						{
							bResult = HEU_GenerateGeoCache.GenerateMeshFromSingleGroup(session, LODGroupMeshes[0], geoCache, _generatedOutput, defaultMaterialKey, bGenerateUVs, bGenerateTangents, bGenerateNormals, IsPartInstanced());
						}
						else
						{
							// Set return state to false if no mesh and not a collider type
							bResult = (geoCache._colliderType != HEU_GenerateGeoCache.ColliderType.NONE);
						}

						HEU_GenerateGeoCache.UpdateCollider(geoCache, _generatedOutput._outputData._gameObject);
					}
				}
				else if(IsPartInstancer() || IsObjectInstancer())
				{
					// Always returning true for meshes without geometry that are instancers. These
					// are handled after this.
					bResult = true;
				}
				else
				{
					// No geometry -> return false to clean up
					bResult = false;
				}

				
				return bResult;
			}
		}

		public void ProcessCurvePart(HEU_SessionBase session)
		{
			HEU_HoudiniAsset parentAsset = ParentAsset;

			bool bNewCurve = (_curve == null);
			if(bNewCurve)
			{
				_curve = HEU_Curve.CreateSetupCurve(parentAsset, _geoNode.Editable, _partName, _geoID, false);
			}
			else
			{
				_curve.UploadParameterPreset(session, _geoID, parentAsset);
			}

			_curve.SyncFromParameters(session, parentAsset);
			_curve.UpdateCurve(session, _partID);

			if(bNewCurve)
			{
				_curve.DownloadAsDefaultPresetData(session);
			}
		}

		public void SyncAttributesStore(HEU_SessionBase session, HAPI_NodeId geoID, ref HAPI_PartInfo partInfo)
		{
			if(_attributesStore == null)
			{
				_attributesStore = ScriptableObject.CreateInstance<HEU_AttributesStore>();
			}

			HEU_HoudiniAsset parentAsset = ParentAsset;
			if (parentAsset != null)
			{
				_attributesStore.SyncAllAttributesFrom(session, parentAsset, geoID, ref partInfo, OutputGameObject);

				parentAsset.AddAttributeStore(_attributesStore);
			}
		}

		public void SetupAttributeGeometry(HEU_SessionBase session)
		{
			if (_attributesStore != null)
			{
				HEU_HoudiniAsset parentAsset = ParentAsset;
				if (parentAsset != null)
				{
					_attributesStore.SetupMeshAndMaterials(parentAsset, _partType, OutputGameObject);
				}
			}
		}

		public void DestroyAttributesStore()
		{
			if(_attributesStore != null)
			{
				HEU_HoudiniAsset parentAsset = ParentAsset;
				if (parentAsset != null)
				{
					parentAsset.RemoveAttributeStore(_attributesStore);

					_attributesStore.DestroyAllData(parentAsset);
				}

				HEU_GeneralUtility.DestroyImmediate(_attributesStore);
				_attributesStore = null;
			}
		}

		/// <summary>
		/// Fill in the objInstanceInfos list with the HEU_ObjectInstanceInfos used by this part.
		/// </summary>
		/// <param name="objInstanceInfos">List to fill in</param>
		public void PopulateObjectInstanceInfos(List<HEU_ObjectInstanceInfo> objInstanceInfos)
		{
			objInstanceInfos.AddRange(_objectInstanceInfos);
		}

		/// <summary>
		/// Set object instance infos from the given part into this.
		/// </summary>
		/// <param name="otherPart"></param>
		public void SetObjectInstanceInfos(List<HEU_ObjectInstanceInfo> sourceObjectInstanceInfos)
		{
			int numSourceInfos = sourceObjectInstanceInfos.Count;
			for (int i = 0; i < numSourceInfos; ++i)
			{
				sourceObjectInstanceInfos[i]._instances.Clear();
				sourceObjectInstanceInfos[i]._partTarget = this;

				_objectInstanceInfos.Add(sourceObjectInstanceInfos[i]);
			}
		}

		/// <summary>
		/// Return list of HEU_ObjectInstanceInfo used by this part.
		/// </summary>
		/// <returns></returns>
		public List<HEU_ObjectInstanceInfo> GetObjectInstanceInfos()
		{
			return _objectInstanceInfos;
		}

		/// <summary>
		/// Helper to create a HEU_ObjectInstanceInfo, representing an instanced object
		/// containing list of instances.
		/// Adds this new object to _objectInstanceInfos.
		/// </summary>
		/// <param name="instancedObject">The source instanced object</param>
		/// <param name="instancedObjectNodeID">If instancedObject is a Houdini Engine object node, then this would be its node ID</param>
		/// <param name="instancedObjectPath">Path in Unity to the instanced object (could be empty or null if not a Unity instanced object)</param>
		/// <returns>The created object</returns>
		private HEU_ObjectInstanceInfo CreateObjectInstanceInfo(GameObject instancedObject, HAPI_NodeId instancedObjectNodeID, string instancedObjectPath)
		{
			HEU_ObjectInstanceInfo newInfo = ScriptableObject.CreateInstance<HEU_ObjectInstanceInfo>();
			newInfo._partTarget = this;
			newInfo._instancedObjectNodeID = instancedObjectNodeID;
			newInfo._instancedObjectPath = instancedObjectPath;

			HEU_InstancedInput input = new HEU_InstancedInput();
			input._instancedGameObject = instancedObject;
			newInfo._instancedInputs.Add(input);

			_objectInstanceInfos.Add(newInfo);
			return newInfo;
		}

		/// <summary>
		/// Returns HEU_ObjectInstanceInfo with matching _instancedObjectPath.
		/// </summary>
		/// <param name="path">The path to match with _instancedObjectPath</param>
		/// <returns>HEU_ObjectInstanceInfo with matching _instancedObjectPath or null if none found</returns>
		public HEU_ObjectInstanceInfo GetObjectInstanceInfoWithObjectPath(string path)
		{
			int numSourceInfos = _objectInstanceInfos.Count;
			for (int i = 0; i < numSourceInfos; ++i)
			{
				if(_objectInstanceInfos[i]._instancedObjectPath.Equals(path))
				{
					return _objectInstanceInfos[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Returns HEU_ObjectInstanceInfo with matching objNodeID
		/// </summary>
		/// <param name="objNodeID">The Houdini Engine node ID to match</param>
		/// <returns>HEU_ObjectInstanceInfo with matching objNodeID or null if none found</returns>
		public HEU_ObjectInstanceInfo GetObjectInstanceInfoWithObjectID(HAPI_NodeId objNodeID)
		{
			int numSourceInfos = _objectInstanceInfos.Count;
			for (int i = 0; i < numSourceInfos; ++i)
			{
				if (_objectInstanceInfos[i]._instancedObjectNodeID == objNodeID)
				{
					return _objectInstanceInfos[i];
				}
			}
			return null;
		}

		public void SetTerrainOffsetPosition(Vector3 offsetPosition)
		{
			_terrainOffsetPosition = offsetPosition;
		}

		public void SetTerrainData(TerrainData terrainData)
		{
			if (HEU_AssetDatabase.ContainsAsset(terrainData))
			{
				_assetDBTerrainData = terrainData;
			}
			else
			{
				if (_assetDBTerrainData != null && HEU_AssetDatabase.ContainsAsset(_assetDBTerrainData))
				{
					HEU_AssetDatabase.DeleteAsset(_assetDBTerrainData);
				}
				_assetDBTerrainData = null;

				string objectName = ParentGeoNode.ObjectNode != null ? ParentGeoNode.ObjectNode.ObjectName : "";
				string assetPathName = string.Format("Asset_{0}_{1}_{2}_TerrainData.asset", objectName, ParentGeoNode.GeoName, PartID);
				ParentAsset.AddToAssetDBCache(assetPathName, terrainData, ref _assetDBTerrainData);
			}
		}

		public static string AppendBakedCloneName(string name)
		{
			return name + HEU_Defines.HEU_BAKED_CLONE;
		}

		public override string ToString()
		{
			return (!string.IsNullOrEmpty(_partName) ? ("Part: " + _partName) : base.ToString());
		}

		/// <summary>
		/// Destroy list of parts and their data.
		/// </summary>
		public static void DestroyParts(List<HEU_PartData> parts)
		{
			int numParts = parts.Count;
			for (int i = 0; i < numParts; ++i)
			{
				DestroyPart(parts[i]);
			}
			parts.Clear();
		}

		/// <summary>
		/// Destroy the given part and its data.
		/// </summary>
		/// <param name="part"></param>
		public static void DestroyPart(HEU_PartData part)
		{
			part.DestroyAllData();
			HEU_GeneralUtility.DestroyImmediate(part);
		}
	}

}   // HoudiniEngineUnity
						 
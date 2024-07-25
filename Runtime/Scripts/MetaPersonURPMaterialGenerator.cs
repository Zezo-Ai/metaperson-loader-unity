/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, July 2024
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Loader
{
	public class MetaPersonURPMaterialGenerator : MetaPersonMaterialGenerator
	{
		public List<Material> additionalHaircutMaterials = new List<Material>();

		protected override void ConfigureMeshRenderer(SkinnedMeshRenderer meshRenderer, AvatarPart avatarPart)
		{
			if (avatarPart == AvatarPart.Haircut)
			{
				if (additionalHaircutMaterials.Count > 0) 
				{
					Material mainMaterial = meshRenderer.sharedMaterial;
					List<Material> sharedMaterials = new List<Material>();
					foreach(var templateHaircutMaterial in additionalHaircutMaterials)
					{
						Material newMaterial = new Material(templateHaircutMaterial);
						CopyMaterialTextures(mainMaterial, newMaterial);
						sharedMaterials.Add(newMaterial);
					}
					sharedMaterials.Add(mainMaterial);
					meshRenderer.sharedMaterials = sharedMaterials.ToArray();
				}
			}
			else
				base.ConfigureMeshRenderer(meshRenderer, avatarPart);
		}

		private void CopyMaterialTextures(Material srcMaterial, Material dstMaterial)
		{
			var shader = srcMaterial.shader;
			int propertyCount = ShaderUtil.GetPropertyCount(shader);

			for (int i = 0; i < propertyCount; i++)
			{
				if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
				{
					string propertyName = ShaderUtil.GetPropertyName(shader, i);
					Texture texture = srcMaterial.GetTexture(propertyName);
					dstMaterial.SetTexture(propertyName, texture);
				}
			}
		}
	}
}

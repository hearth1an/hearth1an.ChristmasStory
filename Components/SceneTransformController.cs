using UnityEngine;

namespace ChristmasStory.Components
{
	public class SceneTransformController : MonoBehaviour
	{
		public void TitleScreenChanges()
		{
			var snowTreeTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow_Tree_NEW.png");
			var snowTerrainTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Terrain_BH_SnowTransition_d.png");
			var snowFoliageTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Snow.png");
			var leavesMaterial = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Trees/Tree_TH_SaplingSequoia (1)/Redwood_Leaves_1").GetComponent<MeshRenderer>().materials[0];
			var foliageMaterial = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Props/Foliage/Foliage_TH_CraterGrass (3)").GetComponent<MeshRenderer>().materials[0];
			var trees = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Trees").GetComponentsInChildren<Renderer>();
			var terrain = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Terrain_THM_TitleScreen").GetComponent<MeshRenderer>();
			var foliage = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Props/Foliage").GetComponentsInChildren<Renderer>();

			leavesMaterial.mainTexture = snowTreeTexture;

			foliageMaterial.mainTexture = snowFoliageTexture;
			terrain.material.color = Color.white;

			foreach (MeshRenderer meshRenderer in trees)
			{
				if (meshRenderer.material.name.Contains("Tree_TS_RedwoodLeaves_mat (Instance)"))
				{
					meshRenderer.sharedMaterial = leavesMaterial;
					leavesMaterial.color = Color.white;
				}
			}
			foreach (MeshRenderer meshRenderer in foliage)
			{
				if (meshRenderer.material.name.Contains("Foliage_TS_Grass (Instance)"))
				{
					meshRenderer.sharedMaterial = foliageMaterial;
				}
			}
		}
		public void CreditsMusic()
		{
			var addMusic = GameObject.Find("AudioSource").GetComponent<OWAudioSource>();
			var newMusic = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/Christmas_Instrument.mp3");
			addMusic._audioLibraryClip = AudioType.None;
			addMusic.clip = newMusic;
			addMusic.Play();
		}


	}
}

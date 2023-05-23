using ChristmasStory.Utility;
using UnityEngine;
using NewHorizons.Handlers;
using NewHorizons.Utility;

namespace ChristmasStory.Components
{
	public class InviteStone : OWItem
	{
		public BoxCollider _collider;

		public override string GetDisplayName()
		{
			return TranslationHandler.GetTranslation("INVITE_STONE_ITEM", TranslationHandler.TextType.UI);
		}

		public override void Awake()
		{			
			_type = (ItemType)100;
			base.Awake();
		}

		public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
		{
			base.DropItem(position, normal, parent, sector, customDropTarget);
			EnableInteraction(true);
			SetColliderActivation(true);			
			PlayerEffectController.PlayAudioOneShot(AudioType.ToolItemSharedStoneDrop);
		}

		public override void PickUpItem(Transform holdTranform)
		{
			base.PickUpItem(holdTranform);
			transform.localPosition = new Vector3(0.2f, 0f, 0.1f);
			PlayerEffectController.PlayAudioOneShot(AudioType.ToolItemSharedStonePickUp);
		}

		public override void SocketItem(Transform socketTransform, Sector sector)
		{
			base.SocketItem(socketTransform, sector);
			EnableInteraction(false);
			SetColliderActivation(true);

		}

		
	}
}

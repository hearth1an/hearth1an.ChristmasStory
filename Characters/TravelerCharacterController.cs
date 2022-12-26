namespace ChrismasStory.Characters
{
	// For the Hearthian travelers. They are all based around regular dialogue
	internal abstract class TravelerCharacterController : BaseCharacterController
	{
		public CharacterDialogueTree dialogue;
		public CharacterDialogueTree dialogueShip;
		public CharacterDialogueTree dialogueVillage;

		public override void Start()
		{
			if (dialogue != null)
			{
				dialogue.OnStartConversation += Dialogue_OnStartConversation;
				dialogue.OnEndConversation += Dialogue_OnEndConversation;
			}
			if (dialogueShip != null)
			{
				dialogueShip.OnStartConversation += Dialogue_OnStartConversation;
				dialogueShip.OnEndConversation += Dialogue_OnEndConversation;
			}
			if (dialogueVillage != null)
			{
				dialogueVillage.OnStartConversation += Dialogue_OnStartConversation;
				dialogueVillage.OnEndConversation += Dialogue_OnEndConversation;
			}

			base.Start();
		}

		public virtual void OnDestroy()
		{
			if (dialogue != null)
			{
				dialogue.OnStartConversation -= Dialogue_OnStartConversation;
				dialogue.OnEndConversation -= Dialogue_OnEndConversation;
			}
			if (dialogueShip != null)
			{
				dialogueShip.OnStartConversation -= Dialogue_OnStartConversation;
				dialogueShip.OnEndConversation -= Dialogue_OnEndConversation;
			}
			if (dialogueVillage != null)
			{
				dialogueVillage.OnStartConversation -= Dialogue_OnStartConversation;
				dialogueVillage.OnEndConversation -= Dialogue_OnEndConversation;
			}
		}

		protected abstract void Dialogue_OnStartConversation();
		protected abstract void Dialogue_OnEndConversation();
	}
}

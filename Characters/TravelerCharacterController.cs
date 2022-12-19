namespace ChrismasStory.Characters
{
	// For the Hearthian travelers. They are all based around regular dialogue
	internal abstract class TravelerCharacterController : BaseCharacterController
	{
		public CharacterDialogueTree dialogue;

		public virtual void Start()
		{
			if (dialogue != null)
			{
				dialogue.OnStartConversation += Dialogue_OnStartConversation;
				dialogue.OnEndConversation += Dialogue_OnEndConversation;
			}
		}

		public void OnDestroy()
		{
			if (dialogue != null)
			{
				dialogue.OnStartConversation -= Dialogue_OnStartConversation;
				dialogue.OnEndConversation -= Dialogue_OnEndConversation;
			}
		}

		protected abstract void Dialogue_OnStartConversation();
		protected abstract void Dialogue_OnEndConversation();
	}
}

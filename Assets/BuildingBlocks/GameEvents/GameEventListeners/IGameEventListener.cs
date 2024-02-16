public interface IGameEventListener<Type>
{
	void OnEventRaised(Type type);
}

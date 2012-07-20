namespace CrossUI
{
	public interface IRecorder<in TargetT>
	{
		void Replay(TargetT target);
	}
}

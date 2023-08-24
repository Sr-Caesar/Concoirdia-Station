using System;
namespace ConcordiaStation.SyncApp
{
	public interface ISynchronizer
	{
		public Task SynchronizeDataAsync();
	}
}


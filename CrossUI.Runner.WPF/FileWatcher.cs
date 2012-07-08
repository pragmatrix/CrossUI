using System;
using System.IO;
using System.Windows.Threading;
using CrossUI.Toolbox;
using Toolbox;

namespace CrossUI.Runner.WPF
{
	sealed class FileWatcher : IDisposable
	{
		readonly string _path;
		const int InitialBufferSize = 0x8000;

		public Action Changed;
			
		readonly Dispatcher _dispatcher;
		readonly FileSystemWatcher _watcher;

		public FileWatcher(string directory, string filter)
		{
			_path = directory;
			_dispatcher = Dispatcher.CurrentDispatcher;

			_watcher = new FileSystemWatcher();
			_watcher.BeginInit();
			_watcher.Path = directory;
			_watcher.Filter = filter;
			_watcher.IncludeSubdirectories = false;
			_watcher.InternalBufferSize = InitialBufferSize;

			_watcher.NotifyFilter
				= NotifyFilters.DirectoryName
				| NotifyFilters.FileName
				| NotifyFilters.LastWrite
				| NotifyFilters.Attributes
				| NotifyFilters.Security
				| NotifyFilters.Size;

			// note: all events are on threadpool threads!
			_watcher.Created += onCreated;
			_watcher.Deleted += onDeleted;
			_watcher.Changed += onChanged;
			_watcher.Renamed += onRenamed;
			_watcher.Error += onError;

			_watcher.EndInit();

			_watcher.EnableRaisingEvents = true;
		}

		void onCreated(object sender, FileSystemEventArgs e)
		{
			dispatchChanged();
		}

		void onDeleted(object sender, FileSystemEventArgs e)
		{
			dispatchChanged();
		}

		void onChanged(object sender, FileSystemEventArgs e)
		{
			dispatchChanged();
		}

		void onRenamed(object sender, RenamedEventArgs e)
		{
			dispatchChanged();
		}

		void onError(object sender, ErrorEventArgs e)
		{
			throw new Exception("{0}: File system watcher error {1}".format(_path, e.GetException().Message));
		}

		public void dispatchChanged()
		{
			_dispatcher.BeginInvoke((Action)(() =>
				{
					if (_watcher.EnableRaisingEvents)
						Changed.raise();
				}));
		}

		public void Dispose()
		{
			_watcher.EnableRaisingEvents = false;
			_watcher.Dispose();
		}
	}
}

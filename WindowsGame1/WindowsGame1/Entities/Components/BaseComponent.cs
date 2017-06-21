using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entities.Components {
	interface IComponent : IObservable<IComponent>, IObserver<IComponent>, IDisposable {

	}

	abstract class BaseComponent : IComponent {
		public List<IComponent> observers;

		//IObservable
		public IDisposable Subscribe(IComponent observer) {
			observers.Add(observer);
			return this;
		}

		//IObserver
		public void OnCompleted() {
			throw new NotImplementedException();
		}

		public void OnError(Exception error) {
			throw new NotImplementedException();
		}

		public void OnNext(IComponent value) {
			throw new NotImplementedException();
		}

		//IDisposable
		public void Dispose() {
			//Clear observers
		}
	}
}

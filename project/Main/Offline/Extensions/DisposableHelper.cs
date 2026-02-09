namespace Main.Offline.Extensions
{
	using System;

	public class DisposableHelper : IDisposable
    {
        private readonly Action end;

        // When the object is create, write "begin" function
        public DisposableHelper(Action begin, Action end)
        {
            this.end = end;
            begin();
        }

        // When the object is disposed (end of using block), write "end" function
        public void Dispose()
        {
            end();
        }
    }
}

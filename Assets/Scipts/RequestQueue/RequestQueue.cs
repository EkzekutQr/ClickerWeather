using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class RequestQueue
{
    private Queue<UniTask> _requestQueue = new Queue<UniTask>();
    private bool _isProcessing = false;

    public void EnqueueRequest(UniTask request)
    {
        _requestQueue.Enqueue(request);
        if (!_isProcessing)
        {
            ProcessQueue().Forget();
        }
    }

    private async UniTaskVoid ProcessQueue()
    {
        _isProcessing = true;
        while (_requestQueue.Count > 0)
        {
            var request = _requestQueue.Dequeue();
            await request;
        }
        _isProcessing = false;
    }
}
using Batch_Manager.Models;

namespace Batch_Manager.Services
{
    public interface IBatch
    {
        Batch GetBatches(string batchId);
        BatchResponse CreateBatch(Batch batch);
        string AddFile(string batchGuid, string fileName, string fileType, string fileSize);
    }
}

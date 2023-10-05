using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Batch_Manager.DatabaseContext;
using Batch_Manager.DataTableObjects;
using Batch_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Batch_Manager.Services
{
    public class BatchService : IBatch
    {
        private readonly BatchContext _batchContext;
        private readonly IConfiguration _configuration;
        private readonly IBlobStorageClient _blobStorageClient;
        private readonly IMapper _mapper;
        public BatchService(BatchContext batchContext, IConfiguration configuration, IMapper mapper, IBlobStorageClient blobStorageClient)
        {
            _batchContext = batchContext;
            _configuration = configuration;
            _mapper = mapper;
            _blobStorageClient = blobStorageClient;
        }
        public BatchResponse CreateBatch(Batch batch)
        {
            BatchResponse batchResponse = new BatchResponse();
            if (batch == null)
                throw new ArgumentNullException(nameof(batch));

            string batchId = Guid.NewGuid().ToString();
            batch.BatchId = batchId;

            foreach (var file in batch.Files)
            {
                var batchFile = _mapper.Map<BatchFile>(file);
                batchFile.BatchGuid = batchId;
                batchFile.FileContent = new byte[] { };
                batchFile.Batch = batch;
                _batchContext.File.Add(batchFile);
            }
            _batchContext.Batch.Add(batch);
            _batchContext.SaveChanges();
            CreateBatchContainerOnAzureStorage(batchId);
            batchResponse.BatchId = batchId;
            return batchResponse;
        }

        public Batch GetBatches(string batchId)
        {
            Batch batch = new Batch();
            var batchQuery = (from batchEntity in _batchContext.Batch
                              where batchEntity.BatchId == batchId
                              select new
                              {
                                  batchEntity
                              });
            var batchResult = batchQuery.FirstOrDefault();
            if (batchResult == null)
                throw new ApplicationException("No batch record found with give batch id.");
            batch = batchResult.batchEntity;

            batch.Acl = _batchContext.Acl.Where(x => x.Id == batch.Id).Select(x => x).FirstOrDefault();
            var groupsQuery = (from groups in _batchContext.Group
                               where groups.AclId == batch.Acl.AclId
                               select new
                               {
                                   groups
                               });
            var groupsResult = groupsQuery.ToList();
            List<Group> groupList = groupsResult.Select(x => x.groups).ToList();
            batch.Acl.ReadGroups = new List<Group>();
            foreach (var item in groupList)
            {
                batch.Acl.ReadGroups.Add(item);
            }

            var usersQuery = (from users in _batchContext.User
                              where users.AclId == batch.Acl.AclId
                              select new
                              {
                                  users
                              });
            var usersResult = usersQuery.ToList();
            List<User> userList = usersResult.Select(x => x.users).ToList();
            batch.Acl.ReadUsers = new List<User>();
            foreach (var item in userList)
            {
                batch.Acl.ReadUsers.Add(item);
            }

            var attributesQuery = (from attributes in _batchContext.BatchAttribute
                                   where attributes.Id == batch.Id
                                   select new
                                   {
                                       attributes
                                   });
            var attributeResult = attributesQuery.ToList();
            batch.Attributes = attributeResult.Select(x => x.attributes).ToList();

            var filesQuery = (from files in _batchContext.File.Include("Attributes")
                              where files.Id == batch.Id
                              select new
                              {
                                  files
                              });
            var filesResult = filesQuery.ToList();
            var batchFiles = filesResult.Select(x => x.files).ToList<BatchFile>();
            batch.Files = new List<BatchFileDto>();
            foreach (var file in batchFiles)
            {
                batch.Files.Add(_mapper.Map<BatchFileDto>(file));
            }

            foreach (var item in batchFiles)
            {
                var fileAttributesQuery = (from attributes in _batchContext.FileAttribute
                                           where attributes.Id == item.Id
                                           select new
                                           {
                                               attributes
                                           });
                var result = fileAttributesQuery.ToList();
                item.Attributes = result.Select(x => x.attributes).ToList();
            }
            return batch;
        }

        public string AddFile(string batchGuid, string fileName, string fileType, string fileSize)
        {
            BatchFile file = new BatchFile();
            file.FileName = fileName;
            file.MimeType = fileType;
            file.FileSize = fileSize;
            file.BatchGuid = batchGuid;
            var rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (rootDirectory.Contains("bin"))
                rootDirectory = rootDirectory.Substring(0, rootDirectory.IndexOf("bin"));

            var filePath = Path.Combine(rootDirectory, "UploadFiles\\SampleDoc.txt");
            file.FileContent = System.IO.File.ReadAllBytes(filePath);
            file.Attributes = new List<FileAttribute>();
            file.Batch = _batchContext.Batch.Where(x => x.BatchId == batchGuid).Select(x => x).FirstOrDefault();
            _batchContext.File.Add(file);
            _batchContext.SaveChanges();
            string blobName = UploadFileOnAzureStorage(filePath, batchGuid.ToString());
            return $"File Created and uploaded to blob in container : {blobName}";
        }

        private void CreateBatchContainerOnAzureStorage(string batchId)
        {
            BlobContainerClient blobContainer = _blobStorageClient.GetBlobContainerClient(_configuration, "container" + batchId);
            blobContainer.CreateIfNotExists();
        }
        private string UploadFileOnAzureStorage(string filePath, string batchId)
        {
            BlobContainerClient blobContainer = _blobStorageClient.GetBlobContainerClient(_configuration, "container" + batchId);

            string blobName = Path.GetFileName(filePath).Split(".")[0];
            BlobClient blobClient = blobContainer.GetBlobClient(blobName);
            blobClient.Upload(filePath, true);
            return blobClient.BlobContainerName;
        }
    }
}

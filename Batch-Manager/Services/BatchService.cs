using Batch_Manager.DatabaseContext;
using Batch_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Batch_Manager.Services
{
    public class BatchService : IBatch
    {
        private readonly BatchContext _batchContext;
        private readonly IConfiguration _configuration;
        public BatchService(BatchContext batchContext, IConfiguration configuration)
        {
            _batchContext = batchContext;
            _configuration = configuration;
        }
        public BatchResponse CreateBatch(Batch batch)
        {
            BatchResponse batchResponse = new BatchResponse();
            if (batch == null)
                throw new ArgumentNullException(nameof(batch));

            string batchId = Guid.NewGuid().ToString();
            batch.BatchId = batchId;
            _batchContext.Batch.Add(batch);
            _batchContext.SaveChanges();

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
            batch.Files = filesResult.Select(x => x.files).ToList();

            foreach (var item in batch.Files)
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
    }
}
